using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneButtons : MonoBehaviour
{
    public GameObject panelController;
    public VisualizationManager vm;
    public ViewChanger vc;
    StaticInfos si;

    // Start is called before the first frame update
    void Start()
    {
        si = GameObject.FindGameObjectWithTag("StaticInfos").GetComponent<StaticInfos>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void exitClicked()
    {
        panelController.GetComponent<PanelController>().escape();
    }

    public void teleportClicked()
    {
        if (!si.isOnAiBusMode())
            panelController.GetComponent<PanelController>().teleport();
    }

    public void busClicked()
    {
        if (!si.isOnAiBusMode())
            panelController.GetComponent<PanelController>().bus();
    }

    public void mapClicked()
    {
        panelController.GetComponent<PanelController>().map();
    }

    public void gasClicked()
    {
        panelController.GetComponent<PanelController>().gases();
    }

    public void visualizationClicked()
    {
        vm.callForVisualization();
    }

    public void camaraClicked()
    {
        vc.cCall();
    }

    public void helpClicked()
    {

    }

    public void challengesClicked()
    {

    }
}
