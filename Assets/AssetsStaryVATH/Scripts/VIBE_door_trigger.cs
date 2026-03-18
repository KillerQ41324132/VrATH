using FMODUnity;
using UnityEngine;

public class VIBE_door_trigger : MonoBehaviour
{
    

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            door_script.Instance.closingDoor();

            Debug.Log("zamykanie drzwi");

        }

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Dead"))
        {
            door_script.Instance.openingDoor();

            Debug.Log("otwieranie drzwi");

        }
    }
}
