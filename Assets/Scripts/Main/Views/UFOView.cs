using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Main.Views
{
    public class UFOView : BaseView
    {
        public Action<UFOView, string> onCollision;
        private void OnTriggerEnter2D(Collider2D collision)
        {
            onCollision?.Invoke(this, collision.tag);
        }
    }
}
