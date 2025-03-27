using UnityEngine;

public class GameManager : MonoBehaviour
{
    private SceneManager sceneManager;
    private Collider2D _myCollider2D;

    public int maxSuitcases = 10;

    private void Start()
    {
        sceneManager = Object.FindFirstObjectByType<SceneManager>();
        _myCollider2D = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("SafeSuitcase") || other.CompareTag("DangerousSuitcase"))
        {
            SuitcaseManager.currentSuitcase = other.gameObject;
            
            SuitcaseManager.ResetSuitcase();
            
            sceneManager.ShowObservationScreen();
            
            _myCollider2D.enabled = false;
        }
    }

    public void EnableCollider()
    {
        _myCollider2D.enabled = true;
    }
}