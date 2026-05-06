using UnityEngine;
using UnityEngine.SceneManagement;

public class Next_level_script : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
       
        if (other.CompareTag("Player") || other.CompareTag("Dead"))
        {
            SceneManager.LoadScene("level_0");
        }
    }
}
