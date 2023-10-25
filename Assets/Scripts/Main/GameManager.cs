using Assets.Scripts.Main.Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public PoolConfiguration PoolConfiguration;
    public Transform mainUIRoot;

    private PlayerInputActions m_playerInputActions;
    private PoolsManager m_poolsManager;
    private StateMachineController m_stateMachineController;

    private StateChangeModel m_stateChangeModel;
    private MainMenuUIModel m_mainMenuUIModel;
    private MainUIModel m_mainUIModel;

    private void Awake()
    {
        m_playerInputActions = new PlayerInputActions();
        m_playerInputActions.Enable();
        m_stateChangeModel = new StateChangeModel();
        m_mainMenuUIModel = new MainMenuUIModel();
        m_mainUIModel = new MainUIModel();
        m_stateMachineController = new StateMachineController(GameStateIndex.MainMenu, m_stateChangeModel);
    }

    // Start is called before the first frame update
    void Start()
    {
        m_poolsManager = new PoolsManager(PoolConfiguration);
        m_poolsManager.InitializePool();
        m_stateMachineController.AddGameState(new MainMenuState(mainUIRoot, m_mainMenuUIModel));
        m_stateMachineController.AddGameState(new MainGameState(mainUIRoot, m_playerInputActions, m_poolsManager, m_mainUIModel));
        m_stateMachineController.AddGameState(new EndGameState(mainUIRoot, m_mainUIModel));
        m_stateMachineController.InitializeFirstState();
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.R))
        //{
        //    m_stateMachineController.ChangeState(GameStateIndex.Game);
        //}
        //if (Input.GetKeyDown(KeyCode.S))
        //{
        //    m_stateMachineController.ChangeState(GameStateIndex.MainMenu);
        //}
        m_stateMachineController.Update();
    }
}
