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

    public List<GameObject> passengers; // Assign in Unity Inspector
    private Dictionary<GameObject, bool> passengerSuitcaseMap = new Dictionary<GameObject, bool>();

    private GameObject activePassenger; // The passenger currently being inspected
    public Transform inspectionArea; // Where the active passenger moves
    public float shiftAmount = 1.5f; // Passenger queue shift distance

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

        // Disable all passengers' colliders except the first one
        for (int i = 0; i < passengers.Count; i++)
        {
            passengers[i].GetComponent<Collider2D>().enabled = false;
        }

        if (passengers.Count > 0)
        {
            activePassenger = passengers[0]; // First passenger is active
            activePassenger.GetComponent<Collider2D>().enabled = true;
            MovePassengerToInspection(activePassenger); // Move first passenger immediately
        }
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
        if (_myCollider2D != null)
        {
            _myCollider2D.enabled = true;
        }
    }

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


    public GameObject GetActivePassenger()
    {
        return activePassenger;
    }

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

    public void RestartGame()
    {
        currentPassengerIndex = 0;
        passengerSuitcaseMap.Clear();
        activePassenger = null;
        Debug.Log("Game Restarted");
    }

    private void MovePassengerToInspection(GameObject passenger)
    {
        if (inspectionArea != null)
        {
            StartCoroutine(MoveToPosition(passenger, inspectionArea.position, 0.5f));
        }
    }

    public void AdvanceToNextPassenger()
    {
        if (passengers.Count == 0)
        {
            Debug.Log("All passengers processed.");
            activePassenger = null;
            return;
        }

        // Ensure the previous active passenger is disabled and its collider is off
        if (activePassenger != null)
        {
            activePassenger.GetComponent<Collider2D>().enabled = false;
        }

        // Increment the passenger index to move to the next one
        currentPassengerIndex++;

        // Ensure there are still passengers left before accessing the next one
        if (currentPassengerIndex < passengers.Count)
        {
            activePassenger = passengers[currentPassengerIndex];
            activePassenger.GetComponent<Collider2D>().enabled = true; // Enable the collider of the next passenger
            MovePassengerToInspection(activePassenger); // Move first passenger to inspection

            // Now shift the rest of the passengers in line up
            ShiftWaitingLineUp();
        }
        else
        {
            activePassenger = null;
            Debug.Log("No more passengers.");
        }
    }

    private void ShiftWaitingLineUp()
    {
        Debug.Log($"Shifting passengers starting from index {currentPassengerIndex + 1}");

        for (int i = currentPassengerIndex + 1; i < passengers.Count; i++)
        {
            // Log each passenger's current position
            Debug.Log($"Shifting passenger {passengers[i].name} from {passengers[i].transform.position}");

            Vector3 newPosition = passengers[i].transform.position;
            newPosition.x += shiftAmount; // Move up in the queue (you can adjust the axis as needed)
    
            // Log the new position
            Debug.Log($"New position for {passengers[i].name}: {newPosition}");
    
            StartCoroutine(MoveToPosition(passengers[i], newPosition, 0.5f));
        }
    }

    

    private IEnumerator MoveToPosition(GameObject obj, Vector3 targetPosition, float duration)
    {
        Debug.Log($"Moving passenger {obj.name} to {targetPosition}"); // Debug log here
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

}
