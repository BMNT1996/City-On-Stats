using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EscPanelButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void buttonClicked()
    {
        if (gameObject.name.Equals("menu"))
        {
            SceneManager.LoadScene("COS Main Menu");
        }
        if (gameObject.name.Equals("select"))
        {
            SceneManager.LoadScene("COS Configuration");
        }

        if (gameObject.name.Equals("exit"))
        {
            Application.Quit();
        }
    }
}
