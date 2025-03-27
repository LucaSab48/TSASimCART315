using UnityEngine;

public class SuitcaseManager : MonoBehaviour
{
    public static SuitcaseManager instance;
    public GameObject[] suitcasePrefabs;
    public Transform spawnPoint;
    public static GameObject currentSuitcase;
    public static bool isMoving = false;

    public bool preventDuplicateSpawn = true;
    private int lastSpawnedIndex = -1;

    private bool canSpawn = true;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if (canSpawn)
        {
            SpawnSuitcase();
        }
    }

    public void SpawnSuitcase()
    {
        if (!canSpawn)
            return;

        if (currentSuitcase != null)
        {
            Destroy(currentSuitcase);
            currentSuitcase = null;
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
        currentSuitcase = Instantiate(suitcasePrefabs[randomIndex], spawnPoint.position, Quaternion.identity, spawnPoint.parent);
        
        bool isDangerous = Random.value < 0.2f;
        currentSuitcase.tag = isDangerous ? "DangerousSuitcase" : "SafeSuitcase";
        
        isMoving = false;
        
        Rigidbody2D rb = currentSuitcase.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }
    }

    public static void ResetSuitcase()
    {
        if (currentSuitcase != null)
        {
            Rigidbody2D rb = currentSuitcase.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;
                rb.angularVelocity = 0f;
            }

            isMoving = false;
        }
    }

    public void StopSpawning()
    {
        canSpawn = false;
    }
}
