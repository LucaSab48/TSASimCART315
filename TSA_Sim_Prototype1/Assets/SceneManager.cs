using UnityEngine;

public class SceneManager : MonoBehaviour
{
    public GameObject passengerCheckObjects;
    public GameObject observationScreenObjects;

    private void Start()
    {
        ShowPassengerCheck();
    }

    public void ShowObservationScreen()
    {
        passengerCheckObjects.SetActive(false);
        observationScreenObjects.SetActive(true);
    }

    public void ShowPassengerCheck()
    {
        passengerCheckObjects.SetActive(true);
        observationScreenObjects.SetActive(false);
        
        // Ensure suitcase stops moving when returning
        SuitcaseManager.ResetSuitcase();
    }
}