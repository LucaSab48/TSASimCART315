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

    public List<GameObject> passengers;
    private Dictionary<GameObject, bool> passengerSuitcaseMap = new Dictionary<GameObject, bool>();

    private GameObject activePassenger;
    public Transform inspectionArea;
    public float shiftAmount = 1.5f;

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

        for (int i = 0; i < passengers.Count; i++)
        {
            passengers[i].GetComponent<Collider2D>().enabled = false;
        }

        if (passengers.Count > 0)
        {
            activePassenger = passengers[0];
            activePassenger.GetComponent<Collider2D>().enabled = true;

            PassengerMovement activeMovement = activePassenger.GetComponent<PassengerMovement>();
            activeMovement.MovePassengerToInspection();

            StartCoroutine(ShiftWaitingLineUp(1)); // Shift passengers after the first
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

        // Start a coroutine to remove the passenger safely at the end of the frame
        StartCoroutine(RemovePassengerAfterFrame(passenger));
    }

// Coroutine for safe removal
    private IEnumerator RemovePassengerAfterFrame(GameObject passenger)
    {
        yield return new WaitForEndOfFrame(); // Ensures frame logic (like shifting) completes

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

    public void AdvanceToNextPassenger()
    {
        if (passengers.Count == 0)
        {
            Debug.Log("All passengers processed.");
            activePassenger = null;
            return;
        }

        if (activePassenger != null)
        {
            activePassenger.GetComponent<Collider2D>().enabled = false;
        }

        if (currentPassengerIndex < passengers.Count)
        {
            activePassenger = passengers[currentPassengerIndex];
            activePassenger.GetComponent<Collider2D>().enabled = true;

            PassengerMovement activeMovement = activePassenger.GetComponent<PassengerMovement>();
            activeMovement.MovePassengerToInspection();

            // Only shift others if there are more passengers behind the current one
            if (currentPassengerIndex + 1 < passengers.Count)
            {
                StartCoroutine(ShiftWaitingLineUp(currentPassengerIndex + 1));
            }

            currentPassengerIndex++; // Advance the index after processing
        }
        else
        {
            activePassenger = null;
            Debug.Log("No more passengers.");
        }
    }



    private IEnumerator ShiftWaitingLineUp(int startIndex)
    {
        Debug.Log($"Shifting passengers starting from index {startIndex}");

        for (int i = startIndex; i < passengers.Count; i++)
        {
            GameObject passenger = passengers[i];
            Vector3 newPosition = passenger.transform.position;
            newPosition.x += shiftAmount;

            yield return StartCoroutine(MoveToPosition(passenger, newPosition, 0.5f));
        }
    }

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

    public void EnableCollider()
    {
        if (_myCollider2D != null)
        {
            _myCollider2D.enabled = true;
        }
    }
}
