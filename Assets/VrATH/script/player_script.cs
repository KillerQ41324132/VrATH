using UnityEngine;
using System.Collections;
using FMODUnity;
using TMPro;
using UnityEngine.SceneManagement;
public class player_script : MonoBehaviour
{

    private Transform killerTransform; // Referencja do zabójcy
    private float lookDuration = 0.5f;
    private float lookTimer = 0f;

    private bool isDead = false;
    private bool win = false;
    private Vector3 spawnPoint;
    [SerializeField] private float acceleration = 2f;
    [SerializeField] private float startSpeed = 0.1f;
    [SerializeField] private float delayBeforeReturn = 1f;

    [SerializeField] private float sensitivity = 2.0f;
    [SerializeField] private float maxYAngle = 45f;
    [SerializeField] private float moveSpeed;
    [SerializeField] private EventReference deathPlayer;
    [SerializeField] private EventReference carWin;


    private Vector2 currentRotation;

    public static player_script Instance { get; private set; }

    //kroki
    [SerializeField] private EventReference HouseWalking;
    [SerializeField] private EventReference walking;
    private float stepInterval = 0.6f;
    private float stepTimer = 0f;

    private bool HouseZone;
    [SerializeField] private GameObject canvas;
    [SerializeField] private TextMeshProUGUI timerText;
    private float currentTime = 0f;

    private void Awake()
    {
        canvas.SetActive(false);
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {

        spawnPoint = transform.position;
        currentRotation = new Vector2(90f, 0f);
        transform.rotation = Quaternion.Euler(currentRotation.y, currentRotation.x, 0);

    }

    void Update()
    {
        //timer
        HouseZone = transform.position.x < 0f;
        if (!HouseZone && !win)
        {
            currentTime += Time.deltaTime;
            UpdateTimerDisplay();
        }

        if (!win && !isDead)
        {
            float mouseX = Input.GetAxis("Mouse X") * sensitivity;
            float mouseY = Input.GetAxis("Mouse Y") * sensitivity;

            currentRotation.x += mouseX;
            currentRotation.y -= mouseY;
            currentRotation.y = Mathf.Clamp(currentRotation.y, -maxYAngle, maxYAngle);

            transform.rotation = Quaternion.Euler(currentRotation.y, currentRotation.x, 0);
        }
        else if(win && isDead && killerTransform != null)
        {
            SmoothLookAtKiller(killerTransform);
        }
        if (Input.GetMouseButton(1) && !win && !isDead || Input.GetKey(KeyCode.W) && !win && !isDead ) // to lub to, ale bez tego albo/lub 
        {
            transform.position += Vector3.right * moveSpeed * Time.deltaTime;

            if (Input.GetMouseButton(1) || Input.GetKey(KeyCode.W))
            {
                stepTimer -= Time.deltaTime;

                if (stepTimer <= 0f)
                {
                    if (HouseZone)
                    {
                    RuntimeManager.PlayOneShot(HouseWalking, transform.position);
                    }
                    else
                    {
                    RuntimeManager.PlayOneShot(walking, transform.position);
                    }
                    stepTimer = stepInterval;
                }
            }
            else
            {
                stepTimer = 0f;
            }
        }
        Vector3 pos = transform.position;
        if (pos.x >= 0f)
        {
            pos.y = 0.15f;
        }
        else if (pos.x >= -0.1f)
        {
            pos.y = Mathf.Lerp(0.17f, 0.15f, Mathf.InverseLerp(-0.1f, 0f, pos.x));
        }
        else
        {
            pos.y = 0.17f;
        }
        transform.position = pos;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Demon"))
        {
            Debug.Log("DEATH");
            gameObject.tag = "Dead";
            isDead = true;
            killerTransform = other.transform;
            lookTimer = 0f; // resetujemy licznik
            RuntimeManager.PlayOneShot(deathPlayer);
            ReturnToSpawn();
            GameManager.Instance.Placing();
        }
        if (other.CompareTag("House"))
        {
            gameObject.tag = "Player";
            isDead = false;
            currentTime = 0f;
            killerTransform = null; // Resetujemy, bo gracz ju¿ nie jest martwy
        }
        if (other.CompareTag("car"))
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
            RuntimeManager.PlayOneShot(carWin, transform.position);
            win = true;
            canvas.SetActive(true);

        }
    }

    public void ReturnToSpawn()
    {
        StartCoroutine(MoveToSpawnWithAcceleration());
    }

    private IEnumerator MoveToSpawnWithAcceleration()
    {
        yield return new WaitForSeconds(delayBeforeReturn);

        float speed = startSpeed;

        while (Vector3.Distance(transform.position, spawnPoint) > 0.01f)
        {
            if (!isDead) yield break; // PRZERYWAMY, jeœli gracz ju¿ nie jest "martwy"

            speed += acceleration * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, spawnPoint, speed * Time.deltaTime);
            yield return null;
        }

        transform.position = spawnPoint;
    }

    private void SmoothLookAtKiller(Transform killer)
    {
        Vector3 directionToKiller = killer.position - transform.position;
        directionToKiller.y = 0; // ignorujemy oœ Y

        if (directionToKiller == Vector3.zero) return;

        Quaternion targetRotation = Quaternion.LookRotation(directionToKiller);
        lookTimer += Time.deltaTime / lookDuration;

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, lookTimer);
    }

    void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(currentTime / 60f);
        int seconds = Mathf.FloorToInt(currentTime % 60f);

        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
    public void OnMainMenuGame()
    {
        SceneManager.LoadScene("start");

    }

}
