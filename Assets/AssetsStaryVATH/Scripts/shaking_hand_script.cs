using UnityEngine;

public class shaking_hand_script : MonoBehaviour
{
    [Header("Rotacja (szybkie dr¿enie w osi X)")]
    [Tooltip("Zakres rotacji w osi X (np. 20 = -20 do +20 stopni)")]
    private float rotationRange = 4f;

    [Tooltip("Jak szybko rotacja dr¿y")]
    private float rotationFrequency = 10f;

    [Header("Pozycja (mikrodr¿enie)")]
    private float positionAmplitude = 0.004f;
    private float positionFrequency = 7f;

    private Quaternion initialRotation;
    private Vector3 initialPosition;
    private float timeOffset;

    void Start()
    {
        initialRotation = transform.localRotation;
        initialPosition = transform.localPosition;
        timeOffset = Random.Range(0f, 100f); // unikalny dla ka¿dego obiektu
    }

    void Update()
    {
        float time = Time.time + timeOffset;

        // ROTACJA – szybkie drgania w osi X
        float rotNoise = Mathf.PerlinNoise(time * rotationFrequency, 0f);
        float rotX = Mathf.Lerp(-rotationRange, rotationRange, rotNoise);
        Quaternion xRotation = Quaternion.Euler(rotX, 0f, 0f);
        transform.localRotation = initialRotation * xRotation;

        // POZYCJA – delikatne mikrodr¿enie
        float offsetX = (Mathf.PerlinNoise(0f, time * positionFrequency) * 2f - 1f) * positionAmplitude;
        float offsetY = (Mathf.PerlinNoise(1f, time * positionFrequency) * 2f - 1f) * positionAmplitude;
        float offsetZ = (Mathf.PerlinNoise(2f, time * positionFrequency) * 2f - 1f) * positionAmplitude;

        transform.localPosition = initialPosition + new Vector3(offsetX, offsetY, offsetZ);
    }
}
