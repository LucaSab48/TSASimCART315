using UnityEngine;

public class DontDestroy : MonoBehaviour
{
    private static GameObject persistentObject;

    void Awake()
    {
        if (persistentObject == null)
        {
            persistentObject = gameObject;
            DontDestroyOnLoad(gameObject);
        }
        else if (persistentObject != gameObject)
        {
            Destroy(gameObject);
        }
    }
}

