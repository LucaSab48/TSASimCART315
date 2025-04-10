using UnityEngine;

public class SuitcaseMovement : MonoBehaviour
{
    public float moveSpeed = 2f;

    private Vector3 targetPosition;
    private bool isMoving = false;
    public bool isLocked = false;

    [Header("Locking")]
    public bool useLockPosition = true;
    public float lockPositionX = -3f;

    void Update()
    {
        if (isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            // ✅ Lock based on custom position
            if (!isLocked && useLockPosition && Mathf.Abs(transform.position.x - lockPositionX) < 0.01f)
            {
                isLocked = true;
                isMoving = false;
                Debug.Log("Suitcase locked at custom lock position: " + lockPositionX);
            }

            // ✅ Lock when reaching final destination (if lock-by-position is off)
            if (!isLocked && !useLockPosition && transform.position == targetPosition)
            {
                isLocked = true;
                isMoving = false;
                Debug.Log("Suitcase locked at target position.");
            }
        }
    }

    public void MoveSuitcase(Vector3 newTargetPosition)
    {
        if (isLocked) return;

        targetPosition = newTargetPosition;
        isMoving = true;
    }

    public void AutoMove(Vector3 newTargetPosition)
    {
        isLocked = false;
        MoveSuitcase(newTargetPosition);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector3 lockLineStart = new Vector3(lockPositionX, transform.position.y - 1, transform.position.z);
        Vector3 lockLineEnd = new Vector3(lockPositionX, transform.position.y + 1, transform.position.z);
        Gizmos.DrawLine(lockLineStart, lockLineEnd);
    }
}