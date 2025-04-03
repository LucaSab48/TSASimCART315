using UnityEngine;

public class BoundingWall : MonoBehaviour
{
    public SuitcaseManager suitcaseSpawner;
    public GameManager gameManager;
    public PlaneMovement planeMovement;

    private int completedSuitcases = 0;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("DangerousSuitcase") || other.CompareTag("SafeSuitcase"))
        {
            Destroy(other.gameObject);
            completedSuitcases++;

            if (completedSuitcases >= gameManager.maxSuitcases)
            {
                suitcaseSpawner.StopSpawning();
                Vector2 newDestination = new Vector2(planeMovement.destination.x, planeMovement.destination.y);
                planeMovement.StartMoving(newDestination);
            }
            else
            {
                suitcaseSpawner.SpawnSuitcase();
            }
            
            if (gameManager != null)
            {
                gameManager.EnableCollider();
            }
        }
    }
}
