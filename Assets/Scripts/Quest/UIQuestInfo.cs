using System;
using UnityEngine;
using TMPro;

public class UIQuestInfo : MonoBehaviour
{
    [SerializeField] private QuestCollector questCollector;

    [SerializeField] private TMP_Text textDescription;
    [SerializeField] private TMP_Text textTask;

    private Transform reachedPoint;

    private void Start()
    {
        questCollector.QuestResived += OnQuestResived;
        questCollector.QuestCompleted += OnQuestComplited;
    }

    private void OnDestroy()
    {
        questCollector.QuestResived -= OnQuestResived;
        questCollector.QuestCompleted -= OnQuestComplited;
    }

    private void OnQuestResived(Quest quest)
    {
        textDescription.enabled = true;
        textTask.enabled = true;

        textDescription.text = quest.Properties.Description;
        textTask.text = quest.Properties.Task;
    }

    private void OnQuestComplited(Quest quest)
    {
        textDescription.enabled = false;
        textTask.enabled = false;
    }
}