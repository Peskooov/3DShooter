using UnityEngine;

public class CameraShooter : MonoBehaviour
{
    [SerializeField] private Weapon weapon;

    [SerializeField] private Camera targetCamera;
    public Camera Camera => targetCamera;

    [SerializeField] private RectTransform imageSigh;

    public void Shoot()
    {
        RaycastHit hit;
        Ray ray = targetCamera.ScreenPointToRay(imageSigh.position);

        if (Physics.Raycast(ray, out hit, 1000))
        {
            weapon.FirePointLookAt(hit.point);
        }
        else
        {
            weapon.FirePointLookAt(targetCamera.transform.position + ray.direction * 1000);
        }

        if (weapon.CanFire == true)
        {
            weapon.Fire();
        }
    }
}