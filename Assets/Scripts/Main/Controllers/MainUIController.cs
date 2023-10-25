using Assets.Scripts.Main.Models;
using UnityEngine;

namespace Assets.Scripts.Main.Controllers
{
    public class MainUIController : IBaseController, IEnterController, IExitController, IUpdateController
    {
        private GameObject m_uiPrefab;
        private GameUIView m_gameUIView;
        private MainUIModel m_mainUiModel;
        private Transform m_uiRoot;

        public MainUIController(Transform uiRoot, MainUIModel model)
        {
            m_mainUiModel = model;
            m_uiRoot = uiRoot;
            m_uiPrefab = Resources.Load<GameObject>(GameobjectsNameKeys.MainUI);
        }
        public void OnEnterExecute()
        {
            m_mainUiModel.ClearModel();
            var obj = GameObject.Instantiate(m_uiPrefab, m_uiRoot);
            m_gameUIView = obj.GetComponent<GameUIView>();
        }

        public void OnExitExecute()
        {
            GameObject.Destroy(m_gameUIView.gameObject);
        }

        public void OnUpdateExecute()
        {
            m_gameUIView.Speed.text = m_mainUiModel.currentSpeed.ToString("0.0");
            m_gameUIView.Coordinates.text = m_mainUiModel.currentPosition.ToString("0.0");
            m_gameUIView.Angle.text = m_mainUiModel.currentAngle.ToString("0.0");
            m_gameUIView.LasersLeft.text = m_mainUiModel.laserShotsLeft.ToString();
            m_gameUIView.LasersCooldown.text = m_mainUiModel.laserCooldown.ToString("0.0");
            m_gameUIView.Score.text = m_mainUiModel.score.ToString();

        }
    }
}
