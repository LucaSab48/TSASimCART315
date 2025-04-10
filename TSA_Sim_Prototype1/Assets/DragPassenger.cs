using UnityEngine;
using System.Collections;

public class DraggablePassenger : MonoBehaviour
{
    private Vector3 startPosition;
    private bool isDragging = false;
    public Transform exitSign;
    public Transform boardSign;
    public float suitcaseReadyXThreshold = -3f; // Adjust this based on your scene layout

    void OnMouseDown()
    {
        if (GameManager.instance.GetActivePassenger() != gameObject)
            return;

        GameObject suitcase = SuitcaseManager.currentSuitcase;
        if (suitcase == null || suitcase.transform.position.x < suitcaseReadyXThreshold)
        {
            Debug.Log("Suitcase hasn't reached the inspection point yet.");
            return;
        }

        startPosition = transform.position;
        isDragging = true;
    }

    void OnMouseDrag()
    {
        if (!isDragging) return;

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(mousePosition.x, transform.position.y, transform.position.z);
    }

    void OnMouseUp()
    {
        if (!isDragging) return;

        isDragging = false;
        CheckPlacement();
    }

    void CheckPlacement()
    {
        if (transform.position.x < exitSign.position.x)
        {
            Debug.Log("Passenger removed.");
            StartCoroutine(RemovePassengerAfterExit());
        }
        else if (transform.position.x > boardSign.position.x)
        {
            Debug.Log("Passenger boarded.");
            GameManager.instance.CheckPassengerBoarding(gameObject);
        }
        else
        {
            transform.position = startPosition;
        }
    }

    private IEnumerator RemovePassengerAfterExit()
    {
        yield return new WaitForEndOfFrame();

        GameManager.instance.passengers.Remove(gameObject);
        Destroy(gameObject);

        GameManager.instance.AdvanceToNextPassenger();
    }
}