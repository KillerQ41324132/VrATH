using UnityEngine;
using FMODUnity;

public class VIBE_sounds_script : MonoBehaviour
{
    [SerializeField] private EventReference vibeSound; // Przypisz dŸwiêk w Inspectorze

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            RuntimeManager.PlayOneShot(vibeSound, transform.position);
        }
    }
}
