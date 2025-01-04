using UnityEngine;
using UnityEngine.Events;

public class Quest : MonoBehaviour
{
    public UnityAction Completed;

    [SerializeField] private Quest nextQuest;
    [SerializeField] private QuestProperties properties;
    [SerializeField] private Transform reachedPoint;

    public Quest NextQuest => nextQuest;
    public QuestProperties Properties => properties;
    public Transform ReachedPoint => reachedPoint;

    private void Update()
    {
        UpdateCompleteCondition();
    }

    protected virtual void UpdateCompleteCondition()
    {
    }
}