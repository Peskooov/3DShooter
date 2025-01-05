using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Canvas))]
public class Hint : MonoBehaviour
{
    [SerializeField] private GameObject hint;
    [SerializeField] private float activateRadius;
    [SerializeField] private TriggerInteractAction interactAction;
    private List<AIAlienSoldier> aiAlienSoldiers;

    private Canvas canvas;
    private Transform target;
    private Transform lookTransform;

    private void Start()
    {
        canvas = GetComponent<Canvas>();
        canvas.worldCamera = Camera.main;
        lookTransform = Camera.main.transform;
        target = Player.Instance.transform;

        hint.SetActive(false);
    }

    private void Update()
    {
        hint.transform.LookAt(lookTransform);

        if (Vector3.Distance(transform.position, Player.Instance.transform.position) < activateRadius)
            hint.SetActive(true);
        else
            hint.SetActive(false);
    }
}