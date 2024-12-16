using System.Collections.Generic;
using UnityEngine;

public class ContextActionInputControl : MonoBehaviour
{
    [SerializeField] private EntityActionCollector targetActionCollector;

    private void Update()
    {
        if (Input.GetKey(KeyCode.E))
        {
            List<EntityContextAction> actionsList = targetActionCollector.GetAction<EntityContextAction>();

            for (int i = 0; i < actionsList.Count; i++)
            {
                actionsList[i].StartAction();
                actionsList[i].EndAction();
            }
        }
    }
}