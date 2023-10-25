using Assets.Scripts.Main.Models;
using System;
using UnityEngine;

public class PlayerController : IBaseController, IUpdateController, IEnterController, IExitController
{
    private GameState m_gameState;
    private PoolsManager m_poolsManager;
    private PlayerInputActions.PlayerMovementActions m_playerMovementActions;

    private PlayerModel m_playerModel;
    private MainUIModel m_mainUIModel;

    private float m_movementSpeed;
    private Vector2 m_savedDirection;

    private Vector2 m_playerScreenPosition;

    private float screenWidth;
    private float screenHeight;

    public PlayerController(PlayerInputActions.PlayerMovementActions movementActions, PoolsManager poolsManager, PlayerModel playerModel, MainUIModel mainUIModel, GameState currentState)
    {
        m_gameState = currentState;
        m_poolsManager = poolsManager;
        m_playerMovementActions = movementActions;
        m_playerModel = playerModel;
        m_mainUIModel = mainUIModel;
        screenHeight = Screen.height;
        screenWidth = Screen.width;
    }

    public void OnEnterExecute()
    {
        m_playerModel.PlayerView.objectTransform.position = Vector3.zero;
        m_playerMovementActions.Enable();
        m_movementSpeed = 0.0f;
        m_playerModel.PlayerView.onPlayerCollision += OnPlayerCollisionHandler;
    }

    private void OnPlayerCollisionHandler(string tag)
    {
        if(tag == GameobjectsNameKeys.Asteroid || tag == GameobjectsNameKeys.UFO)
        {
            m_playerMovementActions.Disable();
            m_gameState.changeStateRequest?.Invoke(GameStateIndex.EndGame);
        }
    }

    public void OnExitExecute()
    {
        m_playerMovementActions.Disable();
    }

    public void OnUpdateExecute()
    {
        var inputValues = m_playerMovementActions.KeyboardMovement.ReadValue<Vector2>();
        var mousePositionValue = m_playerMovementActions.MouseRotation.ReadValue<Vector2>();

        if (inputValues != Vector2.zero && m_movementSpeed <= m_playerModel.maxMovementSpeed)
        {
            m_movementSpeed += m_playerModel.accelerationRate;
            m_savedDirection = inputValues;
        }
        if(inputValues == Vector2.zero && m_movementSpeed >= m_playerModel.speedThreshold)
        {
            m_movementSpeed -= m_playerModel.inertiaRate;
        }
        if(inputValues == Vector2.zero && m_movementSpeed <= 0.0f)
        {
            m_savedDirection = Vector2.zero;
            m_movementSpeed = 0.0f;
        }
        if(inputValues != Vector2.zero && inputValues != m_savedDirection)
        {
            m_savedDirection = inputValues;
        }
        m_playerModel.PlayerView.transform.position += (Vector3)(m_savedDirection * m_movementSpeed * Time.deltaTime);
        m_playerModel.PlayerView.transform.rotation = 
            CommonUtils.GetRotationBetweenTwoScreenPoints(Camera.main.WorldToScreenPoint(m_playerModel.PlayerView.objectTransform.position), mousePositionValue);
        Portal(m_playerModel.PlayerView.objectTransform);
        m_mainUIModel.currentSpeed = m_movementSpeed;
        m_mainUIModel.currentAngle = CalculateUIAngle();
        m_mainUIModel.currentPosition = (Vector2)(m_playerModel.PlayerView.objectTransform.position);
    }

    private void Portal(Transform playerPosition)
    {
        m_playerScreenPosition = Camera.main.WorldToScreenPoint(playerPosition.position);
        if(m_playerScreenPosition.x < 0)
        {
            m_playerScreenPosition.x = screenWidth;
        }
        if (m_playerScreenPosition.x > screenWidth) 
        {
            m_playerScreenPosition.x = 0.0f;
        }
        if (m_playerScreenPosition.y < 0)
        {
            m_playerScreenPosition.y = screenHeight;
        }
        if (m_playerScreenPosition.y > screenHeight)
        {
            m_playerScreenPosition.y = 0.0f;
        }
        playerPosition.position = Camera.main.ScreenToWorldPoint(m_playerScreenPosition);
    }

    private float CalculateUIAngle()
    {
        return Mathf.Abs((Mathf.Atan2(m_playerModel.PlayerView.objectTransform.up.y, m_playerModel.PlayerView.objectTransform.up.x) * Mathf.Rad2Deg) - 180);
    }
}
