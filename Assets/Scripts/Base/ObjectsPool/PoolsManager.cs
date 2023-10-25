using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PoolsManager
{
    private PoolConfiguration m_poolConfiguration;

    private Dictionary<string, ObjectPool<BaseView>> m_objects;
    private Transform m_poolRoot;

    public PoolsManager(PoolConfiguration poolConfiguration)
    {
        m_poolConfiguration = poolConfiguration;
        m_objects = new Dictionary<string, ObjectPool<BaseView>>();
    }

    public void InitializePool()
    {
        m_poolRoot = new GameObject(GameobjectsNameKeys.PoolRoot).transform;
        foreach (var pObj in m_poolConfiguration.poolObjects)
        {
            if (!m_objects.ContainsKey(pObj.objectName))
            {
                m_objects.Add(pObj.objectName, new ObjectPool<BaseView>(() => OnCreateObject(pObj.objectName), OnGetObject, OnReleaseObject, OnDestroyObject, true, pObj.initialCount, pObj.maxCount));
            }
        }
    }

    private void OnDestroyObject(BaseView view)
    {
        GameObject.Destroy(view.gameObject);
    }

    private void OnReleaseObject(BaseView view)
    {
        view.gameObject.SetActive(false);
    }

    private BaseView OnCreateObject(string name)
    {
        var result = GameObject.Instantiate(m_poolConfiguration[name].view, m_poolRoot);
        result.gameObject.SetActive(false);
        return result;
    }

    private void OnGetObject(BaseView view)
    {
        view.gameObject.SetActive(true);
    }

    public BaseView GetObjectFromPool(string name)
    {
        return m_objects[name].Get();
    }

    public void ReturnObjectToPool(BaseView obj)
    {
        m_objects[m_poolConfiguration.poolObjects.Find(x => obj.name.Contains(x.objectName)).objectName].Release(obj);
    }
}
