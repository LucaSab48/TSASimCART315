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
                    planeCrash.gameObject.SetActive(true); // enable if it was hidden
                    planeCrash.StartMoving(new Vector2(-2f, -6f)); // move diagonally down/right

                    // Call EndGame when the plane crash finishes moving
                    StartCoroutine(WaitForPlaneCrashAndEndGame());
                }
                else
                {
                    Debug.Log("All passengers were safe. Plane flying normally.");
                    planeSafe.gameObject.SetActive(true);
                    planeSafe.StartMoving(new Vector2(-2f, 6f)); // move straight right or upward

                    // Call EndGame when the plane finishes flying
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
        // Wait for the plane crash animation to finish
        yield return new WaitForSeconds(5f); // Adjust to match crash animation time

        // Trigger game end for failure (crash)
        gameManager.EndGame(false);
    }

    // Coroutine to wait for the plane flight to finish and then end the game
    private IEnumerator WaitForPlaneSafeAndEndGame()
    {
        // Wait for the plane to finish flying (assuming it's a set time or you can check animation)
        yield return new WaitForSeconds(5f); // Adjust to match flight animation time

        // Trigger game end for success (safe flight)
        gameManager.EndGame(true);
    }

}
