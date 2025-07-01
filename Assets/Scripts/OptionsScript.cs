using UnityEngine;

public class OptionsScript : MonoBehaviour
{
    public GameObject mainMenu;
    public void MainMenu()
    {
        mainMenu.SetActive(true);
        gameObject.SetActive(false);
    }
}
