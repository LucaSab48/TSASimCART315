using UnityEngine;

public class SuitcaseManager : MonoBehaviour
{
    public GameObject[] suitcasePrefabs;
    public Transform spawnPoint;
    public static GameObject currentSuitcase;
    public static bool isMoving = false; // Track movement state

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
            currentSuitcase = null; // Ensure no lingering reference
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

        // Reset movement state when a new suitcase spawns
        isMoving = false;

        // Ensure Rigidbody2D is set up correctly
        Rigidbody2D rb = currentSuitcase.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero; // Reset linearVelocity instead of velocity
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
                rb.linearVelocity = Vector2.zero; // Reset linearVelocity
                rb.angularVelocity = 0f;
            }

            isMoving = false; // Stop movement when switching back
        }
    }
}
