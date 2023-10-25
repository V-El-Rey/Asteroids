using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class GameState
{
    public Action<GameStateIndex> changeStateRequest;

    public ControllersManager m_controllersManager;
    public GameStateIndex Index;
    public GameState()
    {
        m_controllersManager = new ControllersManager();
    }
    public virtual void OnEnterState()
    {
        m_controllersManager.OnEnterControllersExecute();
    }

    public virtual void OnUpdateState()
    {
        m_controllersManager.OnUpdateControllersExecute();
    }

    public virtual void OnExitState()
    {
        m_controllersManager.OnExitControllersExecute();
    }
}
