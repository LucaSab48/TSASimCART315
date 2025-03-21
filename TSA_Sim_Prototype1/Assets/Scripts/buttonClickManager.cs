using UnityEngine;
using UnityEngine.SceneManagement;

public class buttonClickManager : MonoBehaviour
{
    private Vector3 _ogScale;
    public string nextScene = "PassengerCheck";

    private void Awake()
    {
        _ogScale = transform.localScale;
    }

    public void OnClick()
    {
        transform.localScale = _ogScale * 0.8f;
        Invoke(nameof(ResetScale), 0.2f);
        
        if (CheckItem())
        {
            SceneManager.LoadScene(nextScene);
        }
    }
    
    private void ResetScale()
    {
        transform.localScale = _ogScale;
    }
    
    
    private bool CheckItem()
    {
        GameObject[] correctItems = GameObject.FindGameObjectsWithTag("DangerousItem");
        foreach (GameObject item in correctItems)
        {
            XRayItem xRayItem = item.GetComponent<XRayItem>();
            if (xRayItem != null && !xRayItem.IsMarked)
            {
                return false; 
            }
        }
        
        GameObject[] incorrectItems = GameObject.FindGameObjectsWithTag("SafeItem");
        foreach (GameObject item in incorrectItems)
        {
            XRayItem xRayItem = item.GetComponent<XRayItem>();
            if (xRayItem != null && xRayItem.IsMarked)
            {
                return false;
            }
        }

        return true; 
    }
    
}
