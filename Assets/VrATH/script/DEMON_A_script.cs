using UnityEngine;
using FMODUnity;

public class DEMON_A_script : MonoBehaviour
{
    [SerializeField] private GameObject demonPrefab; // Przypisany prefab dla demona
    [SerializeField] private GameObject demonChild;  // Przypisany GameObject jako dziecko, kt�re ma si� pojawi�
    [SerializeField] private EventReference BushA;
    [SerializeField] private EventReference BushA2;
    [SerializeField] private EventReference BushAJump;
    private int stage;
    private BoxCollider[] boxColliders;
    private Vector3 spawnerPosition; // Zapami�tana pozycja spawnera

    private void Start()
    {
        Collider[] colliders = GetComponents<Collider>();

        if (colliders.Length == 0) return;

        // Zak�adamy, �e ostatni dodany collider to ostatni w tablicy
        BoxCollider lastBoxCollider = colliders[colliders.Length - 1] as BoxCollider;

        if (lastBoxCollider != null)
        {
            // Obliczamy globaln� pozycj� �rodka collidiera
            Vector3 worldCenter = lastBoxCollider.transform.TransformPoint(lastBoxCollider.center);
            // prawo/lewo
            float offsetX = Random.Range(0f, 1f) > 0.5f ? 20f : -20f;

            spawnerPosition = worldCenter + new Vector3(offsetX, 0, 0);

            if (demonChild != null)
            {
                Instantiate(demonChild, spawnerPosition, Quaternion.identity, transform);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            stage++;
            switch (stage)
            {
                case 1:
                    Stage1();
                    break;
                case 2:
                    Stage2();
                    break;
                case 3:
                    Stage3();
                    break;
            }
        }
    }

    // Metody dla poszczeg�lnych etap�w
    void Stage1()
    {
        RuntimeManager.PlayOneShot(BushA, spawnerPosition);//MIJSCE DLA AGNIESI
        Debug.Log("Entered Stage A1");
        // Dodatkowe dzia�ania dla Stage 1
    }

    void Stage2()
    {
        RuntimeManager.PlayOneShot(BushA2, spawnerPosition);//MIJSCE DLA AGNIESI
        Debug.Log("Entered Stage A2");
        // Dodatkowe dzia�ania dla Stage 2
    }

    void Stage3()
    {
        RuntimeManager.PlayOneShot(BushAJump, spawnerPosition);//MIJSCE DLA AGNIESI + MICHASIA UWU
        Debug.Log("Entered Stage A3");

        // Stworzenie demona w zapisanej pozycji
        if (demonPrefab != null && spawnerPosition != null)
        {
            GameObject demon = Instantiate(demonPrefab, spawnerPosition, Quaternion.identity);
            demon.layer = LayerMask.NameToLayer("Fint"); //interakcja na jakiej warstwie //Fint bo Aint nie wsp�pracowa�o
        }
        // Dodatkowe dzia�ania dla Stage 3
    }
}
