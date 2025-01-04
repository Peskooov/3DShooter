using UnityEngine;

[CreateAssetMenu]
public class QuestProperties : ScriptableObject
{
    [TextArea] [SerializeField] private string description;
    public string Description => description;

    [TextArea] [SerializeField] private string task;
    public string Task => task;
}