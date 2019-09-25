using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleFileBrowser;
using System.IO;
using UnityEngine.SceneManagement;

public class FileManager : MonoBehaviour
{
    string path;

    public void openExplorer()
    {
        FileBrowser.SetDefaultFilter(".obj");
        StartCoroutine(ShowLoadDialogCoroutine());
    }

    IEnumerator ShowLoadDialogCoroutine()
    {
        yield return FileBrowser.WaitForLoadDialog(false, null, "Load File", "Load");
        path = FileBrowser.Result;
        goNextScene();
    }

    void goNextScene()
    {
        if (path != null)
        {
            GameObject obj = GameObject.FindGameObjectWithTag("Path");
            obj.GetComponent<PathUtil>().setPath(path);
            SceneManager.LoadScene(1);
        }
        else
        {
            Debug.Log("Path is null");
        }
    }

}