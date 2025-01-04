using UnityEngine;
using Cinemachine;

public class CameraShakeManager : MonoBehaviour
{
    public static CameraShakeManager Instance;

    [SerializeField] private CinemachineVirtualCamera cinemachineCamera;
    [SerializeField] private float shakeDuration = 0.2f;
    [SerializeField] private float shakeAmplitude = 1f; 
    [SerializeField] private float shakeFrequency = 1f; 

    private CinemachineBasicMultiChannelPerlin cinemachinePerlin;
    private float shakeTimer;

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        cinemachinePerlin = cinemachineCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public void TriggerShake()
    {
        cinemachinePerlin.m_AmplitudeGain = shakeAmplitude;
        cinemachinePerlin.m_FrequencyGain = shakeFrequency;
        shakeTimer = shakeDuration;
    }

    private void Update()
    {
        if(shakeTimer <= 0) return;

        shakeTimer -= Time.deltaTime;
        if(shakeTimer <= 0)
        {
            cinemachinePerlin.m_AmplitudeGain = 0f;
            cinemachinePerlin.m_FrequencyGain = 0f;
        }
    }
}
