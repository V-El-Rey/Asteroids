using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ControllersManager
{
    private List<IBaseController> m_controllers;
    public ControllersManager()
    {
        m_controllers = new List<IBaseController>();
    }

    public void OnEnterControllersExecute()
    {
        foreach (var controller in m_controllers)
        {
            if(controller is IEnterController controllerInstance)
            {
                controllerInstance.OnEnterExecute();
            }
        }
    }

    public void OnExitControllersExecute()
    {
        foreach (var controller in m_controllers)
        {
            if (controller is IExitController controllerInstance)
            {
                controllerInstance.OnExitExecute();
            }
        }

    }

    public void OnUpdateControllersExecute()
    {
        foreach (var controller in m_controllers)
        {
            if (controller is IUpdateController controllerInstance)
            {
                controllerInstance.OnUpdateExecute();
            }
        }
    }

    public void AddController(IBaseController controller)
    {
        m_controllers.Add(controller);
    }
}
