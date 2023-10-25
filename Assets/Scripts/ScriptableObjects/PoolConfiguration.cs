using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "PoolConfiguration", menuName = "Config/Pool", order = 1)]
public class PoolConfiguration : ScriptableObject
{
    public List<PObject> poolObjects;
    public PObject this[string index]
    {
        get => poolObjects.Find(x => x.objectName == index);
        set => poolObjects.Find(x => x.objectName == index).view = value.view;
    }
}

