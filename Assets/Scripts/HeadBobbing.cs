using UnityEngine;

public class SC_HeadBobber : MonoBehaviour
{
    [SerializeField] float walkingBobbingSpeed = 14f;
    [SerializeField] float bobbingAmount = 0.05f;
    [SerializeField] Movement movementController;
    [SerializeField] AudioClip[] footstepSounds; // Array of footstep sound variations
    [SerializeField] AudioSource audioSource; // Reference to the AudioSource component

    float defaultPosY = 0;
    float timer = 0;
    bool isWalking = false;
    float nextFootstepTime = 0;
    float footstepInterval = 0.5f; // Time interval between each footstep sound

    // Start is called before the first frame update
    void Start()
    {
        defaultPosY = transform.localPosition.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (Mathf.Abs(movementController.velocity.x) > 0.1f || Mathf.Abs(movementController.velocity.z) > 0.1f)
        {
            // Player is moving
            timer += Time.deltaTime * walkingBobbingSpeed;
            transform.localPosition = new Vector3(transform.localPosition.x, defaultPosY + Mathf.Sin(timer) * bobbingAmount, transform.localPosition.z);

            if (!isWalking || Time.time >= nextFootstepTime)
            {
                PlayRandomFootstepSound();
                nextFootstepTime = Time.time + footstepInterval;
            }

            isWalking = true;
        }
        else
        {
            // Idle
            timer = 0;
            transform.localPosition = new Vector3(transform.localPosition.x, Mathf.Lerp(transform.localPosition.y, defaultPosY, Time.deltaTime * walkingBobbingSpeed), transform.localPosition.z);

            if (isWalking)
            {
                StopFootstepSound();
            }

            isWalking = false;
        }
    }

    // Play a random footstep sound with a random pitch
    void PlayRandomFootstepSound()
    {
        if (footstepSounds.Length == 0)
        {
            return;
        }

        AudioClip randomFootstepSound = footstepSounds[Random.Range(0, footstepSounds.Length)];
        float randomPitch = Random.Range(0.8f, 1.2f);

        audioSource.clip = randomFootstepSound;
        audioSource.pitch = randomPitch;
        audioSource.Play();
    }

    // Stop the footstep sound
    void StopFootstepSound()
    {
        audioSource.Stop();
    }
}
