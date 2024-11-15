using UnityEngine;

[RequireComponent(typeof(Drone))]
public class DroneAI : MonoBehaviour
{
    [SerializeField] private CubeArea movementArea;
    [SerializeField] private float fireDistance;
    
    private Drone drone;
    private Vector3 movementPosition;
    private Transform shootTarget;
    private Transform player;
    
    private void Start()
    {
        drone = GetComponent<Drone>();
        drone.EventOnDeath.AddListener(OnDroneDeath);
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void OnDestroy()
    {
        drone.EventOnDeath.RemoveListener(OnDroneDeath);
    }

    private void Update()
    {
        UpdateAI();
    }

    private void OnDroneDeath()
    {
        enabled = false;
    }
    
    private void UpdateAI()
    {
        if (transform.position == movementPosition)
            movementPosition = movementArea.GetRandomInsideZone();

        if (Physics.Linecast(transform.position, movementPosition))
            movementPosition = movementArea.GetRandomInsideZone();

        if (Vector3.Distance(transform.position, player.position) <= fireDistance)
            shootTarget = player;
        
        drone.MoveTo(movementPosition);

        if (shootTarget)
            drone.LookAt(shootTarget.position);
        else
             drone.LookAt(movementPosition);

        if (shootTarget)
        {
            drone.Fire(shootTarget.position);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, fireDistance);
    }
}