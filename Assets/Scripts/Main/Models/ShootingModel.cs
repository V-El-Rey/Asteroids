using System;

public class ShootingModel
{
    public Action onBulletShot;
    public Action onLaserShot;

    public int laserShotsLeft;
    public int maximumLaserShots = 5;

    public float laserBeamTime = 3.0f;
    public float laserCooldown = 30.0f;
}
