using System;
using UnityEngine;

public class QuestKillDestructible : Quest
{
    [SerializeField] private Destructible[] destructibles;

    private int countDestructableDead;

    private void Start()
    {
        if (destructibles == null) return;

        for (int i = 0; i < destructibles.Length; i++)
        {
            destructibles[i].EventOnDeath.AddListener(OnDestructibleDeath);
        }
    }

    private void OnDestroy()
    {
    }

    private void OnDestructibleDeath()
    {
        countDestructableDead++;

        if (countDestructableDead == destructibles.Length)
        {
            for (int i = 0; i < destructibles.Length; i++)
            {
                if (destructibles[i] != null)
                    destructibles[i].EventOnDeath.RemoveListener(OnDestructibleDeath);
            }

            Completed?.Invoke();
            Debug.Log("Kill");
        }
    }
}