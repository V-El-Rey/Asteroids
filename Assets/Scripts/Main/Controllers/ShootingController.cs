using Assets.Scripts.Main.Models;
using Assets.Scripts.Main.Views;
using System.Collections.Generic;
using UnityEngine;

public class ShootingController : IBaseController, IEnterController, IExitController, IUpdateController
{
    private PlayerView m_playerView;
    private PoolsManager m_poolsManager;
    private ShootingModel m_shootingModel;
    private MainUIModel m_mainUIModel;

    private List<BaseView> m_bullets;
    private LaserView m_enabledLaser;
    private HashSet<int> m_indexesToRemove;

    private float m_laserWorkTime;
    private float m_laserCooldownTime;

    public ShootingController(PlayerModel playerModel, PoolsManager poolsManager, ShootingModel shootingModel, MainUIModel mainUIModel)
    {
        m_poolsManager = poolsManager;
        m_shootingModel = shootingModel;
        m_mainUIModel = mainUIModel;
        m_playerView = playerModel.PlayerView as PlayerView;
        m_bullets = new List<BaseView>();
        m_indexesToRemove = new HashSet<int>();
        m_shootingModel.laserShotsLeft = m_shootingModel.maximumLaserShots;
    }

    public void OnEnterExecute()
    {
        m_laserWorkTime = m_shootingModel.laserCooldown;
        m_laserCooldownTime = m_shootingModel.laserCooldown;
        m_shootingModel.onBulletShot += LaunchBullet;
        m_shootingModel.onLaserShot += LaunchLaser;
    }

    private void LaunchBullet()
    {
        var poolObject = m_poolsManager.GetObjectFromPool(GameobjectsNameKeys.Bullet) as BulletView;
        poolObject.onCollision = null;
        poolObject.objectTransform.position = m_playerView.muzzle.position;
        poolObject.objectTransform.rotation = m_playerView.muzzle.rotation;
        poolObject.onCollision += OnBulletCollision;
        m_bullets.Add(poolObject);
    }
    private void LaunchLaser()
    {
        if (m_enabledLaser != null) return;
        var poolObject = m_poolsManager.GetObjectFromPool(GameobjectsNameKeys.Laser) as LaserView;
        poolObject.onCollision = null;
        poolObject.objectTransform.position = m_playerView.muzzle.position;
        poolObject.objectTransform.rotation = m_playerView.muzzle.rotation;
        poolObject.onCollision += OnLaserCollision;
        m_laserWorkTime = m_shootingModel.laserBeamTime;
        m_enabledLaser = poolObject;
        m_shootingModel.laserShotsLeft--;
    }

    private void OnLaserCollision()
    {
        m_mainUIModel.score++;
    }

    private void OnBulletCollision(BulletView view, string tag)
    {
        if (tag == GameobjectsNameKeys.Asteroid || tag == GameobjectsNameKeys.UFO)
        {
            RemoveProjectile(view);
            m_mainUIModel.score++;
        }
    }

    private void RemoveProjectile(BaseView bullet)
    {
        m_bullets.Remove(bullet);
        m_poolsManager.ReturnObjectToPool(bullet);
    }


    public void OnExitExecute()
    {
        m_shootingModel.onBulletShot -= LaunchBullet;
        m_shootingModel.onLaserShot -= LaunchLaser;
    }

    public void OnUpdateExecute()
    {
        foreach (var bullet in m_bullets)
        {
            bullet.transform.position += bullet.transform.up * Time.deltaTime * 5.0f;
            if (CommonUtils.IsOutOfScreenBorders(Camera.main.WorldToScreenPoint(bullet.transform.position)))
            {
                m_indexesToRemove.Add(m_bullets.IndexOf(bullet));
            }
        }
        foreach (var index in m_indexesToRemove)
        {
            RemoveProjectile(m_bullets[index]);
        }
        if (m_indexesToRemove.Count > 0)
        {
            m_indexesToRemove.Clear();
        }
        if (m_laserWorkTime > 0 && m_enabledLaser != null)
        {
            m_laserWorkTime -= Time.deltaTime;
            m_enabledLaser.objectTransform.position = m_playerView.muzzle.position;
            m_enabledLaser.objectTransform.rotation = m_playerView.muzzle.rotation;
            if (m_laserWorkTime <= 0.0f)
            {
                RemoveProjectile(m_enabledLaser);
                m_enabledLaser = null;
            }
        }

        if (m_shootingModel.laserShotsLeft < m_shootingModel.maximumLaserShots)
        {
            m_laserCooldownTime -= Time.deltaTime;
            if (m_laserCooldownTime <= 0.0f)
            {
                m_shootingModel.laserShotsLeft++;
                m_laserCooldownTime = m_shootingModel.laserCooldown;
            }
        }
        m_mainUIModel.laserShotsLeft = m_shootingModel.laserShotsLeft;
        m_mainUIModel.laserCooldown = m_laserCooldownTime;
    }
}
