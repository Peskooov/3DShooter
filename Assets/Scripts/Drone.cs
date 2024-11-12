using UnityEngine;
using Random = UnityEngine.Random;

public class Drone : Destructible
{
    [Header("Main")]
    [SerializeField] private Transform mainMesh;
    
    [Header("View")]
    [SerializeField] private GameObject[] meshComponents;
    [SerializeField] private Renderer[] meshRenderers;
    [SerializeField] private Material[] deadMaterials;

    [Header("Movement")] 
    [SerializeField] private float hoverAmplitude;
    [SerializeField] private float hoverSpeed;
    [SerializeField] private float timePatrolDelay;
    [SerializeField] private Vector3 maxStepDistance;

    private Vector3 targetPosition;
    private float timer;
    private bool isWaiting;

    private void Update()
    {
        Move();
        SetWaiting();
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

    private void Move()
    {
        mainMesh.position += new Vector3(0, Mathf.Sin(Time.time * hoverAmplitude) * hoverSpeed * Time.deltaTime, 0);
        
        if (Vector3.Distance(transform.position, targetPosition) <= 0.1f)
        {
            SetWaiting();
            return; 
        }
        
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, hoverSpeed * Time.deltaTime);
        
        Vector3 direction = (targetPosition - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * hoverSpeed);
    }

    private void SetNextPosition()
    {
        targetPosition = new Vector3(transform.position.x + Random.Range(-maxStepDistance.x, maxStepDistance.x), transform.position.y, 
                                     transform.position.z + Random.Range(-maxStepDistance.z, maxStepDistance.z)); 
  
        timer = timePatrolDelay;
    }

    private void SetWaiting()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            timer = 0;
            SetNextPosition();
        }
    }
}