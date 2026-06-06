using UnityEngine;
using FMODUnity;

public class DEMON_jump_script : MonoBehaviour
{
    [SerializeField] private EventReference Death;
    [SerializeField] private GameObject player_G;
    [SerializeField] private float followSpeed;
    [SerializeField] private GameObject bloodParticle;

    void Start()
    {
        if (Player.Instance != null)
        {
            player_G = Player.Instance.gameObject;
        }
        else
        {
            Debug.LogWarning("Obiekt z komponentem 'player_script' nie zosta� znaleziony!");
        }

        Destroy(gameObject, 5f);
    }

    void Update()
    {
        if (player_G == null) return;

        Vector3 targetPos = player_G.transform.position + new Vector3(0, 1, 0);

        // Kierunek do gracza
        Vector3 direction = (targetPos - transform.position).normalized;

        // Ruch w stron� gracza
        transform.position += direction * followSpeed * Time.deltaTime;

        // Obr�t w stron� gracza (tylko w poziomie)
        if (direction != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == gameObject.layer)
        {
            RuntimeManager.PlayOneShot(Death, transform.position);

            if (bloodParticle != null)
            {
                Vector3 hitPosition = other.transform.position;
                var bloodObj = Instantiate(bloodParticle, hitPosition, Quaternion.identity);
                Destroy(bloodObj, 1f);
            }

            Destroy(gameObject);
        }

        if (other.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}