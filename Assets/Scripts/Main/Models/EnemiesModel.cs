
namespace Assets.Scripts.Main.Models
{
    public class EnemiesModel : BaseModel
    {
        public float asteroidSpeed;
        public float ufoSpeed;
        public float asteroidsCooldown;
        public float ufosCooldown;

        public EnemiesModel() 
        {
            asteroidSpeed = 0.4f;
            ufoSpeed = 0.05f;
            asteroidsCooldown = 4.0f;
            ufosCooldown = 10.0f;
        }

        public override void ClearModel()
        {
            asteroidsCooldown = 0.0f;
            ufosCooldown = 0.0f;
        }
    }
}
