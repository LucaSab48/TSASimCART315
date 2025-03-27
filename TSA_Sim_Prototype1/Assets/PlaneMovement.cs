using UnityEngine;

public class PlaneMovement : MonoBehaviour
{
    public Vector2 destination; 
    public float moveSpeed = 2f; 
    private bool isMoving = false; 
    private Vector2 startPosition; 
    private float journeyLength; 
    private float startTime;

    private void Start()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        if (isMoving)
        {
            MovePlane();
        }
    }

    public void StartMoving(Vector2 newDestination)
    {
        destination = newDestination;
        isMoving = true;
        startTime = Time.time;
        
        journeyLength = Vector2.Distance(startPosition, destination);
    }

    private void MovePlane()
    {
        float distanceCovered = (Time.time - startTime) * moveSpeed;
        float fractionOfJourney = distanceCovered / journeyLength;

        transform.position = Vector2.Lerp(startPosition, destination, fractionOfJourney);

        if (fractionOfJourney >= 1)
        {
            isMoving = false;
        }
    }
}