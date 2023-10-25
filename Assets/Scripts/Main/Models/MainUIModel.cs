using UnityEngine;

namespace Assets.Scripts.Main.Models
{
    public class MainUIModel : BaseModel
    {
        public float currentSpeed;
        public float currentAngle;
        public Vector2 currentPosition;
        public float laserCooldown;
        public int laserShotsLeft;
        public int score;

        public override void ClearModel()
        {
            currentSpeed = 0;
            currentAngle = 0;
            currentPosition = Vector2.zero;
            laserCooldown = 0;
            score = 0;
            laserShotsLeft = 0;
        }
    }
}
