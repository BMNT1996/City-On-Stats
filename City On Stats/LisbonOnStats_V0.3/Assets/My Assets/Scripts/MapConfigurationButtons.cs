using SimpleFileBrowser;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MapConfigurationButtons : MonoBehaviour
{
    public Button obj;
    public Button osm;
    public Button img;
    public Button confirmation;

    public Text objPath;
    public Text osmPath;
    public Text imgPath;

    static int buttonClicked = 0;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void objClicked()
    {
        buttonClicked = 1;
        StartCoroutine(ShowLoadDialogCoroutine());
    }

    public void osmClicked()
    {
        buttonClicked = 2;
        StartCoroutine(ShowLoadDialogCoroutine());
    }

    public void imgClicked()
    {
        buttonClicked = 3;
        StartCoroutine(ShowLoadDialogCoroutine());
    }

    IEnumerator ShowLoadDialogCoroutine()
    {
        yield return FileBrowser.WaitForLoadDialog(false, null, "Load File", "Load");
        switch (buttonClicked)
        {
            case 1:
                objPath.text = FileBrowser.Result;
                break;
            case 2:
                osmPath.text = FileBrowser.Result;
                break;
            case 3:
                imgPath.text = FileBrowser.Result;
                break;

        }
    }

    public void confirmationClicked()
    {
        if (objPath.text.ToString().Contains(".obj") && !objPath.text.ToString().Equals("Select an .obj file"))
            GetComponent<StaticInfos>().setObj(objPath.text.ToString());
        else
            return;

        if (osmPath.text.ToString().Contains(".osm") && !osmPath.text.ToString().Equals("Select an .osm file"))
            GetComponent<StaticInfos>().setOsm(osmPath.text.ToString());
        else
            return;

        GetComponent<StaticInfos>().setImg(imgPath.text.ToString());

        SceneManager.LoadScene("COS Virtual World");

    }

	public void backClicked(){
		SceneManager.LoadScene("COS Main Menu");
	}
}
