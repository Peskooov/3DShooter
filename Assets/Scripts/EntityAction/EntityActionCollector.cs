using System.Collections.Generic;
using UnityEngine;

public class EntityActionCollector : MonoBehaviour
{
    [SerializeField] private Transform parentTransformWithActions;

    private List<EntityAction> allActions = new List<EntityAction>();

    private void Awake()
    {
        for (int i = 0; i < parentTransformWithActions.childCount; i++)
        {
            if (parentTransformWithActions.GetChild(i).gameObject.activeSelf)
            {
                EntityAction entityAction = parentTransformWithActions.GetChild(i).GetComponent<EntityAction>();

                if (entityAction)
                {
                    allActions.Add(entityAction);
                }
            }
        }
    }

    public List<T> GetAction<T>() where T : EntityAction
    {
        List<T> actions = new List<T>();

        for (int i = 0; i < allActions.Count; i++)
        {
            if (allActions[i] is T)
            {
                actions.Add((T)allActions[i]);
            }
        }

        return actions;
    }
}