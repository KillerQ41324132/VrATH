using UnityEngine;
using UnityEngine.SceneManagement;


public class lifescript : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Demon"))
        {
            SceneManager.LoadScene("level_0");
        }
    }
}
