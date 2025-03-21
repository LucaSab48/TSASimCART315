using UnityEngine;

public class SuitcaseManager : MonoBehaviour
{
    public GameObject[] suitcasePrefabs;
    public Transform spawnPoint;
    public Transform exitPoint;
    public static GameObject currentSuitcase;

    public bool preventDuplicateSpawn = true;
    private int lastSpawnedIndex = -1;

    void Start()
    {
        SpawnSuitcase();
    }

    public void SpawnSuitcase()
    {
        if (currentSuitcase != null)
        {
            Destroy(currentSuitcase);
        }

        int randomIndex;
        
        if (preventDuplicateSpawn && suitcasePrefabs.Length > 1)
        {
            do
            {
                randomIndex = Random.Range(0, suitcasePrefabs.Length);
            } while (randomIndex == lastSpawnedIndex);
        }
        else
        {
            randomIndex = Random.Range(0, suitcasePrefabs.Length);
        }

        lastSpawnedIndex = randomIndex;
        
        currentSuitcase = Instantiate(suitcasePrefabs[randomIndex], spawnPoint.position, Quaternion.identity);
        
        bool isDangerous = Random.value < 0.2f;
        currentSuitcase.tag = isDangerous ? "DangerousSuitcase" : "SafeSuitcase";
    }

    private void Update()
    {
        if (currentSuitcase != null && currentSuitcase.transform.position.x > exitPoint.position.x)
        {
            SpawnSuitcase();
        }
    }
}