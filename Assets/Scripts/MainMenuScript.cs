using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject optionsMenu;
    public void PlayGame()
    {
        SceneManager.LoadScene("Level01"); // or use index: SceneManager.LoadScene(1);
    }
    public void QuitGame()
    {
        Application.Quit();
    }

    public void Options()
    {
        optionsMenu.SetActive(true);
        gameObject.SetActive(false);
    }
}
