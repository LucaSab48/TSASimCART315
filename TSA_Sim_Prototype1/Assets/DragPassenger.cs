using UnityEngine;

public class DraggablePassenger : MonoBehaviour
{
    private Vector3 startPosition;
    private bool isDragging = false;
    public Transform exitSign;
    public Transform boardSign;

    void OnMouseDown()
    {
        if (GameManager.instance.GetActivePassenger() != gameObject)
            return; // Prevents interaction with non-active passengers

        startPosition = transform.position;
        isDragging = true;
    }

    void OnMouseDrag()
    {
        if (isDragging)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(mousePosition.x, transform.position.y, transform.position.z);
        }
    }

    void OnMouseUp()
    {
        isDragging = false;
        CheckPlacement();
    }

    void CheckPlacement()
    {
        if (transform.position.x < exitSign.position.x) // Rejected
        {
            Debug.Log("Passenger removed.");
            Destroy(gameObject);
            GameManager.instance.AdvanceToNextPassenger();
        }
        else if (transform.position.x > boardSign.position.x) // Approved
        {
            Debug.Log("Passenger boarded.");
            GameManager.instance.CheckPassengerBoarding(gameObject);
        }
        else
        {
            transform.position = startPosition; // Snap back if not properly placed
        }
    }
}