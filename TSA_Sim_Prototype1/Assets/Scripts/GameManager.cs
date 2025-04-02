using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; // Singleton reference

    private SceneManager sceneManager;
    private Collider2D _myCollider2D;

    public int maxSuitcases = 5;
    private int currentPassengerIndex = 0;

    public List<GameObject> passengers; // Assign these in the Unity Inspector
    private Dictionary<GameObject, bool> passengerSuitcaseMap = new Dictionary<GameObject, bool>();

    private void Awake()
    {
        // Singleton setup
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Optional, if you want this object to persist across scenes
        }
        else
        {
            Destroy(gameObject); // Ensure no duplicates of GameManager
        }
    }

    private void Start()
    {
        sceneManager = Object.FindFirstObjectByType<SceneManager>();
        _myCollider2D = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("SafeSuitcase") || other.CompareTag("DangerousSuitcase"))
        {
            SuitcaseManager.currentSuitcase = other.gameObject;
            SuitcaseManager.ResetSuitcase();
            sceneManager.ShowObservationScreen();
            _myCollider2D.enabled = false;
        }
    }

    public void EnableCollider()
    {
        _myCollider2D.enabled = true;
    }

    // Assigns the suitcase's status (dangerous or safe) to a passenger
    public void AssignSuitcaseToPassenger(bool isDangerous)
    {
        if (currentPassengerIndex < passengers.Count)
        {
            GameObject passenger = passengers[currentPassengerIndex];
            passengerSuitcaseMap[passenger] = isDangerous;
            Debug.Log($"Assigned suitcase to passenger {passenger.name}. Dangerous: {isDangerous}");
            currentPassengerIndex++;
        }
        else
        {
            Debug.LogWarning("No more passengers available!");
        }
    }

    // Checks if a passenger is safe before they board the plane
    public void CheckPassengerBoarding(GameObject passenger)
    {
        if (!passengerSuitcaseMap.ContainsKey(passenger))
        {
            Debug.LogWarning($"Passenger {passenger.name} not found in the suitcase map.");
            return;
        }

        if (passengerSuitcaseMap[passenger])
        {
            Debug.Log("FAIL STATE: Dangerous passenger boarded the plane!");
            TriggerFailState();
        }
        else
        {
            Debug.Log("SAFE: Passenger boarded successfully.");
        }
    }

    // Triggers the fail condition
    private void TriggerFailState()
    {
        Debug.Log("Game Over! Restarting...");
        // You can add UI failure screens, sounds, or restart mechanics here.
        // For now, you could call RestartGame() to reset the game state.
    }

    // Optionally, add a method to restart the game if needed
    public void RestartGame()
    {
        // Reset the game state (passengers, suitcases, etc.)
        currentPassengerIndex = 0;
        passengerSuitcaseMap.Clear();
        Debug.Log("Game Restarted");
    }
}
