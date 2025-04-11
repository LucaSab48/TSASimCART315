using UnityEngine;

public class FullScreenSprite : MonoBehaviour
{
    [Header("Size Settings")]
    [Range(0.1f, 1.5f)]
    public float sizeMultiplier = 0.9f;
    [Tooltip("Should the sprite maintain its original aspect ratio?")]
    public bool maintainAspectRatio = true;

    [Header("Appearance")]
    public Color backgroundColor = Color.black;
    [Tooltip("Hover scale multiplier")]
    public float hoverScale = 1.05f; // Added hover scale parameter

    private bool isFullScreen = false;
    private Vector3 originalScale;
    private Vector3 originalPosition;
    private int originalSortingOrder;
    private Camera mainCamera;
    private GameObject backgroundPanel;
    private Vector3 baseScale; // Stores the original scale before hover

    void Start()
    {
        mainCamera = Camera.main;
        originalScale = transform.localScale;
        baseScale = originalScale; // Initialize base scale
        originalPosition = transform.position;
        originalSortingOrder = GetComponent<SpriteRenderer>().sortingOrder;

        // Ensure there's a collider for click detection
        if (GetComponent<Collider2D>() == null)
        {
            gameObject.AddComponent<BoxCollider2D>();
        }

        // Create background panel (initially hidden)
        CreateBackgroundPanel();
    }

    void OnMouseEnter()
    {
        if (!isFullScreen)
        {
            transform.localScale = baseScale * hoverScale;
        }
    }

    void OnMouseExit()
    {
        if (!isFullScreen)
        {
            transform.localScale = baseScale;
        }
    }

    void OnMouseDown()
    {
        ToggleFullScreen();
    }

    void ToggleFullScreen()
    {
        isFullScreen = !isFullScreen;
        
        if (isFullScreen)
        {
            // Save current state
            baseScale = transform.localScale; // Update base scale
            originalScale = baseScale; // Keep original scale updated
            originalPosition = transform.position;
            
            // Make full screen
            MakeSpriteFillScreen();
            
            // Bring to front
            GetComponent<SpriteRenderer>().sortingOrder = 999;
            
            // Show background
            if (backgroundPanel != null)
                backgroundPanel.SetActive(true);
        }
        else
        {
            // Restore original state
            transform.localScale = baseScale; // Use baseScale instead of originalScale
            transform.position = originalPosition;
            GetComponent<SpriteRenderer>().sortingOrder = originalSortingOrder;
            
            // Hide background
            if (backgroundPanel != null)
                backgroundPanel.SetActive(false);
        }
    }

    void MakeSpriteFillScreen()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr.sprite == null) return;

        float spriteWidth = sr.sprite.bounds.size.x;
        float spriteHeight = sr.sprite.bounds.size.y;
        float screenHeight = mainCamera.orthographicSize * 2;
        float screenWidth = screenHeight * mainCamera.aspect;

        if (maintainAspectRatio)
        {
            // Calculate scale to fit while maintaining aspect ratio
            float scaleX = screenWidth / spriteWidth;
            float scaleY = screenHeight / spriteHeight;
            float scale = Mathf.Min(scaleX, scaleY) * sizeMultiplier;
            transform.localScale = new Vector3(scale, scale, 1);
        }
        else
        {
            // Stretch to fill exactly
            transform.localScale = new Vector3(
                (screenWidth / spriteWidth) * sizeMultiplier,
                (screenHeight / spriteHeight) * sizeMultiplier,
                1
            );
        }
        
        // Center on screen
        transform.position = mainCamera.transform.position + new Vector3(0, 0, 10);
    }

    void CreateBackgroundPanel()
    {
        // Create a semi-transparent background
        backgroundPanel = new GameObject("FullScreenBackground");
        SpriteRenderer bgRenderer = backgroundPanel.AddComponent<SpriteRenderer>();
        bgRenderer.color = backgroundColor;
        bgRenderer.sprite = Sprite.Create(
            new Texture2D(1, 1),
            new Rect(0, 0, 1, 1),
            new Vector2(0.5f, 0.5f)
        );
        
        // Scale to fill screen
        backgroundPanel.transform.localScale = new Vector3(
            mainCamera.orthographicSize * 2 * mainCamera.aspect,
            mainCamera.orthographicSize * 2,
            1
        );
        
        // Position behind everything
        backgroundPanel.transform.position = mainCamera.transform.position + new Vector3(0, 0, 11);
        bgRenderer.sortingOrder = 998;
        
        // Start hidden
        backgroundPanel.SetActive(false);
    }

    // Optional: Add keyboard support
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && isFullScreen)
        {
            ToggleFullScreen();
        }
    }
}
