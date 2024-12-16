using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Canvas))]
public class Hint : MonoBehaviour
{
    public enum HintType
    {
        Null,
        Player
    }

    [SerializeField] private HintType hintType;
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
        target = GameObject.FindGameObjectWithTag("Player").transform;

        hint.SetActive(false);

        if (hintType == HintType.Player)
        {
            aiAlienSoldiers = new List<AIAlienSoldier>();

            // Поиск всех объектов AIAlienSoldier на сцене и добавление их в список
            AIAlienSoldier[] soldiers = FindObjectsOfType<AIAlienSoldier>();
            aiAlienSoldiers.AddRange(soldiers);
            // Подписка на события видимости
            foreach (var soldier in aiAlienSoldiers)
            {
                soldier.OnVisibilityChanged += UpdateUI;
            }
        }
    }

    private void OnDestroy()
    {
        if (hintType == HintType.Player)
        {
            foreach (var soldier in aiAlienSoldiers)
            {
                soldier.OnVisibilityChanged -= UpdateUI;
            }
        }
    }

    private void Update()
    {
        hint.transform.LookAt(lookTransform);

        if (hintType == HintType.Null)
        {
            if (Vector3.Distance(transform.position, target.position) < activateRadius)
                hint.SetActive(true);
            else
                hint.SetActive(false);
        }
    }

    private void UpdateUI(bool isVisible)
    {
        if (isVisible)
        {
            hint.SetActive(true);
        }
        else
        {
            hint.SetActive(false);
        }
    }

    private void InteractEnded()
    {
        Destroy(gameObject);
    }
}