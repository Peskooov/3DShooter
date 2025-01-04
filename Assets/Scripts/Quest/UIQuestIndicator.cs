using System;
using UnityEngine;
using UnityEngine.UI;

public class UIQuestIndicator : MonoBehaviour
{
    [SerializeField] private QuestCollector questCollector;

    [SerializeField] private Camera camera;
    [SerializeField] private Image indicator;

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

    private void Update()
    {
        if (reachedPoint == null) return;

        Vector3 pos = camera.WorldToScreenPoint(reachedPoint.position);

        if (pos.z > 0)
        {
            if (pos.x < 0) pos.x = 0 + indicator.rectTransform.localScale.x;
            if (pos.x > Screen.width) pos.x = Screen.width - indicator.rectTransform.localScale.x;

            if (pos.y < 0) pos.y = 0 + indicator.rectTransform.localScale.y;
            if (pos.y > Screen.height) pos.y = Screen.height - indicator.rectTransform.localScale.y;

            indicator.transform.position = pos;
        }
    }

    private void OnQuestResived(Quest quest)
    {
        indicator.enabled = true;
        reachedPoint = quest.ReachedPoint;
    }

    private void OnQuestComplited(Quest quest)
    {
        indicator.enabled = false;
    }
}