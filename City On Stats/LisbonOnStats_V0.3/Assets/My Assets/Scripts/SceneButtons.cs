using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneButtons : MonoBehaviour
{
    public GameObject panelController;
    public VisualizationManager vm;
    public ViewChanger vc;
    public GameObject WarningPanel;
    StaticInfos si;

    // Start is called before the first frame update
    void Start()
    {
        si = GameObject.FindGameObjectWithTag("StaticInfos").GetComponent<StaticInfos>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("JoystickButton6"))
        {
            helpClicked();
        }

        if (Input.GetKeyDown("JoystickButton5"))
        {
            challengeClicked();
        }
    }

    public void exitClicked()
    {
        panelController.GetComponent<PanelController>().escape();
    }

    public void teleportClicked()
    {
        if (!si.isOnAiBusMode())
            panelController.GetComponent<PanelController>().teleport();
        else
        {
            WarningPanel.SetActive(true);
            WarningPanel.GetComponent<WarningScript>().setTimer(5);
        }
    }

    public void busClicked()
    {
        if (!si.isOnAiBusMode())
            panelController.GetComponent<PanelController>().bus();
        else
        {
            panelController.GetComponent<PanelController>().escape();
            panelController.GetComponent<PanelController>().bus();
        }
    }

    public void mapClicked()
    {
        panelController.GetComponent<PanelController>().map();
    }

    public void gasClicked()
    {
        panelController.GetComponent<PanelController>().gases();
    }
	
	public void helpClicked()
    {
        panelController.GetComponent<PanelController>().help();
    }

    public void helpSelectionClicked()
    {
        panelController.GetComponent<PanelController>().helpSelection();
    }

    public void tutorialClicked()
    {
        panelController.GetComponent<PanelController>().tutorial();
    }
	
	public void challengeClicked()
    {
        panelController.GetComponent<PanelController>().challenge();
    }

    public void visualizationClicked()
    {
        vm.callForVisualization();
    }

    public void camaraClicked()
    {
        vc.cCall();
    }
}
