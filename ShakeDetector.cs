using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeDetector : MonoBehaviour
{
    public float ShakeDetectionThreshold = 2.0f;
    public float MinShakeInterval = 0.5f;
    public float ShakeIntensityMultiplier = 1.5f; // New variable to control shake intensity

    public GameObject speechBubblePrefab; // Reference to the speech bubble prefab
    private GameObject speechBubbleInstance; // Instance of the speech bubble

    private float sqrShakeDetectionThreshold;
    private float timeSinceLastShake;
    private Vector3 initialPosition;

    private bool canShake = true; // New variable to control shake interval

    private float speechBubbleOffsetX = -0.7f;
    private float speechBubbleOffsetY = 0.7f;
    private float speechBubbleOffsetZ = 0f;
    public float shakeInterval = 2f;
    public float speechBubbleDuration = 1.7f;

    void Start()
    {
        const float shakeIntensity = 2.0f;
        // Calculate the square of the shake detection threshold using the shake intensity
        sqrShakeDetectionThreshold = Mathf.Pow(ShakeDetectionThreshold, shakeIntensity);
        initialPosition = transform.position;
    }

    void Update()
    {
        Vector3 acceleration = Input.acceleration;
        if (canShake && (Input.acceleration.sqrMagnitude >= sqrShakeDetectionThreshold || Input.GetKeyDown(KeyCode.W)) && Time.unscaledTime >= timeSinceLastShake + MinShakeInterval)
        { //Checks if the shake intensity is greater than the threshold and if the time since the last shake is greater than the minimum shake interval
            timeSinceLastShake = Time.unscaledTime;
            Debug.Log("Shake event detected at time " + Time.unscaledTime);
            Shake();
            ShowSpeechBubble();
            StartCoroutine(ShakeIntervalCoroutine());
        }
    }

    void Shake()
    {
        float shakeIntensity = 0.1f * ShakeIntensityMultiplier; // Multiply shake intensity by ShakeIntensityMultiplier
        float shakeDuration = 0.2f;

        StartCoroutine(ShakeCoroutine(shakeIntensity, shakeDuration));
    }

    IEnumerator ShakeCoroutine(float intensity, float duration)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            Vector3 randomOffset = Random.insideUnitSphere * intensity;
            transform.position = initialPosition + randomOffset;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = initialPosition;
    }

    void ShowSpeechBubble()
    {
        if (speechBubblePrefab != null)
        {
            if (speechBubbleInstance != null)
            {
                Destroy(speechBubbleInstance);
            }

            Vector3 spawnPosition = transform.position + new Vector3(speechBubbleOffsetX, speechBubbleOffsetY, speechBubbleOffsetZ); // Adjust the spawn position
            speechBubbleInstance = Instantiate(speechBubblePrefab, spawnPosition, Quaternion.identity);
            StartCoroutine(HideSpeechBubble());
        }
    }

    IEnumerator HideSpeechBubble()
    {
        yield return new WaitForSeconds(speechBubbleDuration);

        if (speechBubbleInstance != null)
        {
            Destroy(speechBubbleInstance);
        }
    }

  
    
    IEnumerator ShakeIntervalCoroutine()
    {
        
        canShake = false;
        yield return new WaitForSeconds(shakeInterval);
        canShake = true;
    }
}

