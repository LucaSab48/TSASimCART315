using UnityEngine;

public class PlaneMovement : MonoBehaviour
{
    public Vector2 destination; 
    public float moveSpeed = 2f; 
    private bool isMoving = false; 
    private Vector2 startPosition; 
    private float journeyLength; 
    private float startTime;

    [Header("Audio")]
    public AudioClip takeoffSound;
    public AudioClip crashSound;
    private AudioSource audioSource;

    private void Start()
    {
        startPosition = transform.position;
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void Update()
    {
        if (isMoving)
        {
            MovePlane();
        }
    }

    public void StartMoving(Vector2 newDestination)
    {
        destination = newDestination;
        isMoving = true;
        startTime = Time.time;
        journeyLength = Vector2.Distance(startPosition, destination);

        // Determine direction to play the right sound
        if (destination.y > startPosition.y)
        {
            PlayTakeoffSound();
        }
        else
        {
            PlayCrashSound();
        }
    }

    private void MovePlane()
    {
        float distanceCovered = (Time.time - startTime) * moveSpeed;
        float fractionOfJourney = distanceCovered / journeyLength;

        transform.position = Vector2.Lerp(startPosition, destination, fractionOfJourney);

        if (fractionOfJourney >= 1)
        {
            isMoving = false;
        }
    }

    private void PlayTakeoffSound()
    {
        if (takeoffSound != null)
        {
            audioSource.clip = takeoffSound;
            audioSource.Play();
        }
    }

    private void PlayCrashSound()
    {
        if (crashSound != null)
        {
            audioSource.clip = crashSound;
            audioSource.Play();
        }
    }
}