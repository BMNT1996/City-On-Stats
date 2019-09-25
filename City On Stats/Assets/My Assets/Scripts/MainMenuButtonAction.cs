using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButtonAction : MonoBehaviour
{
    public void startCOS()
    {
        SceneManager.LoadScene("COS Configuration");
    }

    public void creditsCOS()
    {
        SceneManager.LoadScene("COS Credits");
    }

    public void exitCOS()
    {
        Application.Quit();
    }
}
