using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public PoolConfiguration PoolConfiguration;
    public PlayerInputActions PlayerInputActions;
    private PoolsManager m_objectPool;

    private Stack<BaseView> objs;

    // Start is called before the first frame update
    void Start()
    {
        m_objectPool = new PoolsManager(PoolConfiguration);
        objs = new Stack<BaseView>();
        m_objectPool.InitializePool();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            var poolObject = m_objectPool.GetObjectFromPool(GameobjectsNameKeys.AsteroidMedium);
            poolObject.transform.position = Vector3.one * 2;
            objs.Push(poolObject);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            if (objs.Count > 0)
            {
                //m_objectPool.ReturnObjectToPool(objs.Pop(), GameobjectsNameKeys.AsteroidMedium);
            }
        }
    }
}
