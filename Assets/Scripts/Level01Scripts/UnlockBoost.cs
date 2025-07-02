using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnlockBoost : MonoBehaviour
{
    public GameObject moveTutorialPrompt;
    public GameObject jumpTutorialPrompt;
    public GameObject boostLeftRightTutorialPrompt;
    public GameObject boostUpTutorialPrompt;

    public GameObject creditsScreen;
    public TextMeshProUGUI statsText;
    int numTimesCleared = 0;

    AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        numTimesCleared++;
        if (numTimesCleared >= 2)
        {
            TimeSpan timeSpan = TimeSpan.FromSeconds(Time.time);
            string formattedTime = string.Format("{0:D2}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds);

            statsText.text = "Deaths: " + GMRespawn.Instance.timesDied + "\n" +
                             "Time: " + formattedTime;
            creditsScreen.SetActive(true);
        }

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
