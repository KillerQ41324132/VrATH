using UnityEngine;

public class weather_following_player_script : MonoBehaviour
{
    private GameObject player;
    public float speed = 1f;

    void Start()
    {

        if (player_script.Instance != null)
        {
            player = player_script.Instance.gameObject;
        }
        else
        {
            Debug.LogWarning("Obiekt z komponentem 'player_script' nie zosta³ znaleziony!");
        }
    }

    void Update()
    {
        if (player != null && player.transform.position.x > 2.2f)
        {
            Vector3 currentPosition = transform.position;

            // Ustawiamy X taki sam jak u gracza
            transform.position = new Vector3(player.transform.position.x, currentPosition.y, currentPosition.z);

        }
    }
}
