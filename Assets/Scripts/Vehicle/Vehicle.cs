using UnityEngine;

public class Vehicle : Destructible
{
    [SerializeField] protected float maxLinearVelocity;

    [Header("Engine Sound")] [SerializeField]
    private AudioSource engineSFX;

    [SerializeField] private float engineSFXModifier;

    public virtual float LinearVelocity => 0;

    public float NormalizedLinearVelocity
    {
        get
        {
            if (Mathf.Approximately(0, LinearVelocity) == true) return 0;

            return Mathf.Clamp01(LinearVelocity / maxLinearVelocity);
        }
    }

    protected Vector3 TargetInputControl;

    public void SetTargetControl(Vector3 control)
    {
        TargetInputControl = control.normalized;
    }

    protected virtual void Update()
    {
        UpdateEngineSFX();
    }

    private void UpdateEngineSFX()
    {
        if (engineSFX != null)
        {
            if (NormalizedLinearVelocity > 0.01f)
            {
                // Если звук не воспроизводится, запускаем его
                if (!engineSFX.isPlaying)
                {
                    engineSFX.Play();
                }

                // Обновляем параметры звука
                engineSFX.pitch = 1f + engineSFXModifier * NormalizedLinearVelocity;
                engineSFX.volume = 0.5f + NormalizedLinearVelocity;
            }
            else
            {
                // Если транспортное средство не движется, останавливаем звук
                if (engineSFX.isPlaying)
                {
                    engineSFX.Stop();
                }
            }
        }
    }
}