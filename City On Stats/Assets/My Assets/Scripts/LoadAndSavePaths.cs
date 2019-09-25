using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadAndSavePaths : MonoBehaviour
{
    public GameObject LoadPanel;
    public GameObject SavePanel;

    public Text objPath;
    public Text osmPath;
    public Text imgPath;

    public Text name;
    public Text log;

    public Text LoadSelected;

    public Button model;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void buttonClicked(Button button)
    {
        if (button.name.Equals("LoadConf"))
        {
            loadConfigs();
        }

        if (button.name.Equals("SaveConf"))
        {
            saveConfigs();
        }

        if (button.name.Equals("LoadBack"))
        {
            LoadPanel.SetActive(false);
        }

        if (button.name.Equals("SaveBack"))
        {
            SavePanel.SetActive(false);
        }

        if (button.name.Equals("SaveButton"))
        {
            doSaveStuff();
        }

        if (button.name.Equals("LoadSelected"))
        {
            load();
        }

        if (button.name.Equals("DeleteSelected"))
        {
            delete();
        }


    }

    private void saveConfigs()
    {
        SavePanel.SetActive(true);

        string result = "";

        result += logBasicText();

        log.text = result;
    }

    private void delete()
    {
        String path = Application.dataPath;
        path += "/Resources/confs.los";

        StreamReader reader = new StreamReader(path);
        String data = reader.ReadToEnd();
        reader.Close();

        string[] stringSeparators = new string[] { "[@#@]" };
        string[] stringSeparators2 = new string[] { "[@N@]" };

        string result = "";

        foreach (string aux in data.Split(stringSeparators, StringSplitOptions.None))
        {
            if (!aux.Equals(""))
                if (!aux.Split(stringSeparators2, StringSplitOptions.None)[1].Equals(LoadSelected.text))
                    result += "[@#@]" + aux + "[@#@]";
        }

        System.IO.File.Delete(path);

        StreamWriter writer = new StreamWriter(path, true);
        writer.Write(result);
        writer.Close();
        loadConfigs();
    }

    private void load()
    {
        GameObject go = GameObject.FindGameObjectWithTag("StaticInfos");
        StaticInfos si = go.GetComponent<StaticInfos>();

        String path = Application.dataPath;
        path += "/Resources/confs.los";

        StreamReader reader = new StreamReader(path);
        String data = reader.ReadToEnd();
        reader.Close();

        string[] stringSeparators = new string[] { "[@#@]" };
        string[] stringSeparators2 = new string[] { "[@N@]" };

        foreach (string aux in data.Split(stringSeparators, StringSplitOptions.None))
        {
            if (!aux.Equals(""))
            {
                if (aux.Split(stringSeparators2, StringSplitOptions.None)[1].Equals(LoadSelected.text))
                {
                    stringSeparators2 = new string[] { "[@B@]" };
                    si.setObj(aux.Split(stringSeparators2, StringSplitOptions.None)[1]);
                    stringSeparators2 = new string[] { "[@S@]" };
                    si.setOsm(aux.Split(stringSeparators2, StringSplitOptions.None)[1]);
                    stringSeparators2 = new string[] { "[@I@]" };
                    si.setImg(aux.Split(stringSeparators2, StringSplitOptions.None)[1]);
                    break;
                }
            }
        }
        SceneManager.LoadScene("COS Virtual World");
    }

    private void loadConfigs()
    {
        LoadPanel.SetActive(true);
        String path = Application.dataPath;
        path += "/Resources/confs.los";
        String Data = "";
        GameObject content = LoadPanel.transform.Find("TeleportList").gameObject;
        content = content.transform.Find("Viewport").gameObject;
        content = content.transform.Find("Content").gameObject;

        foreach (Transform child in content.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        if (System.IO.File.Exists(path))
        {
            StreamReader reader = new StreamReader(path);
            String data = reader.ReadToEnd();
            reader.Close();
            ArrayList ListOfConfigs = new ArrayList();
            String[] stringSeparators = new string[] { "[@N@]" };
            int i = 0;
            foreach (string aux in data.Split(stringSeparators, StringSplitOptions.None))
            {
                if (i % 2 == 1)
                {
                    ListOfConfigs.Add(aux);
                }
                i++;
            }

            foreach (String name in ListOfConfigs)
            {
                var copy = Instantiate(model);
                copy.transform.parent = content.transform;
                copy.transform.localPosition = Vector3.zero;
                copy.GetComponentInChildren<Text>().text = name;
            }
        }

    }

    private string logBasicText()
    {
        string result = "";

        if (objPath.text != null && !objPath.text.Equals("Select an .obj file") && objPath.text.Contains(".obj"))
        {
            result += "Path to obj file: " + objPath.text + "\n\n";
        }
        else
        {
            result += "!!!Path to obj file not defined properly!!!\n\n";
        }

        if (osmPath.text != null && !osmPath.text.Equals("Select an .osm file") && osmPath.text.Contains(".osm"))
        {
            result += "Path to osm file: " + objPath.text + "\n\n";
        }
        else
        {
            result += "!!!Path to osm file not defined properly!!!\n\n";
        }

        if (imgPath.text != null && !imgPath.text.Equals("(Not mandatory) Select an image file"))
        {
            result += "Path to map image file: " + objPath.text + "\n\n";
        }
        else
        {
            result += "Path to image map file not defined properly, the application will run without a real world mini map\n\n";
        }

        return result;
    }

    private void doSaveStuff()
    {
        String path = Application.dataPath;

        if (objPath.text != null && !objPath.text.Equals("Select an .obj file") && objPath.text.Contains(".obj") && osmPath.text != null && !osmPath.text.Equals("Select an .osm file") && osmPath.text.Contains(".osm") && !name.Equals(""))
        {
            String result = "";
            if (existConfWithName())
            {
                result += "!!! There are a configuration with this name !!!\n\n";
                result += logBasicText();
                log.text = result;
            }
            else
            {
                path += "/Resources/confs.los";
                if (System.IO.File.Exists(path))
                {
                    StreamReader reader = new StreamReader(path);
                    String data = reader.ReadToEnd();
                    reader.Close();
                    data += "[@#@][@N@]" + name.text + "[@N@][@B@]" + objPath.text + "[@B@][@S@]" + osmPath.text + "[@S@][@I@]" + imgPath.text + "[@I@][@#@]";
                    System.IO.File.Delete(path);
                    StreamWriter writer = new StreamWriter(path, true);
                    writer.Write(data);
                    writer.Close();
                }
                else
                {
                    String data = "[@#@][@N@]" + name.text + "[@N@][@B@]" + objPath.text + "[@B@][@S@]" + osmPath.text + "[@S@][@I@]" + imgPath.text + "[@I@][@#@]";
                    StreamWriter writer = new StreamWriter(path, true);
                    writer.Write(data);
                    writer.Close();
                }
                SavePanel.SetActive(false);
            }
        }
        else
        {
            String result = "";

            if (objPath.text == null || objPath.text.Equals("Select an .obj file") || !objPath.text.Contains(".obj"))
                result += "!!! There are a problem with obj path !!!\n\n";

            if (osmPath.text == null || osmPath.text.Equals("Select an .osm file") || !osmPath.text.Contains(".osm"))
                result += "!!! There are a problem with osm path !!!\n\n";

            result += logBasicText();

            log.text = result;
        }
    }

    private bool existConfWithName()
    {
        String path = Application.dataPath;
        path += "/Resources/confs.los";
        if (System.IO.File.Exists(path))
        {
            StreamReader reader = new StreamReader(path);
            String data = reader.ReadToEnd();
            reader.Close();
            string[] stringSeparators = new string[] { "[@#@]" };
            string[] confs = data.Split(stringSeparators, StringSplitOptions.None);
            foreach (string conf in confs)
            {
                if (!conf.Equals(""))
                {
                    stringSeparators = new string[] { "[@N@]" };
                    if (name.text.Equals(conf.Split(stringSeparators, StringSplitOptions.None)[1]))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        else
        {
            return false;
        }
    }

    public void ScrollButtonClicked(Button b)
    {
        LoadSelected.text = b.transform.Find("Text").gameObject.GetComponent<Text>().text;
    }
}
