using UnityEngine;
using FMODUnity;

public class DEMON_jump_script : MonoBehaviour
{
    [SerializeField] private EventReference Death;
    [SerializeField] private GameObject player;
    [SerializeField] private float jumpDuration = 2.0f;
    [SerializeField] private float followSpeed = 0.2f;
    [SerializeField] private GameObject bloodParticle;

    private bool isJumping = false;
    private bool isFollowing = false;

    private Vector3 startPoint;
    private Vector3 midPoint;
    private float jumpHeight;
    private float timer;

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

        Destroy(gameObject, 8f);
        StartJump();
    }
    void Update()
    {
        if (isJumping)
        {
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / jumpDuration);

            Vector3 endPoint = player.transform.position;
            Vector3 flatDistance = new Vector3(endPoint.x - startPoint.x, 0f, endPoint.z - startPoint.z);
            jumpHeight = flatDistance.magnitude / 6f;

            Vector3 pos = Vector3.Lerp(startPoint, endPoint, t);
            pos.y += 4 * jumpHeight * t * (1 - t);
            transform.position = pos;

            if (t >= 0.5f)
            {
                isJumping = false;
                isFollowing = true;
            }
        }
        else if (isFollowing)
        {
            if (player == null) return;

            Vector3 targetPos = player.transform.position;
            Vector3 direction = (targetPos - transform.position).normalized;
            transform.position += direction * followSpeed * Time.deltaTime;

            // Opcjonalnie: obracaj w locie
            if (direction != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            }
        }
    }

    void StartJump()
    {
        if (player == null) return;

        startPoint = transform.position;

        Vector3 lookDirection = player.transform.position - startPoint;
        lookDirection.y = 0f;

        if (lookDirection != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(lookDirection.normalized);

        timer = 0f;
        isJumping = true;
        isFollowing = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == gameObject.layer)
        {
            RuntimeManager.PlayOneShot(Death, transform.position);
            Vector3 hitPosition = other.transform.position;
            var bloodObj = Instantiate(bloodParticle, hitPosition, Quaternion.identity);
            Destroy(bloodObj, 1f);
            Destroy(gameObject);
        }
        if (other.CompareTag("Player"))
        {
            isJumping = false;
            isFollowing = false;
        }
    }
}
