using UnityEngine;

public class BoundingWall : MonoBehaviour
{
    public SuitcaseManager suitcaseSpawner; 
    public GameManager gameManager; // Reference to the GameManager

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("DangerousSuitcase") || other.CompareTag("SafeSuitcase"))
        {
            // Destroy the suitcase and spawn a new one
            Destroy(other.gameObject);
            suitcaseSpawner.SpawnSuitcase();

            // Re-enable the collider in GameManager
            if (gameManager != null)
            {
                gameManager.EnableCollider();
            }
        }
    }
}