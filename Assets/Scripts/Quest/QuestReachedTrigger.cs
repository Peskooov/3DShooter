using UnityEngine;

public class QuestReachedTrigger : Quest
{
    [SerializeField] private GameObject owner;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject != owner) return;

        Completed?.Invoke();

        Debug.Log("Completed");
    }
}