using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public string nextSceneName; // Set this in the Inspector

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("SafeSuitcase") || other.CompareTag("DangerousSuitcase")) // Ensure your suitcase GameObjects are tagged as "Suitcase"
        { ;
            SceneManager.LoadScene("ObservationScreen");
        }
    }
}
