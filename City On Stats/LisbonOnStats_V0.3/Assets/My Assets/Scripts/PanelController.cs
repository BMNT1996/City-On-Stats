using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Vehicles.Car;
using static BusGenerator;

public class PanelController : MonoBehaviour
{
    public GameObject TeleportPanel;
    public GameObject EscPanel;
    public GameObject MapPanel;
    public GameObject BusPanel;
    public GameObject GasesPanel;
    public GameObject HelpPanel;
    public GameObject ChallengesPanel;
    public GameObject TutorialPanel;
    public GameObject HelpSelection;
    public GameObject WarningPanel;

    public GameObject StreetHandler;
    public GameObject itemTemplate;
    public GameObject content;
    public InteractiveTutorial Tutorial;

    public GameObject visual;
    public GameObject buses;
    public GameObject busGenerator;
    public GameObject busControl;

    bool panelActive;
    StaticInfos si;

    // Start is called before the first frame update
    void Start()
    {
        GameObject go = GameObject.FindGameObjectWithTag("StaticInfos");
        si = go.GetComponent<StaticInfos>();
        panelActive = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("m"))
        {
            map();
        }

        if (Input.GetKeyDown("t") || Input.GetKeyDown("JoystickButton2"))
        {
            if(!si.isOnAiBusMode())
                teleport();
            else
                WarningPanel.SetActive(true);
                WarningPanel.GetComponent<WarningScript>().setTimer(5);
        }

        if ((Input.GetKeyDown("b") || Input.GetKeyDown("JoystickButton1")) && !si.isOnAiBusMode())
        {
            bus();
        }

        if (Input.GetKeyDown("g") && !si.isOnAiBusMode())
        {
            gases();
        }

        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown("JoystickButton7"))
        {
            escape();
        }

		if (Input.GetKeyDown("r") && !si.isOnAiBusMode())
        {
			GameObject gm = GameObject.FindGameObjectWithTag("Player");
            gm.transform.position=new Vector3 (gm.transform.position.x,5,gm.transform.position.z);
			gm.GetComponent<Rigidbody>().velocity = Vector3.zero;
			gm.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
			gm.transform.eulerAngles=new Vector3 (0,0,0);
		}
    }

    public void gases()
    {
        if (!GasesPanel.activeSelf)
        {
            closeAllPanels();
            panelActive = true;
            GasesPanel.SetActive(true);
            DataStorage DS = GameObject.FindGameObjectWithTag("DataStorage").GetComponent<DataStorage>();
            Debug.Log("Panel=" + DS.getGasList().Length);

            foreach (Transform child in GasesPanel.transform.Find("Viewport").Find("Content"))
            {
                GameObject.Destroy(child.gameObject);
            }

            foreach (string name in DS.getGasList())
            {
                var copy = Instantiate(itemTemplate);
                copy.transform.parent = GasesPanel.transform.Find("Viewport").Find("Content");
                copy.transform.localPosition = Vector3.zero;
                copy.GetComponentInChildren<Text>().text = name;
            }
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            closeAllPanels();
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    public void map()
    {
        GameObject go = GameObject.FindGameObjectWithTag("StaticInfos");
        StaticInfos si = go.GetComponent<StaticInfos>();
        if (si.getImg() != null && !si.getImg().Equals("(Not mandatory) Select an image file"))
        {
            panelActive = true;
            Texture2D tmpTexture = new Texture2D(1, 1);
            byte[] tmpBytes = File.ReadAllBytes(si.getImg());
            tmpTexture.LoadImage(tmpBytes);

            float imgX = tmpTexture.width;
            float imgY = tmpTexture.height;

            if (MapPanel.active == false)
            {
                closeAllPanels();
                panelActive = true;
                GameObject image = MapPanel.transform.Find("Map").gameObject;

                if (imgX / imgY > 740 / 298)
                {
                    var imag = image.transform as RectTransform;
                    imag.sizeDelta = new Vector2(740, 740 * (imgY / imgX));
                }
                else
                {
                    var imag = image.transform as RectTransform;
                    imag.sizeDelta = new Vector2(298 * (imgX / imgY), 298);
                }

                image.GetComponent<RawImage>().texture = tmpTexture;

                MapPanel.SetActive(true);
            }

            else
            {
                closeAllPanels();
            }
        }
    }

    public void teleport()
    {
        Tutorial.teleportUsed();
        if (!TeleportPanel.activeSelf)
        {
            closeAllPanels();
            panelActive = true;
            TeleportPanel.SetActive(true);

            if (content.transform.childCount == 0)
            {
                foreach (Transform child in content.transform)
                {
                    GameObject.Destroy(child.gameObject);
                }
                ArrayList StreetList = StreetHandler.GetComponent<StreetsHandler>().getStreetList();
                ArrayList NameList = new ArrayList();
                foreach (StreetsHandler.StreetCoordenates sc in StreetList)
                {
                    if (!NameList.Contains(sc.getStreetName()))
                    {
                        NameList.Add(sc.getStreetName());
                    }
                }

                NameList.Sort();

                foreach (string name in NameList)
                {
                    var copy = Instantiate(itemTemplate);
                    copy.transform.parent = content.transform;
                    copy.transform.localPosition = Vector3.zero;
                    copy.GetComponentInChildren<Text>().text = name;
                }
            }
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            closeAllPanels();
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    public void bus()
    {
        Tutorial.busesUsed();
        if (!BusPanel.activeSelf)
        {
            closeAllPanels();
            panelActive = true;
            BusPanel.SetActive(true);

            foreach (Transform child in BusPanel.transform.Find("Viewport").Find("Content"))
            {
                GameObject.Destroy(child.gameObject);
            }

            ArrayList busList = busGenerator.GetComponent<BusGenerator>().getBusList();
            ArrayList NameList = new ArrayList();

            foreach (bus b in busList)
            {
                if (!NameList.Contains(b.getName()))
                {
                    NameList.Add(b.getName());
                }
            }

            NameList.Sort();

            foreach (string name in NameList)
            {
                var copy = Instantiate(itemTemplate);
                copy.transform.parent = BusPanel.transform.Find("Viewport").Find("Content");
                copy.transform.localPosition = Vector3.zero;
                copy.GetComponentInChildren<Text>().text = name;
            }
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            closeAllPanels();
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

	public void help(){
		if(!HelpPanel.activeSelf){
			closeAllPanels();
            panelActive = true;
            HelpPanel.SetActive(true);
		}
		else{
			closeAllPanels();
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
		}
	}

    internal void tutorial()
    {
        if (!TutorialPanel.activeSelf)
        {
            closeAllPanels();
            panelActive = true;
            TutorialPanel.SetActive(true);
        }
        else
        {
            closeAllPanels();
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    internal void helpSelection()
    {
        if (!HelpSelection.activeSelf)
        {
            closeAllPanels();
            panelActive = true;
            HelpSelection.SetActive(true);
        }
        else
        {
            closeAllPanels();
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    public void challenge(){
		if(!ChallengesPanel.activeSelf){
			closeAllPanels();
            panelActive = true;
            ChallengesPanel.SetActive(true);
		}
		else{
			closeAllPanels();
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
		}
	}

    public void escape()
    {
        if (panelActive)
            closeAllPanels();
        else if (!si.isOnAiBusMode())
        {
            if (!EscPanel.activeSelf)
            {
                closeAllPanels();
                panelActive = true;
                EscPanel.SetActive(true);
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                closeAllPanels();
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
        }
        else
        {
            GameObject Player = null;
            foreach (GameObject g in GameObject.FindGameObjectsWithTag("Player"))
            {
                if (g.name.Equals("PlayerView"))
                {
                    Player = g;
                    break;
                }
            }

            if (Player != null)
            {
                busControl.GetComponent<BusUserControl>().close();
                visual.GetComponent<VisualizationManager>().reset();
                GameObject camara = Camera.main.gameObject;
                Player.transform.Find("camera").GetComponent<Camera>().enabled = true;
                Player.transform.Find("camera").GetComponent<AudioListener>().enabled = true;
                Player.transform.gameObject.GetComponent<CarController>().enabled = true;
                Player.transform.gameObject.GetComponent<CarUserControl>().enabled = true;
                //Player.transform.gameObject.GetComponent<CarAudio>().enabled = true;
                camara.GetComponent<Camera>().enabled = false;
                camara.GetComponent<AudioListener>().enabled = false;
                GameObject go = GameObject.FindGameObjectWithTag("StaticInfos");
                StaticInfos si = go.GetComponent<StaticInfos>();
                si.setOnAiBus(false);
            }
        }
    }

    public bool isPanelActive()
    {
        return panelActive;
    }

    public void closeAllPanels()
    {
        panelActive = false;
        if (TeleportPanel.active)
            TeleportPanel.SetActive(false);
        if (BusPanel.active)
            BusPanel.SetActive(false);
        if (MapPanel.active)
            MapPanel.SetActive(false);
        if (EscPanel.active)
            EscPanel.SetActive(false);
        if (GasesPanel.active)
            GasesPanel.SetActive(false);
		if (HelpPanel.active)
            HelpPanel.SetActive(false);
		if (ChallengesPanel.active)
            ChallengesPanel.SetActive(false);
        if (HelpSelection.active)
            HelpSelection.SetActive(false);
        if (TutorialPanel.active)
            TutorialPanel.SetActive(false);
    }
}
