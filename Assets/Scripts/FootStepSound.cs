using System;
using UnityEngine;

[Serializable]
public class FootStepProperties
{
    public float Speed;
    public float Delay;
}

public class FootStepSound : MonoBehaviour
{
    [SerializeField] private FootStepProperties[] propertiesArray;
    [SerializeField] private CharacterController characterController;
    [SerializeField] private NoiseAudioSource audioSource;

    private float delay;
    private float tick;

    private float GetSpeed()
    {
        return characterController.velocity.magnitude;
    }

    private float GetDelayBySpeed(float speed)
    {
        for (int i = 0; i < propertiesArray.Length; i++)
        {
            if (speed <= propertiesArray[i].Speed)
            {
                return propertiesArray[i].Delay;
            }
        }

        return propertiesArray[propertiesArray.Length - 1].Delay;
    }

    private bool IsPlay()
    {
        if (GetSpeed() < 0.01f || !characterController.isGrounded) return false;
        else
            return true;
    }

    private void Update()
    {
        if (!IsPlay())
        {
            return;
            tick = 0;
        }

        tick += Time.deltaTime;
        delay = GetDelayBySpeed(GetSpeed());

        if (tick >= delay)
        {
            audioSource.Play();
            tick = 0;
        }
    }
}