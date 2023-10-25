using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateChangeModel : BaseModel
{
    public Action onStateChanged;

    public override void ClearModel()
    {
        onStateChanged = null;
    }
}
