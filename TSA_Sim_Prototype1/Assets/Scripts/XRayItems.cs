using UnityEngine;

public class XRayItem : MonoBehaviour
{
    private Color _originalColor;
    private SpriteRenderer _spriteRenderer;
    public bool IsMarked { get; private set; } = false;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _originalColor = _spriteRenderer.color;
    }

    private void OnMouseEnter()
    {
        _spriteRenderer.color = IsMarked ? Color.red : Color.white;
    }

    private void OnMouseExit()
    {
        _spriteRenderer.color = IsMarked ? Color.red : _originalColor; 
    }

    private void OnMouseDown()
    {
        IsMarked = !IsMarked;
        _spriteRenderer.color = IsMarked ? Color.red : _originalColor;
    }
}