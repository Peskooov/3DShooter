using UnityEngine;
using UnityEngine.Serialization;

public abstract class ProjectileBase : Entity
{
    [SerializeField] private float velocity;
    [SerializeField] private float lifetime;
    [SerializeField] private int damage;
    
    [FormerlySerializedAs("turretMode")] [SerializeField] private WeaponMode weaponMode;
    [SerializeField] private GameObject[] targets;
    private GameObject closestTarget;
    
    protected Destructible parent;
    private float timer;
    
        private void Update()
    {
 
            float stepLenght = velocity * Time.deltaTime;

            Vector3 step = transform.forward * stepLenght;

            RaycastHit hit;

            if (Physics.Raycast(transform.position, transform.forward, out hit, stepLenght))
            {
                Destructible destructible = hit.collider.transform.root.GetComponent<Destructible>();

                if (destructible != null && destructible != parent)
                {
                    destructible.ApplyDamage(damage);
                }

                OnProjectilleLiveEnd(hit.collider, hit.point);
            }

            timer += Time.deltaTime;

            if (timer > lifetime)
                Destroy(gameObject);

            transform.position += new Vector3(step.x, step.y, step.z);
        
    }
        
    protected virtual void OnProjectilleLiveEnd(Collider collider, Vector3 position)
    {
        Destroy(gameObject);
    }
    
    public void SetParentShooter(Destructible parent)
    {
        this.parent = parent;
    }
}