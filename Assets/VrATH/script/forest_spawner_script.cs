using UnityEngine;
using System.Collections.Generic;

public class forest_spawner_script : MonoBehaviour
{
    //STARE U¯YWANIE SPAWNERU DRZEW, OBECNIE JEST TO W POSZCZEGÓLNYCH PREFFABACH
    public GameObject treePrefab;
    public GameObject treePrefab2; // drugi prefab drzewa

    public GameObject bushPrefab;
    public int numberOfTrees = 200;
    private List<Vector3> spawnedTreePositions = new List<Vector3>();

    void Start()
    {
        SpawnTrees();
        SpawnBushes();
    }

    void SpawnTrees()
    {
        int spawned = 0;
        int maxAttempts = 1000;
        int attempts = 0;

        while (spawned < numberOfTrees && attempts < maxAttempts)
        {
            attempts++;

            float z = Random.Range(-4f, 80f);
            float x = Random.Range(-5f, 5f);

            // Zakazane strefy
            if (z >= -1f && z <= 81f && x >= -1.5f && x <= 1.5f)
                continue;

            Vector3 spawnPos = new Vector3(x, 0f, z);

            // Odleg³oœæ minimalna 1
            bool tooClose = false;
            foreach (Vector3 pos in spawnedTreePositions)
            {
                if (Vector3.Distance(pos, spawnPos) < .3f)
                {
                    tooClose = true;
                    break;
                }
            }
            if (tooClose)
                continue;

            // Skala drzewa zale¿na od odleg³oœci od osi Z
            float normalizedZ = Mathf.InverseLerp(0f, 10f, Mathf.Abs(z));
            float scale = Mathf.Lerp(0.1f, 0.4f, normalizedZ);

            // Losowy obrót
            Quaternion rotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);

            // Wybór prefabu drzewa 50/50
            GameObject selectedTreePrefab = Random.value < 0.5f ? treePrefab : treePrefab2;

            GameObject tree = Instantiate(selectedTreePrefab, spawnPos, rotation);
            tree.transform.localScale = Vector3.one * scale;
            tree.name = "tree";

            spawnedTreePositions.Add(spawnPos);
            spawned++;
        }
    }


    void SpawnBushes()
    {
        for (int x = 0; x <= 30; x++)
        {
            foreach (float z in new float[] { 1f, -1f })
            {
                // Dodaj losowy offset 0.4 na X i Z
                float offsetZ = Random.Range(-0.02f, 0.02f);
                float offsetX = Random.Range(-0.2f, 0.2f);

                Vector3 spawnPos = new Vector3(x + offsetX, 0f, z + offsetZ);

                Quaternion rotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);

                GameObject bush = Instantiate(bushPrefab, spawnPos, rotation);

                // Losowa skala busha od 1.0 do 1.2
                float bushScale = Random.Range(.01f, .03f);
                bush.transform.localScale = Vector3.one * bushScale;

                bush.name = "bush";
            }
        }
    }
}