using UnityEngine;

public class SceneManager : MonoBehaviour
{
    public GameObject passengerCheckObjects;
    public GameObject observationScreenObjects;
    public XRayRandomizer xRayRandomizer;

    private void Start()
    {
        ShowPassengerCheck();
    }

    public void ShowObservationScreen()
    {
        passengerCheckObjects.SetActive(false);
        observationScreenObjects.SetActive(true);
        
        ReloadObservationScreen();
    }

    public void ShowPassengerCheck()
    {
        passengerCheckObjects.SetActive(true);
        observationScreenObjects.SetActive(false);

        SuitcaseManager.ResetSuitcase();
    }

    private void ReloadObservationScreen()
    {
        if (xRayRandomizer != null)
        {
            xRayRandomizer.ClearItems();
            xRayRandomizer.GenerateRandomItems();
        }
    }
}