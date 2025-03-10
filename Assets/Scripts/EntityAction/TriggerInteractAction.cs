using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider))]
public class TriggerInteractAction : MonoBehaviour
{
    [SerializeField] private InteractType interactType;
    [SerializeField] private int interactAmount;
    [SerializeField] private UnityEvent eventStartInteract;
    [SerializeField] private UnityEvent eventEndInteract;
    [SerializeField] protected ActionInteractProperties actionProperties;

    //public UnityEvent EventOnInteract => eventStartInteract;

    protected GameObject owner;
    protected ActionInteract action;

    protected virtual void InitActionProperties()
    {
        action.SetProperties(actionProperties);
    }

    protected virtual void OnStartAction(GameObject owner)
    {
    }

    protected virtual void OnEndAction(GameObject owner)
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if (interactAmount == 0) return;

        EntityActionCollector actionCollector = other.GetComponent<EntityActionCollector>();

        if (actionCollector != null)
        {
            action = GetActionInteract(actionCollector);

            if (action != null)
            {
                InitActionProperties();

                action.IsCanStart = true;
                action.EventOnStart.AddListener(ActionStarted);
                action.EventOnEnd.AddListener(ActionEnded);

                owner = other.gameObject;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (interactAmount == 0) return;

        EntityActionCollector actionCollector = other.GetComponent<EntityActionCollector>();

        if (actionCollector != null)
        {
            action = GetActionInteract(actionCollector);

            if (action != null)
            {
                action.IsCanStart = false;
                action.EventOnStart.RemoveListener(ActionStarted);
                action.EventOnEnd.RemoveListener(ActionEnded);
            }
        }
    }

    private void ActionStarted()
    {
        OnStartAction(owner);

        eventStartInteract?.Invoke();

        interactAmount--;
    }

    private void ActionEnded()
    {
        action.IsCanStart = false;
        action.IsCanEnd = false;
        action.EventOnStart.RemoveListener(ActionStarted);
        action.EventOnEnd.RemoveListener(ActionEnded);

        eventEndInteract?.Invoke();

        OnEndAction(owner);
    }

    private ActionInteract GetActionInteract(EntityActionCollector actionCollector)
    {
        List<ActionInteract> actions = actionCollector.GetAction<ActionInteract>();

        for (int i = 0; i < actions.Count; i++)
        {
            if (actions[i].Type == interactType)
            {
                return actions[i];
            }
        }

        return null;
    }
}