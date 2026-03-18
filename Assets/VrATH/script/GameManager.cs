using System.Collections.Generic;
using System;
using UnityEngine;
using System.Linq;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] private GameObject canvas;
    private int[] sequence;
    private Dictionary<char, int[]> characters = new Dictionary<char, int[]>
    {
        {'0', new int[] {1}},
        {'A', new int[] {1,0,1,0,0,1}},
        {'B', new int[] {1,0,0,1,1}},
        {'C', new int[] {1,0,0,1}},
        {'D', new int[] {1,0,1}},
        {'E', new int[] {1,1}}
    };

    [SerializeField] private GameObject characterA;
    [SerializeField] private GameObject characterB;
    [SerializeField] private GameObject characterC;
    [SerializeField] private GameObject characterD;
    [SerializeField] private GameObject characterE;
    [SerializeField] private GameObject vibeDoor;
    [SerializeField] private GameObject characterVibe;

    private List<GameObject> instantiatedObjects = new List<GameObject>();

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        sequence = new int[19];
        Placing();
    }

    public void Placing()
    {
        PlaceCharacters();
        PrintSequence();
        PlaceVibeInEmptyCells();
    }

    void PlaceCharacters()
    {
        System.Random rand = new System.Random();
        bool allPlacedSuccessfully = false;

        while (!allPlacedSuccessfully)
        {
            Array.Clear(sequence, 0, sequence.Length);

            foreach (var obj in instantiatedObjects)
                Destroy(obj);
            instantiatedObjects.Clear();

            Debug.Log("Reset ułożenia");

            allPlacedSuccessfully = true;

            int zeroPos = rand.Next(0, 3);
            if (!PlaceCharacter('0', zeroPos, rand))
            {
                allPlacedSuccessfully = false;
                continue;
            }

            List<char> requiredCharacters = new List<char> { 'A', 'B', 'C', 'D', 'E' };

            for (int i = requiredCharacters.Count - 1; i > 0; i--)
            {
                int j = rand.Next(i + 1);
                (requiredCharacters[i], requiredCharacters[j]) = (requiredCharacters[j], requiredCharacters[i]);
            }

            foreach (char ch in requiredCharacters)
            {
                bool placed = false;
                List<int> positions = Enumerable.Range(0, sequence.Length).OrderBy(_ => rand.Next()).ToList();

                foreach (int pos in positions)
                {
                    if (PlaceCharacter(ch, pos, rand))
                    {
                        placed = true;
                        break;
                    }
                }

                if (!placed)
                {
                    allPlacedSuccessfully = false;
                    Debug.Log("reset");
                    break;
                }
            }
        }
    }

    bool PlaceCharacter(char character, int position, System.Random rand)
    {
        int[] pattern = characters[character];

        if (position + pattern.Length > sequence.Length)
            return false;

        // kolizja
        for (int i = 0; i < pattern.Length; i++)
        {
            if (sequence[position + i] == 1 && pattern[i] == 1)
                return false;
        }

        // zapis do tablicy
        for (int i = 0; i < pattern.Length; i++)
        {
            if (pattern[i] == 1)
                sequence[position + i] = 1;
        }

        // 🔥 KLUCZ: oś Z + mnożenie ×4
        Vector3 spawnPos = new Vector3(0, 0, position * 4);

        GameObject newCharacter = null;

        if (character == 'A') newCharacter = Instantiate(characterA, spawnPos, Quaternion.identity);
        else if (character == 'B') newCharacter = Instantiate(characterB, spawnPos, Quaternion.identity);
        else if (character == 'C') newCharacter = Instantiate(characterC, spawnPos, Quaternion.identity);
        else if (character == 'D') newCharacter = Instantiate(characterD, spawnPos, Quaternion.identity);
        else if (character == 'E') newCharacter = Instantiate(characterE, spawnPos, Quaternion.identity);
        else if (character == '0') newCharacter = Instantiate(vibeDoor, spawnPos, Quaternion.identity);

        if (newCharacter != null)
        {
            instantiatedObjects.Add(newCharacter);
        }

        Debug.Log($"Postać {character} dodana do pozycji {position}");
        return true;
    }

    void PrintSequence()
    {
        string result = "Ciąg wynikowy: " + string.Join(" ", sequence);
        Debug.Log(result);
    }

    void PlaceVibeInEmptyCells()
    {
        for (int i = 0; i < sequence.Length; i++)
        {
            if (sequence[i] == 0)
            {
                // 🔥 też oś Z + ×4
                Vector3 spawnPos = new Vector3(0, 0, i * 4);

                GameObject vibeCharacter = Instantiate(characterVibe, spawnPos, Quaternion.identity);
                instantiatedObjects.Add(vibeCharacter);
            }
        }
    }
}