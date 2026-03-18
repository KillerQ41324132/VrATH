using UnityEngine;
using UnityEngine.SceneManagement;

public class main_menu_script : MonoBehaviour
{

    public void OnQuitGame()
    {
        Application.Quit();
    }
    public void OnStartGame()
    {
        SceneManager.LoadScene("SampleScene");
    }

}
