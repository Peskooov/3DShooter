using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    [SerializeField] private string nickname;
    public string Nickname => nickname;
}