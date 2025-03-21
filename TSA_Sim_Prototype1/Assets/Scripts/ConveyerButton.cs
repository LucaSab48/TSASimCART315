using UnityEngine;

public class SuitcaseMovement : MonoBehaviour
{
    public bool moveLeft; // Set true for the left button, false for the right button
    public float moveSpeed = 2f;
    private bool isHolding = false;

    private void Update()
    {
        if (isHolding && SuitcaseManager.currentSuitcase != null)
        {
            float direction = moveLeft ? -1f : 1f;
            SuitcaseManager.currentSuitcase.transform.position += Vector3.right * direction * moveSpeed * Time.deltaTime;
        }
    }

    private void OnMouseDown()
    {
        isHolding = true;
    }

    private void OnMouseUp()
    {
        isHolding = false;
    }
}