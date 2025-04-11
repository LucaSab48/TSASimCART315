using UnityEngine;
using System.Collections.Generic;

public class FullScreenGallery : MonoBehaviour
{
    [Header("Settings")]
    public List<GameObject> galleryObjects; // Assign all objects to cycle through
    public float swipeThreshold = 50f; // Minimum drag distance to trigger navigation
    public bool allowKeyboardNavigation = true;

    private int currentIndex = 0;
    private Vector2 touchStartPos;
    private bool isFullScreen = false;

    void Update()
    {
        if (!isFullScreen) return;

        // Touch/Mouse Navigation
        if (Input.GetMouseButtonDown(0))
        {
            touchStartPos = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            Vector2 swipeDelta = (Vector2)Input.mousePosition - touchStartPos;
            if (swipeDelta.magnitude > swipeThreshold)
            {
                if (swipeDelta.x > 0) PreviousObject();
                else NextObject();
            }
        }

        // Keyboard Navigation
        if (allowKeyboardNavigation)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow)) NextObject();
            if (Input.GetKeyDown(KeyCode.LeftArrow)) PreviousObject();
            if (Input.GetKeyDown(KeyCode.Escape)) ExitFullScreen();
        }
    }

    public void EnterFullScreen(int startIndex = 0)
    {
        currentIndex = startIndex;
        isFullScreen = true;
        ShowCurrentObject();
        ScaleToFullScreen(galleryObjects[currentIndex]);
    }

    public void ExitFullScreen()
    {
        isFullScreen = false;
        foreach (var obj in galleryObjects)
        {
            obj.SetActive(false);
        }
    }

    void ShowCurrentObject()
    {
        for (int i = 0; i < galleryObjects.Count; i++)
        {
            galleryObjects[i].SetActive(i == currentIndex);
        }
    }

    void ScaleToFullScreen(GameObject target)
    {
        SpriteRenderer sr = target.GetComponent<SpriteRenderer>();
        if (sr == null || sr.sprite == null) return;

        float screenHeight = Camera.main.orthographicSize * 2;
        float screenWidth = screenHeight * Camera.main.aspect;
        float spriteRatio = sr.sprite.bounds.size.x / sr.sprite.bounds.size.y;

        float scale = Mathf.Min(
            screenWidth / sr.sprite.bounds.size.x,
            screenHeight / sr.sprite.bounds.size.y
        );

        target.transform.localScale = new Vector3(scale, scale, 1);
        target.transform.position = Camera.main.transform.position + new Vector3(0, 0, 10);
    }

    public void NextObject()
    {
        currentIndex = (currentIndex + 1) % galleryObjects.Count;
        ShowCurrentObject();
        ScaleToFullScreen(galleryObjects[currentIndex]);
    }

    public void PreviousObject()
    {
        currentIndex = (currentIndex - 1 + galleryObjects.Count) % galleryObjects.Count;
        ShowCurrentObject();
        ScaleToFullScreen(galleryObjects[currentIndex]);
    }
}
