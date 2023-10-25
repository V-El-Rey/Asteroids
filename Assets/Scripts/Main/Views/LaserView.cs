using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Main.Views
{
    public class LaserView : BaseView
    {
        public GameObject beam;
        public Action onCollision;
        private void OnTriggerEnter2D(Collider2D collision)
        {
            onCollision?.Invoke();
        }
    }
}
