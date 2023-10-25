using Assets.Scripts.Main.Controllers;
using Assets.Scripts.Main.Models;
using UnityEngine;

public class EndGameState : GameState
{
    private MainUIModel m_mainUIModel;
    public EndGameState(Transform mainUIRoot, MainUIModel mainUIModel) : base()
    {
        Index = GameStateIndex.EndGame;
        m_mainUIModel = mainUIModel;

        m_controllersManager.AddController(new EndGameUIController(mainUIRoot, mainUIModel, this));
    }
}
