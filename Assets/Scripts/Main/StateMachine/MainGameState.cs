using Assets.Scripts.Main.Controllers;
using Assets.Scripts.Main.Models;
using UnityEngine;

public class MainGameState : GameState
{
    private PlayerInputActions m_playerInputActions;
    private PlayerModel m_playerModel;
    private ShootingModel m_shootingModel;
    private MainUIModel m_mainUIModel;
    private EnemiesModel m_enemiesModel;
    public MainGameState(Transform uiRoot, PlayerInputActions playerInputActions, PoolsManager poolsManager, MainUIModel mainUIModel) : base()
    {
        Index = GameStateIndex.Game;
        m_playerInputActions = playerInputActions;
        m_shootingModel = new ShootingModel();
        m_playerModel = new PlayerModel();
        m_enemiesModel = new EnemiesModel();

        m_mainUIModel = mainUIModel;
        m_playerModel.PlayerView = poolsManager.GetObjectFromPool(GameobjectsNameKeys.Player) as PlayerView;

        m_controllersManager.AddController(new MainUIController(uiRoot, m_mainUIModel));
        m_controllersManager.AddController(new PlayerController(m_playerInputActions.PlayerMovement, poolsManager, m_playerModel, m_mainUIModel, this));
        m_controllersManager.AddController(new GunController(m_playerInputActions.PlayerMovement, m_shootingModel));
        m_controllersManager.AddController(new ShootingController(m_playerModel.PlayerView, poolsManager, m_shootingModel, m_mainUIModel));
        m_controllersManager.AddController(new EnemiesController(poolsManager, m_playerModel, m_enemiesModel));
    }
}
