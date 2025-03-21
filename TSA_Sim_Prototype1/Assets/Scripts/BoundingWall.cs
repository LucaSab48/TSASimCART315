using UnityEngine;

public class BoundingWall : MonoBehaviour
{
    public SuitcaseManager suitcaseSpawner; 

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("DangerousSuitcase") || other.CompareTag("SafeSuitcase"))
        {
            Destroy(other.gameObject);
            suitcaseSpawner.SpawnSuitcase();
        }
    }
}

