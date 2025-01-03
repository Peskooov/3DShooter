using UnityEngine;

public class UIPlayerNotice : MonoBehaviour
{
    [SerializeField] private GameObject hit;

    public void Show()
    {
        hit.SetActive(true);
    }

    public void Hide()
    {
        hit.SetActive(false);
    }
}