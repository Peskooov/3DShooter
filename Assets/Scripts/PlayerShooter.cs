using UnityEngine;

public class PlayerShooter : MonoBehaviour
{
    [SerializeField] private CharacterMovement characterMovement;
    [SerializeField] private Camera characterCamera;
    [SerializeField] private Weapon weapon;
    [SerializeField] private SpreadShootRig spreadShootRig;
    
    [SerializeField] private RectTransform imageSigh ;

    public void Shoot()
    {
        RaycastHit hit;
        Ray ray = characterCamera.ScreenPointToRay(imageSigh.position);

        if (Physics.Raycast(ray, out hit, 1000))
        {
            weapon.FirePointLookAt(hit.point);
        }

        if (weapon.CanFire)
        {
            weapon.Fire();
            spreadShootRig.Spread();
        }
    }
}
