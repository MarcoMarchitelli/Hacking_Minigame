using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(CinemachineVirtualCamera))]
public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance;

    CinemachineBasicMultiChannelPerlin noise;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        noise = GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public void Shake(float _amplitude, float _frequency, float _duration)
    {
        StartCoroutine(ShakeRoutine(_amplitude, _frequency, _duration));
    }

    IEnumerator ShakeRoutine(float _amplitude, float _frequency, float _duration)
    {
        noise.m_FrequencyGain = _frequency;
        noise.m_AmplitudeGain = _amplitude;

        yield return new WaitForSeconds(_duration);

        noise.m_FrequencyGain = 0;
        noise.m_AmplitudeGain = 0;
    }
}