using UnityEngine;

public class weather_following_player_script : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private float speed;

    void Start()
    {
        if (Player.Instance != null)
        {
            player = Player.Instance.gameObject;
        }
        else
        {
            Debug.LogWarning("Obiekt z komponentem 'player_script' nie zosta³ znaleziony!");
        }
    }

    void Update()
    {
        if (player != null)
        {
            Vector3 currentPosition = transform.position;

            // Ustawiamy z taki sam jak u gracza
            transform.position = new Vector3(currentPosition.x, currentPosition.y, player.transform.position.z);

        }
    }
}
