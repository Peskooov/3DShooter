using UnityEngine;

public class Drone : Destructible
{
    [Header("Main")] 
    [SerializeField] private Weapon[] weapons;
    [SerializeField] private Transform mainMesh;
    public Transform MainMesh => mainMesh;
    
    [Header("View")]
    [SerializeField] private GameObject[] meshComponents;
    [SerializeField] private Renderer[] meshRenderers;
    [SerializeField] private Material[] deadMaterials;

    [Header("Movement")] 
    [SerializeField] private float hoverAmplitude;
    [SerializeField] private float hoverSpeed;
    [SerializeField] private float movementSpeed;
    [SerializeField] private float rotationLerpFactor;
    
    [SerializeField] private float timePatrolDelay;
    [SerializeField] private Vector3 maxStepDistance;

    private bool isDisabled;
    public bool IsDisabled => isDisabled;
    
    private void Update()
    {
        Hover();
    }

    public void Disable()
    {
        isDisabled = true;
        
        for (int i = 0; i < meshComponents.Length; i++)
        {
            meshComponents[i].transform.SetParent(meshComponents[0].transform);
            
            if (!meshComponents[0].GetComponent<Rigidbody>())
                meshComponents[0].AddComponent<Rigidbody>();
        }
    }
    
    public void LookAt(Vector3 target)
    {
        transform.rotation = Quaternion.RotateTowards(transform.rotation,
            Quaternion.LookRotation(target - transform.position, Vector3.up), Time.deltaTime * rotationLerpFactor);
    }
    
    public void MoveTo(Vector3 target)
    {
        transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * movementSpeed);
    }
    
    public void Fire(Vector3 target)
    {
        for (int i = 0; i < weapons.Length; i++)
        {
            weapons[i].FirePointLookAt(target);
            weapons[i].Fire();
        }
    }
    
    protected override void OnDeath()
    {
        EventOnDeath?.Invoke();

        enabled = false;

        for (int i = 0; i < meshComponents.Length; i++)
        {
            if (!meshComponents[i].GetComponent<Rigidbody>())
                meshComponents[i].AddComponent<Rigidbody>();
        }

        for (int i = 0; i < meshRenderers.Length; i++)
        {
            meshRenderers[i].material = deadMaterials[i];
        }
    }
    
    private void Hover()
    {
        if(isDisabled) return;
        mainMesh.position += new Vector3(0, Mathf.Sin(Time.time * hoverAmplitude) * hoverSpeed * Time.deltaTime, 0);
    }
}