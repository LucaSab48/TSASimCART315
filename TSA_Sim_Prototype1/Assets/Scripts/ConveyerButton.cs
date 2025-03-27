using UnityEngine;

public class SuitcaseMovement : MonoBehaviour
{
    public bool moveLeft; // True for left button, false for right button
    public float moveSpeed = 2f;
    private bool isHolding = false;

    private void Update()
    {
        if (SuitcaseManager.currentSuitcase == null) return;

        Rigidbody2D rb = SuitcaseManager.currentSuitcase.GetComponent<Rigidbody2D>();

        if (isHolding)
        {
            float direction = moveLeft ? -1f : 1f;
            SuitcaseManager.currentSuitcase.transform.position += Vector3.right * direction * moveSpeed * Time.deltaTime;
            SuitcaseManager.isMoving = true;

            // Apply movement physics using linearVelocity (updated API)
            if (rb != null)
            {
                rb.linearVelocity = new Vector2(direction * moveSpeed, rb.linearVelocity.y);
            }
        }
        else if (!isHolding && SuitcaseManager.isMoving)
        {
            // Stop movement when the player releases the button
            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero; // Stop movement using linearVelocity
                rb.angularVelocity = 0f;
            }

            SuitcaseManager.isMoving = false; // Reset movement flag
        }
        else
        {
            // If not moving and suitcase is null, ensure it stops fully
            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;
                rb.angularVelocity = 0f;
            }
        }
    }

    private void OnMouseDown()
    {
        isHolding = true;
        Debug.Log("Mouse is down");
    }

    private void OnMouseUp()
    {
        isHolding = false;
    }
}