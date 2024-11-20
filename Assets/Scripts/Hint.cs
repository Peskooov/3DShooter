using UnityEngine;

[RequireComponent(typeof(Canvas))]
public class Hint : MonoBehaviour
{
    [SerializeField] private GameObject hint;
    [SerializeField] private float activateRadius;

    private Canvas canvas;
    private Transform target;
    private Transform lookTransform;

    private void Start()
    {
        canvas = GetComponent<Canvas>();
        canvas.worldCamera = Camera.main;
        lookTransform = Camera.main.transform;
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        hint.transform.LookAt(lookTransform);
        
        if(Vector3.Distance(transform.position, target.position) < activateRadius)
            hint.SetActive(true);
        else
            hint.SetActive(false);
    }
}
