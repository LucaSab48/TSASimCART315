using UnityEngine;

public class DraggablePassenger : MonoBehaviour
{
    private Vector3 startPosition;
    private bool isDragging = false;
    public Transform exitSign;
    public Transform boardSign;

    void OnMouseDown()
    {
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
        if (transform.position.x < exitSign.position.x) // Example threshold for "rejected"
        {
            Debug.Log("Passenger removed.");
        }
        else if (transform.position.x > boardSign.position.x) // Example threshold for "boarding"
        {
            Debug.Log("Passenger boarded.");
            GameManager.instance.CheckPassengerBoarding(gameObject);
        }
        else
        {
            transform.position = startPosition; // Snap back if dropped in the middle
        }
    }
}