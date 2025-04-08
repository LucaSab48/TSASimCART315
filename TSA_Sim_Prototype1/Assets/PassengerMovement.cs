using UnityEngine;
using System.Collections;

public class PassengerMovement : MonoBehaviour
{
    public Transform inspectionArea;

    // Move the passenger to the inspection area
    public void MovePassengerToInspection()
    {
        if (inspectionArea != null)
        {
            StartCoroutine(MoveToPosition(inspectionArea.position, 0.5f));
        }
    }

    // Coroutine to smoothly move a passenger to a new position
    private IEnumerator MoveToPosition(Vector3 targetPosition, float duration)
    {
        Vector3 startPos = transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(startPos, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;
    }
}