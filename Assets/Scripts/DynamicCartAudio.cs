using UnityEngine;
using Dreamteck.Splines;

public class DynamicCartAudio : MonoBehaviour
{
    [Header("Компоненты")]
    public SplineFollower mainFollower;
    public SplineFollower detourFollower;

    [Header("Источники звука")]
    public AudioSource windSource;
    public AudioSource railSource;

    [Header("Настройки")]
    public float maxSpeedForVolume = 30f;
    public float minPitch = 0.8f;
    public float maxPitch = 1.5f;
    public float smoothingSpeed = 5f;

    [Header("Громкость")]
    public float railVolumeMultiplier = 1.5f; 

    public bool isOnLift = false;

    private Vector3 lastPos;
    private float smoothedSpeed = 0f;

    void Start()
    {
        lastPos = transform.position;
    }

    void LateUpdate()
    {
        bool isMoving = (mainFollower != null && mainFollower.enabled) ||
                        (detourFollower != null && detourFollower.enabled);

        if (!isMoving || Time.deltaTime <= 0)
        {
            if (windSource != null) windSource.volume = 0;
            if (railSource != null) railSource.volume = 0;
            lastPos = transform.position;
            smoothedSpeed = 0f;
            return;
        }

        float currentDist = (transform.position - lastPos).magnitude;
        float rawSpeed = currentDist / Time.deltaTime;
        lastPos = transform.position;

        smoothedSpeed = Mathf.Lerp(smoothedSpeed, rawSpeed, Time.deltaTime * smoothingSpeed);

        float speedFactor = Mathf.Clamp01(smoothedSpeed / maxSpeedForVolume);

        if (windSource != null)
        {
            float windVolume = isOnLift ? 0f : speedFactor;

            windSource.volume = windVolume;
            windSource.pitch = Mathf.Lerp(minPitch, maxPitch, speedFactor);
        }

        if (railSource != null)
        {
            float railVolume = 0f;
            if (speedFactor > 0.01f)
            {
                railVolume = (speedFactor * 0.8f + 0.2f) * railVolumeMultiplier;
            }
            railSource.volume = Mathf.Clamp01(railVolume);
        }
    }
}