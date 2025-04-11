using System.Collections;
using UnityEngine;

public class BoundingWall : MonoBehaviour
{
    public SuitcaseManager suitcaseSpawner;
    public GameManager gameManager;
    public PlaneMovement planeCrash;
    public PlaneMovement planeSafe;

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

                if (gameManager.dangerousPassengerBoarded)
                {
                    Debug.Log("A dangerous passenger boarded. Crashing the plane!");
                    planeCrash.gameObject.SetActive(true); 
                    planeCrash.StartMoving(new Vector2(-2f, -6f));
                    
                    StartCoroutine(WaitForPlaneCrashAndEndGame());
                }
                else
                {
                    Debug.Log("All passengers were safe. Plane flying normally.");
                    planeSafe.gameObject.SetActive(true);
                    planeSafe.StartMoving(new Vector2(-2f, 6f));
                    
                    StartCoroutine(WaitForPlaneSafeAndEndGame());
                }
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

    
    private IEnumerator WaitForPlaneCrashAndEndGame()
    {
        yield return new WaitForSeconds(4f); 
        
        gameManager.EndGame(false);
    }
    
    private IEnumerator WaitForPlaneSafeAndEndGame()
    {
 
        yield return new WaitForSeconds(4f);
        
        gameManager.EndGame(true);
    }

}
