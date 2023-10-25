using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerView : BaseView
{ 
    public Transform muzzle;
    public Action<string> onPlayerCollision;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        onPlayerCollision?.Invoke(collision.tag);
    }
}
