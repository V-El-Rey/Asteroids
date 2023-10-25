using UnityEngine;

public class MainMenuState : GameState
{
    private MainMenuUIModel m_mainMenuUIModel;
    public MainMenuState(Transform uiRoot, MainMenuUIModel mainMenuModel) : base()
    {
        Index = GameStateIndex.MainMenu;
        m_mainMenuUIModel = mainMenuModel;
        m_controllersManager.AddController(new MainMenuController(uiRoot, m_mainMenuUIModel));
    }

    public override void OnEnterState()
    {
        m_mainMenuUIModel.onExitGameClicked += ExitGame;
        m_mainMenuUIModel.onStartGameClicked += StartGame;
        base.OnEnterState();
    }

    public override void OnExitState()
    {
        m_mainMenuUIModel.onStartGameClicked -= StartGame;
        m_mainMenuUIModel.onExitGameClicked -= ExitGame;
        base.OnExitState();
    }

    private void ExitGame()
    {
        Application.Quit();
    }

    private void StartGame()
    {
        changeStateRequest?.Invoke(GameStateIndex.Game);
    }
}
