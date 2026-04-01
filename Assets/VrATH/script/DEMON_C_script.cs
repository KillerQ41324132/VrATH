using System.Collections;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class DEMON_C_script : MonoBehaviour
{
    [SerializeField] private GameObject demonPrefab; // Przypisany prefab dla demona
    [SerializeField] private GameObject demonChild;  // Przypisany GameObject jako dziecko, które ma siê pojawiæ
    [SerializeField] private EventReference BushC;
    [SerializeField] private EventReference BushCJump;
    private int stage;
    private BoxCollider[] boxColliders;
    private Vector3 spawnerPosition; // Zapamiêtana pozycja spawnera

    private void Start()
    {
        Collider[] colliders = GetComponents<Collider>();

        if (colliders.Length == 0) return;

        // Zak³adamy, ¿e ostatni dodany collider to ostatni w tablicy
        BoxCollider lastBoxCollider = colliders[colliders.Length - 1] as BoxCollider;

        if (lastBoxCollider != null)
        {
            // Obliczamy globaln¹ pozycjê œrodka collidiera
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
            }
        }
    }

    // Metody dla poszczególnych etapów
    void Stage1()
    {
        RuntimeManager.PlayOneShot(BushC, spawnerPosition);//MIJSCE DLA AGNIESI
        Debug.Log("Entered Stage C1");
        // Dodatkowe dzia³ania dla Stage 1
    }
    void Stage2()
    {
        RuntimeManager.PlayOneShot(BushCJump, spawnerPosition);//MIJSCE DLA AGNIESI + MICHASIA UWU
        Debug.Log("Entered Stage C3");

        // Stworzenie demona w zapisanej pozycji
        if (demonPrefab != null && spawnerPosition != null)
        {
            GameObject demon = Instantiate(demonPrefab, spawnerPosition, Quaternion.identity);
            demon.layer = LayerMask.NameToLayer("Cint"); //interakcja na jakiej warstwie
        }
        // Dodatkowe dzia³ania dla Stage 3
    }
}
