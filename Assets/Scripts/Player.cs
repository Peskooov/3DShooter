using UnityEngine;

public class Player : SingletonBase<Player>
{
    [SerializeField] private UIPlayerNotice uiPlayerNotice;

    private int pursuersNumbers;

    public void StartPursuet()
    {
        pursuersNumbers++;

        uiPlayerNotice.Show();
    }

    public void EndPursuet()
    {
        pursuersNumbers--;

        if (pursuersNumbers == 0)
        {
            uiPlayerNotice.Hide();
        }
    }
}