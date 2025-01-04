using UnityEngine;
using UnityEngine.Events;

public class QuestCollector : MonoBehaviour
{
    public UnityAction<Quest> QuestResived;
    public UnityAction<Quest> QuestCompleted;
    public UnityAction LastQuestCompleted;

    [SerializeField] private Quest currentQuest;
    public Quest CurrentQuest => currentQuest;

    private void Start()
    {
        if (currentQuest != null)
            AssignQuest(currentQuest);
    }

    public void AssignQuest(Quest quest)
    {
        currentQuest = quest;

        QuestResived?.Invoke(currentQuest);

        currentQuest.Completed += OnQuestCompleted;
    }

    private void OnQuestCompleted()
    {
        currentQuest.Completed -= OnQuestCompleted;

        QuestCompleted?.Invoke(currentQuest);

        if (currentQuest.NextQuest != null)
            AssignQuest(currentQuest.NextQuest);
        else
            LastQuestCompleted?.Invoke();
    }
}