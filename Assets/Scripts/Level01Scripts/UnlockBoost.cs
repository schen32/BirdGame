using UnityEngine;

public class UnlockBoost : MonoBehaviour
{
    public GameObject moveTutorialPrompt;
    public GameObject jumpTutorialPrompt;
    public GameObject boostLeftRightTutorialPrompt;
    public GameObject boostUpTutorialPrompt;

    AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        PlayerMovementScript playerMovement = collision.GetComponent<PlayerMovementScript>();
        playerMovement.canBoostLeft = true;
        playerMovement.canBoostRight = true;
        playerMovement.canBoostUp = true;

        moveTutorialPrompt.SetActive(false);
        jumpTutorialPrompt.SetActive(false);
        boostLeftRightTutorialPrompt.SetActive(true);
        boostUpTutorialPrompt.SetActive(true);

        audioSource.Play();
    }
}
