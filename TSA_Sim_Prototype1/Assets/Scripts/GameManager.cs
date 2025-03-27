using UnityEngine;

public class GameManager : MonoBehaviour
{
    private SceneManager sceneManager;
    private Collider2D _myCollider2D;  // Renamed to avoid conflict with inherited member

    private void Start()
    {
        sceneManager = Object.FindFirstObjectByType<SceneManager>(); // Updated API

        if (sceneManager == null)
        {
            Debug.LogError("❌ SceneManager not found in the scene!");
        }

        _myCollider2D = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("SafeSuitcase") || other.CompareTag("DangerousSuitcase")) 
        {
            Debug.Log("🛄 Suitcase entered trigger, switching to Observation Screen...");

            // Store reference to the current suitcase
            SuitcaseManager.currentSuitcase = other.gameObject;

            // Stop movement before switching scenes
            SuitcaseManager.ResetSuitcase();

            // Switch scenes
            sceneManager.ShowObservationScreen();

            // Disable the collider
            _myCollider2D.enabled = false;
        }
    }

    public void EnableCollider()
    {
        _myCollider2D.enabled = true;
    }
}