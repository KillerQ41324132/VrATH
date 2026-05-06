using UnityEngine;
using FMODUnity;
using TMPro;
using UnityEngine.InputSystem;

public class Player_hand_script : MonoBehaviour
{
    private float lastShootTime = -Mathf.Infinity;
    private float shootCooldown = 1f;

    [SerializeField] private float bulletSpeed = 10f;
    [SerializeField] private EventReference shootSound;
    [SerializeField] private EventReference reload;

    [SerializeField] private GameObject armInteraction;
    [SerializeField] private GameObject pistolBullet;

    [Header("Input System")]
    [SerializeField] private InputActionReference rightJoystick;
    [SerializeField] private Transform bullet_start_point;
    [SerializeField] private InputActionReference shootAction;
    private int ammo;
    private bool isDead = false;
    [SerializeField] private bool HouseZone;

    private bool[] hasShotBulletType = new bool[5];
    private int selectedBulletType = 1;

    [SerializeField] private float joystickDeadzone = 0.7f;
    private bool joystickInUse = false;

    // ✅ TEXT 3D (NA PLANE / W ŚWIECIE)
    [SerializeField] private TextMeshPro currentBulletTypeText;

    void OnEnable()
    {
        rightJoystick.action.Enable();
        shootAction.action.Enable();
        shootAction.action.performed += OnShoot;
    }

    void OnDisable()
    {
        rightJoystick.action.Disable();
        shootAction.action.performed -= OnShoot;
        shootAction.action.Disable();
    }

    void Start()
    {
        ammo = 5;

        for (int i = 0; i < hasShotBulletType.Length; i++)
        {
            if (i == 10)
            {
                return;
            }
            hasShotBulletType[i] = false;

        }


        UpdateBulletText(); // ✅ ustaw tekst na start
    }

    void Update()
    {
        HandleJoystickInput();
        
    }

    void OnShoot(InputAction.CallbackContext context)
    {
        if (isDead)
            return;
        if(ammo > 0)
        {
            Shoot();
        }

    }

    void HandleJoystickInput()
    {
        Vector2 input = rightJoystick.action.ReadValue<Vector2>();
        float x = input.x;

        if (!joystickInUse)
        {
            if (x > joystickDeadzone)
            {
                int attempts = 0;
                do
                {
                    selectedBulletType++;
                    if (selectedBulletType > 5)
                        selectedBulletType = 1;
                    attempts++;
                }
                while (hasShotBulletType[selectedBulletType - 1] && attempts < 5);
                RuntimeManager.PlayOneShot(reload);
                Debug.Log("Typ: " + selectedBulletType);

                UpdateBulletText(); // ✅ aktualizacja tekstu
                joystickInUse = true;
            }
            else if (x < -joystickDeadzone)
            {
                int attempts = 0;
                do
                {
                    selectedBulletType--;
                    if (selectedBulletType < 1)
                        selectedBulletType = 5;
                    attempts++;
                }
                while (hasShotBulletType[selectedBulletType - 1] && attempts < 5);
                
                
                RuntimeManager.PlayOneShot(reload);
                Debug.Log("Typ: " + selectedBulletType);

                UpdateBulletText(); // ✅ aktualizacja tekstu
                joystickInUse = true;
            }
        }

        if (Mathf.Abs(x) < 0.2f)
        {
            joystickInUse = false;
        }
    }

    void UpdateBulletText()
    {
        if (currentBulletTypeText != null)
            currentBulletTypeText.text = selectedBulletType.ToString();
    }

    void Shoot()
    {

        if (Time.time - lastShootTime < shootCooldown)
            return;

        if (ammo <= 0)
        {
            Debug.Log("Brak amunicji!");
            return;
        }

        int typeIndex = selectedBulletType - 1;

        if (hasShotBulletType[typeIndex])
        {
            Debug.Log($"Typ {selectedBulletType} już użyty!");
            return;
        }

        GameObject prefabToUse = HouseZone ? armInteraction : pistolBullet;
        GameObject bullet = Instantiate(prefabToUse, bullet_start_point.position, bullet_start_point.rotation);

        if (HouseZone)
        {
            string layerName = selectedBulletType switch
            {
                1 => "Fint",
                2 => "Bint",
                3 => "Cint",
                4 => "Dint",
                5 => "Eint",
                _ => null
            };

            if (!string.IsNullOrEmpty(layerName))
            {
                int layer = LayerMask.NameToLayer(layerName);
                if (layer != -1)
                    bullet.layer = layer;
            }
        }

        Bullet bulletScript = bullet.GetComponent<Bullet>();
        if (bulletScript != null)
        {
            Debug.Log("shot");
            bulletScript.SetSpeed(bulletSpeed);


        }


        ammo--;
            hasShotBulletType[typeIndex] = true;
            lastShootTime = Time.time;

        //AudioManager.instance.PlayOneShot(shootSound, transform.position);
        Debug.Log($"Strzał typ {selectedBulletType} | ammo: {ammo}");
        UpdateBulletText();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Demon"))
        {
            gameObject.tag = "Dead";
            isDead = true;
        }

        if (other.CompareTag("House"))
        {
            ammo = 5;
            isDead = false;

            for (int i = 0; i < hasShotBulletType.Length; i++)
                hasShotBulletType[i] = false;
        }
    }
}