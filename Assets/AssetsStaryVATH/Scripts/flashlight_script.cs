using UnityEngine;

public class flashlight_script : MonoBehaviour
{
    private Light spotLight;
    private float baseIntensity = 0.04f;
    private float heldIntensity = 0.1f;
    private float minAngle = 56f;
    private float maxAngle = 96f;
    private float angleStep = 8f;

    void Start()
    {
        spotLight = GetComponent<Light>();

        if (spotLight == null || spotLight.type != LightType.Spot)
        {
            Debug.LogError("Brak komponentu Light typu Spot!");
        }
    }

    void Update()
    {
        if (spotLight == null) return;

        // Zwiêksz k¹t
        if (Input.GetKeyDown(KeyCode.C)) 
        {
            float newAngle = Mathf.Min(spotLight.spotAngle + angleStep, maxAngle);
            spotLight.spotAngle = newAngle;
        }

        // Zmniejsz k¹t
        if (Input.GetKeyDown(KeyCode.Z)) 
        {
            float newAngle = Mathf.Max(spotLight.spotAngle - angleStep, minAngle);
            spotLight.spotAngle = newAngle;
        }

        // Intensity na trzymanie "/"
        if (Input.GetKey(KeyCode.X)) 
        {
            spotLight.intensity = heldIntensity;
        }
        else
        {
            spotLight.intensity = baseIntensity;
        }
    }
}
