using UnityEngine;

public class MainMenuController : IBaseController, IEnterController, IExitController
{
    private GameObject m_uiPrefab;
    private MainMenuView m_mainMenuView;
    private MainMenuUIModel m_mainMenuUIModel;
    private Transform m_uiRoot;

    public MainMenuController(Transform uiRoot, MainMenuUIModel model) 
    {
        m_mainMenuUIModel = model;
        m_uiRoot = uiRoot;
        m_uiPrefab = Resources.Load<GameObject>(GameobjectsNameKeys.MainMenu);
    }
    public void OnEnterExecute()
    {
        var obj = GameObject.Instantiate(m_uiPrefab, m_uiRoot);
        m_mainMenuView = obj.GetComponent<MainMenuView>();
        m_mainMenuView.StartGame.onClick.AddListener(() => m_mainMenuUIModel.onStartGameClicked?.Invoke());
        m_mainMenuView.Exit.onClick.AddListener(() => m_mainMenuUIModel.onExitGameClicked?.Invoke());
    }

    public void OnExitExecute()
    {
        m_mainMenuView.StartGame.onClick.RemoveAllListeners();
        m_mainMenuView.Exit.onClick.RemoveAllListeners();
        GameObject.Destroy(m_mainMenuView.gameObject);
    }
}
