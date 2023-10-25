using UnityEngine.InputSystem;

public class GunController : IBaseController, IEnterController, IExitController
{
    private PlayerInputActions.PlayerMovementActions m_playerMovementActions;
    private ShootingModel m_shootingModel;
    public GunController(PlayerInputActions.PlayerMovementActions movementActions, ShootingModel shootingModel) 
    { 
        m_playerMovementActions = movementActions;
        m_shootingModel = shootingModel;
    }
    public void OnEnterExecute()
    {
        m_playerMovementActions.Shoot1.performed += ShootBullet;
        m_playerMovementActions.Shoot2.performed += ShootLaser;
    }

    public void OnExitExecute()
    {
        m_playerMovementActions.Shoot1.performed -= ShootBullet;
        m_playerMovementActions.Shoot2.performed -= ShootLaser;
    }
    private void ShootLaser(InputAction.CallbackContext context)
    {
        m_shootingModel.onLaserShot?.Invoke();
    }

    private void ShootBullet(InputAction.CallbackContext context)
    {
        m_shootingModel.onBulletShot?.Invoke();
    }
}
