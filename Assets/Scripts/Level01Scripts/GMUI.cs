using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GMUI : MonoBehaviour
{
    public Animator sceneTransitionAnimator;
    public GameObject pauseMenuUI;
    bool isPaused = false;
    public void OnPause(InputValue value)
    {
        if (isPaused)
            Resume();
        else
            Pause();
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void MainMenu()
    {
        StartCoroutine(MainMenuCoroutine());
    }
    IEnumerator MainMenuCoroutine()
    {
        Time.timeScale = 1f;
        isPaused = false;
        sceneTransitionAnimator.SetTrigger("SceneEnd");

        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
