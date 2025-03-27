using UnityEngine;

public class SuitcaseMovement : MonoBehaviour
{
    public float moveSpeed = 2f;
    private Vector3 targetPosition;
    private bool isMoving = false;

    void Update()
    {
        if (isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            if (transform.position == targetPosition)
            {
                isMoving = false;
            }
        }
    }
    
    public void MoveSuitcase(Vector3 newTargetPosition)
    {
        targetPosition = newTargetPosition;
        isMoving = true;
    }
}