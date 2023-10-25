using Assets.Scripts.Main.Models;
using Assets.Scripts.Main.Views;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Main.Controllers
{
    public class EnemiesController : IBaseController, IUpdateController, IEnterController
    {
        private PoolsManager m_poolsManager;
        private MainUIModel m_mainUIModel;
        private PlayerModel m_playerModel;
        private EnemiesModel m_enemiesModel;

        private List<AsteroidView> m_asteroids;
        private List<UFOView> m_ufos;
        private HashSet<int> m_asteroidsToRemove;

        private int screenWidth;
        private int screenHeight;

        private float m_asteroidsCooldown;
        private float m_ufoCooldown;
        public EnemiesController(PoolsManager poolsManager, PlayerModel playerModel, EnemiesModel enemiesModel)
        {
            m_playerModel = playerModel;
            m_enemiesModel = enemiesModel;
            m_poolsManager = poolsManager;
            m_asteroids = new List<AsteroidView>();
            m_ufos = new List<UFOView>();
            m_asteroidsToRemove = new HashSet<int>();
            m_asteroidsCooldown = 4.0f;
            m_enemiesModel = enemiesModel;
        }

        private void SpawnAsteroid(AsteroidSize size, bool randomSpawn = true, float spawnPointX = 0, float spawnPointY = 0)
        {
            var objName = "";
            switch (size)
            {
                case AsteroidSize.Small: objName = GameobjectsNameKeys.AsteroidSmall; break;
                case AsteroidSize.Medium: objName = GameobjectsNameKeys.AsteroidMedium; break;
                case AsteroidSize.Big: objName = GameobjectsNameKeys.AsteroidBig; break;
            }
            var poolObject = m_poolsManager.GetObjectFromPool(objName) as AsteroidView;
            poolObject.onCollision = null;
            poolObject.onCollision += OnAsteroidCollide;
            poolObject.size = size;
            var screenPointX = 0;
            var screenPointY = 0;
            var directionVector = Vector2.zero;
            if (randomSpawn)
            {
                screenPointX = GetRandomSpawnPosition().Item1;
                screenPointY = GetRandomSpawnPosition().Item2;
                directionVector = new Vector2(screenWidth * 0.5f + Random.Range(-screenWidth / 2, screenWidth / 2), screenHeight * 0.5f + Random.Range(-screenHeight / 2, screenHeight / 2));

            }
            else
            {
                screenPointX = (int)spawnPointX + Random.Range(-20, 20);
                screenPointY = (int)spawnPointY + Random.Range(-20, 20);
                directionVector = CommonUtils.GetRandomVector2(360, 0);
            }


            Vector2 asteroidScreenPosition = new Vector2(screenPointX, screenPointY);
            poolObject.objectTransform.position = Camera.main.ScreenToWorldPoint(asteroidScreenPosition);
            poolObject.objectTransform.rotation = CommonUtils.GetRotationBetweenTwoScreenPoints(asteroidScreenPosition, directionVector);
            poolObject.instantiated = true;
            m_asteroids.Add(poolObject);
        }

        private void SpawnUFO()
        {
            var poolObject = m_poolsManager.GetObjectFromPool(GameobjectsNameKeys.UFO) as UFOView;
            poolObject.onCollision = null;
            poolObject.onCollision += OnUFOCollide;
            var screenPointX = 0;
            var screenPointY = 0;
            var directionVector = Vector2.zero;

            screenPointX = GetRandomSpawnPosition().Item1;
            screenPointY = GetRandomSpawnPosition().Item2;
            directionVector = new Vector2(screenWidth * 0.5f + Random.Range(-screenWidth / 2, screenWidth / 2), screenHeight * 0.5f + Random.Range(-screenHeight / 2, screenHeight / 2));
            Vector2 asteroidScreenPosition = new Vector2(screenPointX, screenPointY);
            poolObject.objectTransform.position = Camera.main.ScreenToWorldPoint(asteroidScreenPosition);
            poolObject.objectTransform.rotation = CommonUtils.GetRotationBetweenTwoScreenPoints(asteroidScreenPosition, directionVector);
            m_ufos.Add(poolObject);
        }

        private void OnUFOCollide(UFOView view, string tag)
        {
            if (tag == GameobjectsNameKeys.Bullet || tag == GameobjectsNameKeys.Laser)
            { 
                m_ufos.Remove(view);
                m_poolsManager.ReturnObjectToPool(view);
            } 
        }

        private void OnAsteroidCollide(AsteroidView asteroid, string tag)
        {
            var asteroidScreenPosition = Camera.main.WorldToScreenPoint(asteroid.objectTransform.position);
            if (tag == GameobjectsNameKeys.Bullet)
            {
                m_asteroids.Remove(asteroid);
                m_poolsManager.ReturnObjectToPool(asteroid);
                if (asteroid.size == AsteroidSize.Big)
                {
                    SpawnAsteroid(AsteroidSize.Medium, false, asteroidScreenPosition.x + Random.Range(-20, 20), asteroidScreenPosition.y + Random.Range(-20, 20));
                    SpawnAsteroid(AsteroidSize.Medium, false, asteroidScreenPosition.x + Random.Range(-20, 20), asteroidScreenPosition.y + Random.Range(-20, 20));
                }
                else if (asteroid.size == AsteroidSize.Medium)
                {
                    SpawnAsteroid(AsteroidSize.Small, false, asteroidScreenPosition.x + Random.Range(-20, 20), asteroidScreenPosition.y + Random.Range(-20, 20));
                    SpawnAsteroid(AsteroidSize.Small, false, asteroidScreenPosition.x + Random.Range(-20, 20), asteroidScreenPosition.y + Random.Range(-20, 20));
                }
            }
            if (tag == GameobjectsNameKeys.Laser)
            {
                m_asteroids.Remove(asteroid);
                m_poolsManager.ReturnObjectToPool(asteroid);
            }
        }

        public void OnEnterExecute()
        {
            m_asteroidsCooldown = m_enemiesModel.asteroidsCooldown;
            m_ufoCooldown = m_enemiesModel.ufosCooldown;
            foreach (var asteroid in m_asteroids)
            {
                m_poolsManager.ReturnObjectToPool(asteroid);
            }
            m_asteroids.Clear();
            foreach (var ufo in m_ufos)
            {
                m_poolsManager.ReturnObjectToPool(ufo);
            }
            m_ufos.Clear();
            screenWidth = Screen.width;
            screenHeight = Screen.height;
        }

        public void OnUpdateExecute()
        {
            m_asteroidsCooldown -= Time.deltaTime;
            if (m_asteroidsCooldown <= 0)
            {
                m_asteroidsCooldown = m_enemiesModel.asteroidsCooldown;
                SpawnAsteroid(AsteroidSize.Big);
            }
            m_ufoCooldown -= Time.deltaTime;
            if (m_ufoCooldown <= 0)
            {
                m_ufoCooldown = m_enemiesModel.ufosCooldown;
                SpawnUFO();
            }

            foreach (var asteroid in m_asteroids)
            {
                asteroid.transform.position += asteroid.transform.right * Time.deltaTime * m_enemiesModel.asteroidSpeed;
                if (!CommonUtils.IsOutOfScreenBorders(Camera.main.WorldToScreenPoint(asteroid.transform.position)) && asteroid.instantiated)
                {
                    asteroid.instantiated = false;
                }
                else if (CommonUtils.IsOutOfScreenBorders(Camera.main.WorldToScreenPoint(asteroid.transform.position)) && !asteroid.instantiated)
                {
                    m_poolsManager.ReturnObjectToPool(asteroid);
                    m_asteroidsToRemove.Add(m_asteroids.IndexOf(asteroid));
                }
            }

            foreach (var ufo in m_ufos)
            {
                var rotationTo = 
                    CommonUtils.GetRotationBetweenTwoScreenPoints(Camera.main.WorldToScreenPoint(m_playerModel.PlayerView.objectTransform.position), 
                    Camera.main.WorldToScreenPoint(ufo.objectTransform.position));
                ufo.objectTransform.rotation = rotationTo;

                ufo.objectTransform.position += -ufo.objectTransform.right * Time.deltaTime * m_enemiesModel.asteroidSpeed;
            }
                foreach (var index in m_asteroidsToRemove)
            {
                m_asteroids.RemoveAt(index);
            }
            if (m_asteroidsToRemove.Count > 0)
            {
                m_asteroidsToRemove.Clear();
            }
        }

        private (int, int) GetRandomSpawnPosition()
        {
            var screenPointX = 0;
            var screenPointY = 0;
            var spawnSideX = Random.Range(0, 2);
            var spawnSideY = Random.Range(0, 2);

            screenPointX = Random.Range(5, screenWidth + 5);
            if (screenPointX > 0 && screenPointX < screenWidth)
            {
                screenPointY = spawnSideY > 0.5f ? Random.Range(screenHeight, screenHeight + 5) : Random.Range(-5, 0);
            }
            else
            {
                screenPointY = Random.Range(-5, screenHeight + 5);
                if (screenPointY > 0 && screenPointY < screenHeight)
                {
                    screenPointX = spawnSideX > 0.5f ? Random.Range(screenWidth, screenWidth + 5) : Random.Range(-5, 0);
                }
            }
            return (screenPointX, screenPointY);

        }
    }
}
