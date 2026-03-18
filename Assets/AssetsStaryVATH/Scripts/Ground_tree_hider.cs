using UnityEngine;
public class Ground_tree_hider : MonoBehaviour
{
    [SerializeField] GameObject tree1;
    [SerializeField] GameObject tree2;

    public int numberOfSpawns = 10;
    void Awake()
    {
        for (int i = 0; i < numberOfSpawns; i++)
        {
            SpawnObject(tree1);
            SpawnObject(tree2);
        }
        if (transform.position.x > 3f)
        {
            foreach (Transform child in gameObject.transform)
            {
                child.gameObject.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Dead"))
        {
            foreach (Transform child in gameObject.transform)
            {
                child.gameObject.SetActive(true);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Dead"))
        {
            foreach (Transform child in gameObject.transform)
            {
                child.gameObject.SetActive(false);
            }
        }
    }
    //[SerializeField] bool isHide = false;
    //void Start()
    //{
    //    for (int i = 0; i < numberOfSpawns; i++)
    //    {
    //        SpawnObject(tree1);
    //        SpawnObject(tree2);
    //    }
    //    if (transform.position.x > 3f)
    //    {
    //        isHide = true;
    //        foreach (Transform child in gameObject.transform)
    //        {
    //            child.gameObject.SetActive(false);
    //        }
    //    }
    //    else
    //    {
    //        isHide = false;
    //    }

    //}
    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("Player") || other.CompareTag("Dead"))
    //    {

    //        if (!isHide)
    //        {
    //            isHide = true;
    //            foreach (Transform child in gameObject.transform)
    //            {
    //                child.gameObject.SetActive(false);
    //            }
    //        }
    //        else
    //        {
    //            foreach (Transform child in gameObject.transform)
    //            {
    //                child.gameObject.SetActive(true);
    //            }
    //            isHide = false;
    //        }

    //    }
    //}

    void SpawnObject(GameObject prefab)
    {
        float x = Random.Range(-0.2f, 2.2f);

        bool isPositiveZ = Random.value > 0.5f;
        float z = isPositiveZ ? Random.Range(1f, 2.5f) : Random.Range(-2.5f, -1f);

        Vector3 localSpawnPosition = new Vector3(x, 0f, z);
        Vector3 worldSpawnPosition = transform.TransformPoint(localSpawnPosition);

        Quaternion localRotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
        Quaternion worldRotation = transform.rotation * localRotation;

        GameObject spawned = Instantiate(prefab, worldSpawnPosition, worldRotation);
        spawned.transform.SetParent(transform); // Ustawienie jako dziecko
    }
}
