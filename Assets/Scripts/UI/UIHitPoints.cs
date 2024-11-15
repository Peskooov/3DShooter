using UnityEngine;
using UnityEngine.UI;

public class UIHitPoints : MonoBehaviour
{
    [SerializeField] private Destructible destructible;
    [SerializeField] private Slider slider;
    
    private void Update()
    {
        slider.value = destructible.HitPoints;
    }
}
