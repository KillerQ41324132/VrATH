using UnityEngine;
using FMODUnity;

public class Player_hand_script : MonoBehaviour
{
    private float lastShootTime = -Mathf.Infinity;
    private float shootCooldown = 1f;

    [SerializeField] private float bulletSpeed = 10f;
    [SerializeField] private EventReference shootSound;
    [SerializeField] private GameObject armInteraction;
    [SerializeField] private GameObject pistolBullet;
    [SerializeField] private GameObject wholeArm;
    [SerializeField] private GameObject mapObject;
    [SerializeField] private EventReference reload;

    private int ammo;
    private bool isDead = false;
    private bool HouseZone;

    private bool[] hasShotBulletType = new bool[5]; // 1 = A ... 5 = E

    [Range(0f, 100f)]
    public float stress = 0f;
    private float stressMax = 100f;

    private bool isIncreasing = false;
    private bool isDecreasing = false;

    private float increaseDuration = 0.5f;
    private float decreaseDuration = 3f;

    private float increaseTimer = 0f;
    private float decreaseTimer = 0f;

    private int selectedBulletType = 1;

    private Vector3 targetPos;
    private Quaternion targetRot;
    private float showingArmSpeed = 5f;

    // Recoil
    private bool isRecoiling = false;
    private float recoilTimer = 0f;
    private float recoilDuration = .03f;
    private Quaternion defaultArmRotation;

    // Map animation
    private Vector3 mapShownPos = new Vector3(0f, 0.0272f, 0.0165f);
    private Vector3 mapHiddenPos = new Vector3(0f, 0f, 0f); // pozycja poza ekranem
    private Quaternion mapShownRot = Quaternion.Euler(0f, -90f, 60f);
    private Quaternion mapHiddenRot = Quaternion.identity;

    void Start()
    {
        ammo = 5;
        defaultArmRotation = Quaternion.Euler(90f, -90f, -90f);
        for (int i = 0; i < hasShotBulletType.Length; i++)
            hasShotBulletType[i] = false;

        // Start map hidden
        if (mapObject != null)
        {
            mapObject.transform.localPosition = mapHiddenPos;
            mapObject.transform.localRotation = mapHiddenRot;
        }
    }

    void Update()
    {
        // Wybór typu pocisku
        for (int i = 1; i <= 5; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + (i - 1)))
            {
                selectedBulletType = i;
                RuntimeManager.PlayOneShot(reload);
                break;
            }
        }

        // Strza³
        if (Input.GetMouseButtonDown(0) && !isDead)
        {
            if (transform.position.x > -0.2f || ammo > 0)
            {
                Shoot();

                if (!HouseZone && !isIncreasing && !isDecreasing)
                {
                    isIncreasing = true;
                    increaseTimer = 0f;
                }
            }
        }

        // Stres
        if (isIncreasing)
        {
            increaseTimer += Time.deltaTime;
            float percent = Mathf.Clamp01(increaseTimer / increaseDuration);
            stress = Mathf.Lerp(0f, stressMax, percent);

            if (increaseTimer >= increaseDuration)
            {
                isIncreasing = false;
                isDecreasing = true;
                decreaseTimer = 0f;
            }
        }
        else if (isDecreasing)
        {
            decreaseTimer += Time.deltaTime;
            float percent = Mathf.Clamp01(decreaseTimer / decreaseDuration);
            stress = Mathf.Lerp(stressMax, 0f, percent);

            if (decreaseTimer >= decreaseDuration)
            {
                isDecreasing = false;
            }
        }

        // Stres pozycyjny
        float playerX = transform.position.x;
        float positionalStress = 0f;

        if (playerX >= 4f && playerX <= 16f)
        {
            positionalStress = Mathf.InverseLerp(4f, 16f, playerX) * stressMax;
        }
        else if (playerX > 16f)
        {
            positionalStress = stressMax;
        }

        stress = Mathf.Min(Mathf.Max(stress, positionalStress), stressMax);

        HouseZone = transform.position.x < -0.2f;

        // Animacja ramienia: pozycja i rotacja
        if (isRecoiling)
        {
            recoilTimer += Time.deltaTime;
            targetPos = new Vector3(1f, 0.05f, 0f);
            targetRot = Quaternion.Euler(30f, -80f, -80f);

            if (recoilTimer >= recoilDuration)
            {
                isRecoiling = false;
                targetRot = defaultArmRotation;
            }
        }
        else
        {
            if (HouseZone || Input.GetKey(KeyCode.Tab))
            {
                targetPos = new Vector3(0.4f, 0f, -0.3f);
                targetRot = Quaternion.Euler(144f, -84f, -84f);
            }
            else
            {
                targetPos = new Vector3(0.31f, 0.15f, 0f);
                targetRot = defaultArmRotation;
            }
        }

        // P³ynne przejœcie ramienia
        wholeArm.transform.localPosition = Vector3.Lerp(wholeArm.transform.localPosition, targetPos, Time.deltaTime * showingArmSpeed);
        wholeArm.transform.localRotation = Quaternion.Lerp(wholeArm.transform.localRotation, targetRot, Time.deltaTime * showingArmSpeed);

        // P³ynne przejœcie mapy
        if (mapObject != null)
        {
            bool tabHeld = Input.GetKey(KeyCode.Tab);
            Vector3 targetMapPos = tabHeld ? mapShownPos : mapHiddenPos;
            Quaternion targetMapRot = tabHeld ? mapShownRot : mapHiddenRot;

            mapObject.transform.localPosition = Vector3.Lerp(mapObject.transform.localPosition, targetMapPos, Time.deltaTime * showingArmSpeed);
            mapObject.transform.localRotation = Quaternion.Lerp(mapObject.transform.localRotation, targetMapRot, Time.deltaTime * showingArmSpeed);
        }
    }

    void Shoot()
    {
        if (!HouseZone && Time.time - lastShootTime < shootCooldown)
            return;

        if (!HouseZone && ammo <= 0)
        {
            Debug.Log("Brak amunicji!");
            return;
        }

        int typeIndex = selectedBulletType - 1;

        if (!HouseZone && hasShotBulletType[typeIndex])
        {
            Debug.Log($"Typ {selectedBulletType} ju¿ u¿yty!");
            return;
        }

        GameObject prefabToUse = HouseZone ? armInteraction : pistolBullet;
        GameObject bullet = Instantiate(prefabToUse, wholeArm.transform.position, transform.rotation);

        if (!HouseZone)
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
                else
                    Debug.LogWarning($"Layer '{layerName}' nie istnieje!");
            }
        }

        Bullet bulletScript = bullet.GetComponent<Bullet>();
        if (bulletScript != null)
            bulletScript.SetSpeed(bulletSpeed);

        if (!HouseZone)
        {
            ammo--;
            hasShotBulletType[typeIndex] = true;
            lastShootTime = Time.time;
            AudioManager.instance.PlayOneShot(shootSound, transform.position);
            Debug.Log($"Wystrzelono typ {selectedBulletType} | Pozosta³a amunicja: {ammo}");

            isRecoiling = true;
            recoilTimer = 0f;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Demon"))
        {
            gameObject.tag = "Dead";
            isDead = true;
            stress = 0;
        }

        if (other.CompareTag("House"))
        {
            ammo = 5;
            isDead = false;

            // Reset typów pocisków
            for (int i = 0; i < hasShotBulletType.Length; i++)
                hasShotBulletType[i] = false;
        }
    }
}
