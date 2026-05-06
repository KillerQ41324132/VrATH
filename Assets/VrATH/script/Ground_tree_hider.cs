using UnityEngine;

public class Ground_tree_hider : MonoBehaviour
{
    [SerializeField] GameObject tree1; // Prefab drzewa 1 do spawnów
    [SerializeField] GameObject tree2; // Prefab drzewa 2 do spawnów

    // Liczba par spawnów (po jednym obiekcie tree1 i tree2 na iteracjê)
    public int numberOfSpawns = 10;

    void Awake()
    {
        // Tworzy 'numberOfSpawns' razy po jednym obiekcie tree1 i tree2
        for (int i = 0; i < numberOfSpawns; i++)
        {
            SpawnObject(tree1);
            SpawnObject(tree2);
        }

        // Jeœli pozycja obiektu nadrzêdnego na osi X jest wiêksza ni¿ 0,
        // to wszystkie dzieci (spawnione drzewa) s¹ ukrywane (dezaktywowane)
        if (transform.position.x > 0f)
        {
            foreach (Transform child in gameObject.transform)
            {
                child.gameObject.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Jeœli obiekt z tagiem "Player" lub "Dead" wejdzie w trigger,
        // to wszystkie dzieci (spawnione drzewa) s¹ aktywowane (pokazywane)
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
        // Jeœli obiekt z tagiem "Player" lub "Dead" opuœci trigger,
        // to wszystkie dzieci (spawnione drzewa) s¹ dezaktywowane (ukrywane)
        if (other.CompareTag("Player") || other.CompareTag("Dead"))
        {
            foreach (Transform child in gameObject.transform)
            {
                child.gameObject.SetActive(false);
            }
        }
    }

    // Metoda do spawnowania pojedynczego obiektu (prefabu) w losowej pozycji i rotacji
    void SpawnObject(GameObject prefab)
    {
        // Losowa pozycja na osi Z w zakresie od -0.2 do 0.2 jednostek lokalnych
        float z = Random.Range(-1.5f, 1.5f);

        // Losowy wybór, czy pozycja na osi X bêdzie dodatnia czy ujemna
        bool isPositiveZ = Random.value > 0.5f;

        // Jeœli isPositiveZ jest true, X jest losowane z zakresu 1 do 2.5 jednostek
        // Jeœli false, X jest losowane z zakresu -2.5 do -1 jednostek
        // To zapewnia rozrzut spawnów po obu stronach osi X
        float x = isPositiveZ ? Random.Range(0.45f, 0.9f) : Random.Range(-0.45f, -0.9f);

        // Pozycja lokalna wzglêdem obiektu nadrzêdnego
        Vector3 localSpawnPosition = new Vector3(x, 0f, z);

        // Konwersja pozycji lokalnej na pozycjê w œwiecie
        Vector3 worldSpawnPosition = transform.TransformPoint(localSpawnPosition);

        // Losowa rotacja wokó³ osi Y od 0 do 360 stopni
        Quaternion localRotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);

        // Rotacja w przestrzeni œwiata uwzglêdniaj¹ca rotacjê obiektu nadrzêdnego
        Quaternion worldRotation = transform.rotation * localRotation;

        // Tworzenie instancji obiektu w œwiecie z ustalon¹ pozycj¹ i rotacj¹
        GameObject spawned = Instantiate(prefab, worldSpawnPosition, worldRotation);

        // Ustawienie obiektu jako dziecko tego obiektu (dla organizacji hierarchii)
        spawned.transform.SetParent(transform);
    }
}
