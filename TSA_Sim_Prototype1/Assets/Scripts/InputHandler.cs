using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    private Camera _mainCamera;

    private void Awake()
    {
        _mainCamera = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Left mouse click
        {
            Vector2 worldPosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            Collider2D hitCollider = Physics2D.OverlapPoint(worldPosition);

            if (hitCollider)
            {
                Debug.Log($"Clicked on: {hitCollider.gameObject.name}");
                buttonClickManager button = hitCollider.GetComponent<buttonClickManager>();
                if (button != null)
                {
                    button.OnClick();
                }
            }
        }
    }
}
