using UnityEngine;

public class Projectile : Entity
{ 
    [SerializeField] private WeaponMode weaponMode;
    
    [SerializeField] private float velocity;
    [SerializeField] private float lifetime;
    [SerializeField] private int damage;

    [SerializeField] private ImpactEffect[] impactEffects;
    
    private Destructible parent;
    private float timer;
    
        private void Update()
    {
            float stepLenght = velocity * Time.deltaTime;
            Vector3 step = transform.forward * stepLenght;

            RaycastHit hit;

            if (Physics.Raycast(transform.position, transform.forward, out hit, stepLenght))
            {
                Destructible destructible = hit.collider.transform.root.GetComponent<Destructible>();

                if (destructible && destructible != parent)
                {
                    destructible.ApplyDamage(damage);
                }

                OnProjectileLiveEnd(hit.collider, hit.point, hit.normal);
            }

            timer += Time.deltaTime;

            if (timer > lifetime)
                Destroy(gameObject);

            transform.position += new Vector3(step.x, step.y, step.z);
        
    }
        
    protected virtual void OnProjectileLiveEnd(Collider col, Vector3 pos, Vector3 normal)
    {
        ImpactEffect selectedEffect = null;

        if (col.CompareTag("Stone"))
            selectedEffect = impactEffects[0];
        if (col.CompareTag("Metal"))
            selectedEffect = impactEffects[1];
        if (col.CompareTag("Wood"))
            selectedEffect = impactEffects[2];
        if (col.CompareTag("Glass"))
            selectedEffect = impactEffects[3];
        if (col.CompareTag("Untagged"))
            selectedEffect = null;
        
        if (selectedEffect)
        {
            ImpactEffect impact = Instantiate(selectedEffect, pos, Quaternion.LookRotation(normal));
            
            impact.transform.SetParent(col.transform);
        }
        
        Destroy(gameObject);
    }
    
    public void SetParentShooter(Destructible parent)
    {
        this.parent = parent;
    }
}