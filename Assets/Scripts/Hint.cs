using UnityEngine;

[RequireComponent(typeof(Canvas))]
public class Hint : MonoBehaviour
{
    [SerializeField] private GameObject hint;
    [SerializeField] private float activateRadius;
    [SerializeField] private TriggerInteractAction interactAction;

    private Canvas canvas;
    private Transform target;
    private Transform lookTransform;

    private void Start()
    {
        canvas = GetComponent<Canvas>();
        canvas.worldCamera = Camera.main;
        lookTransform = Camera.main.transform;
        target = GameObject.FindGameObjectWithTag("Player").transform;
        
        if(!interactAction) return;
        interactAction.EventOnInteract.AddListener(InteractEnded);
    }

    private void OnDestroy()
    {
        if(!interactAction) return;
        interactAction.EventOnInteract.RemoveListener(InteractEnded);
    }

    private void Update()
    {
        hint.transform.LookAt(lookTransform);

        if (Vector3.Distance(transform.position, target.position) < activateRadius)
            hint.SetActive(true);
        else
            hint.SetActive(false);
    }

    private void InteractEnded()
    {
        Destroy(gameObject);
    }
}