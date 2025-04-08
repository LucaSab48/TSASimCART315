using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }

    private SceneManager sceneManager;
    private Collider2D _myCollider2D;

    public int maxSuitcases = 5;
    private int currentPassengerIndex = 0;

    public List<GameObject> passengers; // List of passengers
    private Dictionary<GameObject, bool> passengerSuitcaseMap = new Dictionary<GameObject, bool>();

    private GameObject activePassenger; // The currently active passenger for inspection
    public Transform inspectionArea; // The area where the passenger moves to for inspection
    public float shiftAmount = 1.5f; // How much to shift passengers in the queue

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        sceneManager = Object.FindFirstObjectByType<SceneManager>();
        _myCollider2D = GetComponent<Collider2D>();

        // Disable all passengers' colliders except for the first one
        for (int i = 0; i < passengers.Count; i++)
        {
            passengers[i].GetComponent<Collider2D>().enabled = false;
        }

        // Set the first passenger as the active one
        if (passengers.Count > 0)
        {
            activePassenger = passengers[0]; // First passenger is active
            activePassenger.GetComponent<Collider2D>().enabled = true;

            // Move the active passenger to inspection area
            PassengerMovement activeMovement = activePassenger.GetComponent<PassengerMovement>();
            activeMovement.MovePassengerToInspection();
        }
    }

    // Process when a passenger's suitcase is detected
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

    // Method to assign a suitcase to the current passenger
    public void AssignSuitcaseToPassenger(bool isDangerous)
    {
        if (currentPassengerIndex < passengers.Count)
        {
            GameObject passenger = passengers[currentPassengerIndex];
            passengerSuitcaseMap[passenger] = isDangerous;

            Debug.Log($"Assigned suitcase to passenger {passenger.name}. Dangerous: {isDangerous}");
        }
        else
        {
            Debug.LogWarning("No more passengers available!");
        }
    }

    // Retrieve the currently active passenger
    public GameObject GetActivePassenger()
    {
        return activePassenger;
    }

    // Process the boarding of a passenger
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

        passengers.Remove(passenger);
        Destroy(passenger);

        AdvanceToNextPassenger();
    }

    private void TriggerFailState()
    {
        Debug.Log("Game Over! Restarting...");
    }

    // Restart the game
    public void RestartGame()
    {
        currentPassengerIndex = 0;
        passengerSuitcaseMap.Clear();
        activePassenger = null;
        Debug.Log("Game Restarted");
    }

    // Advance to the next passenger in the queue
    public void AdvanceToNextPassenger()
    {
        if (passengers.Count == 0)
        {
            Debug.Log("All passengers processed.");
            activePassenger = null;
            return;
        }

        // Disable the collider of the current active passenger
        if (activePassenger != null)
        {
            activePassenger.GetComponent<Collider2D>().enabled = false;
        }

        currentPassengerIndex++;

        if (currentPassengerIndex < passengers.Count)
        {
            activePassenger = passengers[currentPassengerIndex];
            activePassenger.GetComponent<Collider2D>().enabled = true;

            // Move the next active passenger
            PassengerMovement activeMovement = activePassenger.GetComponent<PassengerMovement>();
            activeMovement.MovePassengerToInspection();

            // Shift all passengers after the current one
            StartCoroutine(ShiftWaitingLineUp());
        }
        else
        {
            activePassenger = null;
            Debug.Log("No more passengers.");
        }
    }

    // Shift passengers forward in the queue (move their positions)
    private IEnumerator ShiftWaitingLineUp()
    {
        Debug.Log($"Shifting passengers starting from index {currentPassengerIndex + 1}");

        for (int i = currentPassengerIndex + 1; i < passengers.Count; i++)
        {
            // Get each passenger's current position and shift them
            GameObject passenger = passengers[i];
            Vector3 newPosition = passenger.transform.position;
            newPosition.x += shiftAmount;

            yield return StartCoroutine(MoveToPosition(passenger, newPosition, 0.5f));
        }
    }

    // Coroutine to smoothly move a passenger to a new position
    private IEnumerator MoveToPosition(GameObject obj, Vector3 targetPosition, float duration)
    {
        Vector3 startPos = obj.transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            obj.transform.position = Vector3.Lerp(startPos, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        obj.transform.position = targetPosition;
    }

    // Enable the collider of the GameManager's collider (if applicable)
    public void EnableCollider()
    {
        if (_myCollider2D != null)
        {
            _myCollider2D.enabled = true;
        }
    }
}
