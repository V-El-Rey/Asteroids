using System;
using UnityEngine;

public class BulletView : BaseView
{
    public Action<BulletView, string> onCollision;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        onCollision?.Invoke(this, collision.tag);
    }
}
