using UnityEngine;
using UnityEngine.UI;

public class main_camera_script : MonoBehaviour
{
    [SerializeField] private float maxRotation = 3f;
    [SerializeField] private float idleSpeed = 1f;
    [SerializeField] private float idleAmount = 1f;
    private Vector3 initialRotation;
    [SerializeField] private GameObject canvas;
         
    //click
    [SerializeField] private Button moveButton;          // Przycisk UI
    [SerializeField] private float moveSpeed = 2f;       // Szybkoœæ ruchu
    [SerializeField] private float rotateSpeed = 2f;     // Szybkoœæ rotacji
    [SerializeField] private bool shouldMove = false;   // Bool aktywuj¹cy ruch

    private Vector3 startPos = new Vector3(3.8f, 0.18f, 0.22f);
    private Vector3 endPos = new Vector3(-0.615f, 0.22f, 0.1f);

    private Quaternion startRot = Quaternion.Euler(0f, -99f, 0f);
    private Quaternion endRot = Quaternion.Euler(-5.5f, -87f, 0f);
    private bool isInHowToPlay = false; // Czy jesteœmy w trybie "how to play"
    private void Start()
    {
        initialRotation = transform.eulerAngles;


        transform.position = startPos;
        transform.rotation = startRot;

        if (moveButton != null)
            moveButton.onClick.AddListener(OnEnterHowToPlay);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnExitHowToPlay();
        }

        // Pobranie pozycji myszy w zakresie od -1 do 1
        float mouseX = (Input.mousePosition.x / Screen.width - 0.5f) * 2f;
        float mouseY = (Input.mousePosition.y / Screen.height - 0.5f) * 2f;

        // Odwrócenie osi
        float targetYaw = mouseX * maxRotation;    // Lewo-prawo
        float targetPitch = -mouseY * maxRotation; // Góra-dó³

        // Idle movement
        float idleYaw = Mathf.Sin(Time.time * idleSpeed) * idleAmount;
        float idlePitch = Mathf.Cos(Time.time * idleSpeed * 0.8f) * idleAmount;

        // Koñcowa rotacja
        Vector3 finalRotation = new Vector3(
            initialRotation.x + targetPitch + idlePitch,
            initialRotation.y + targetYaw + idleYaw,
            initialRotation.z
        );

        // P³ynne przejœcie
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(finalRotation), Time.deltaTime * 3f);


        if (shouldMove)
        {
            Vector3 targetPos = isInHowToPlay ? endPos : startPos;
            Quaternion targetRot = isInHowToPlay ? endRot : startRot;

            // Smooth Position
            transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * moveSpeed);

            // Smooth Rotation
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, Time.deltaTime * rotateSpeed);

            // Stop when close enough
            if (Vector3.Distance(transform.position, targetPos) < 0.01f &&
                Quaternion.Angle(transform.rotation, targetRot) < 1f)
            {
                transform.position = targetPos;
                transform.rotation = targetRot;
                shouldMove = false;
            }
        }
    }

    private void OnEnterHowToPlay()
    {
        isInHowToPlay = true;
        shouldMove = true;
        canvas.SetActive(false);
    }

    private void OnExitHowToPlay()
    {
        isInHowToPlay = false;
        shouldMove = true;
        canvas.SetActive(true);
    }

}
