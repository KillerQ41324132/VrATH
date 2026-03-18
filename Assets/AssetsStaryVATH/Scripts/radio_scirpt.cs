using FMODUnity;
using UnityEngine;
using FMOD.Studio;


public class radio_scirpt : MonoBehaviour
{
    [SerializeField] private EventReference Radio;
    [SerializeField] private EventReference RadioOff;
    private bool isRadioOn;

    private EventInstance Radioi;
    private EventInstance RadioiOff;

    private void Start()
    {
        isRadioOn = false;
        Radioi = RuntimeManager.CreateInstance(Radio);
        RadioiOff = RuntimeManager.CreateInstance(RadioOff);

        Radioi.set3DAttributes(RuntimeUtils.To3DAttributes(transform));
        RadioiOff.set3DAttributes(RuntimeUtils.To3DAttributes(transform));

        transform.Rotate(0f, 0f, 0f);

        // Odtwórz dŸwiêk wy³¹czonego radia
        Radioi.start();
    }

    private void Update()
    {
        Radioi.set3DAttributes(RuntimeUtils.To3DAttributes(transform));
        RadioiOff.set3DAttributes(RuntimeUtils.To3DAttributes(transform));
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Hand"))
        {
            if (!isRadioOn)
            {
                transform.Rotate(0f, 0f, -80f);
                Radioi.start();
                RadioiOff.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                isRadioOn = true;
                Debug.Log("Radio ON");
            }
            else
            {
                transform.Rotate(0f, 0f, 80f);
                RadioiOff.start();
                Radioi.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                isRadioOn = false;
                Debug.Log("Radio OFF");
            }
        }
    }
}

