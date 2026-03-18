using System.Collections;
using FMODUnity;
using UnityEngine;

public class door_script : MonoBehaviour
{
    [SerializeField] private EventReference doorSoundClose; // Przypisz dŸwiêk w Inspectorze
    [SerializeField] private EventReference doorSoundOpen; // Przypisz dŸwiêk w Inspectorze
    public static door_script Instance;
    private Animator doorAnimator;
    private void Awake()
    {
        Instance = this;
        doorAnimator = GetComponent<Animator>();
    }

    public void closingDoor()
    {
        RuntimeManager.PlayOneShot(doorSoundClose, transform.position);
        doorAnimator.SetTrigger("ClosingTrigger");
    }
    public void openingDoor()
    {
        RuntimeManager.PlayOneShot(doorSoundOpen, transform.position);
        doorAnimator.SetTrigger("OpeningTrigger");
    }
}  
