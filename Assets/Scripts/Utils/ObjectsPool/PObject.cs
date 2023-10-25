using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PObject
{
    public string objectName;
    public int initialCount;
    public int maxCount;
    public BaseView view;
}
