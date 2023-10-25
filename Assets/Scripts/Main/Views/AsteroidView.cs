using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidView : BaseView
{
    public AsteroidSize size;
    public bool instantiated;
    public Action<AsteroidView, string> onCollision;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        onCollision?.Invoke(this, collision.tag);
    }
}
