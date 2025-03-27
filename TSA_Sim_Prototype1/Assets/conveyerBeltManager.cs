using UnityEngine;

public class ConveyerBeltButton : MonoBehaviour
{
    public bool moveLeft;
    public float distanceToMove = 5f;

    private void OnMouseDown()
    {
        if (SuitcaseManager.currentSuitcase != null)
        {
            SuitcaseMovement suitcaseMovement = SuitcaseManager.currentSuitcase.GetComponent<SuitcaseMovement>();
            Vector3 targetPosition = SuitcaseManager.currentSuitcase.transform.position + (moveLeft ? Vector3.left : Vector3.right) * distanceToMove;
            suitcaseMovement.MoveSuitcase(targetPosition);
        }
    }
}