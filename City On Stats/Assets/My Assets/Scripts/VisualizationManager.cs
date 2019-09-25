using DigitalRuby.RainMaker;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VisualizationManager : MonoBehaviour
{
    public Text text;
    public Material material;
    public GameObject cylinder;
    public GameObject Cubes;
    public GameObject Fog;
    public GameObject Rain;

    public GameObject AutoBusParent;

    ArrayList listOfVisualization;

    DataStorage DataStorageObject;
    int active = 0;
    int activeAuto = 0;
    bool vCalled = false;
    double nextTime;

    // Start is called before the first frame update
    void Start()
    {
        listOfVisualization = new ArrayList { text, material, cylinder, Cubes, Cubes, Fog, Rain };
        nextTime = Time.time;
        DataStorageObject = GameObject.FindGameObjectWithTag("DataStorage").GetComponent<DataStorage>();
        StaticInfos si = GameObject.FindGameObjectWithTag("StaticInfos").GetComponent<StaticInfos>();
        try
        {
            si.setActualGas(DataStorageObject.getGasList()[0]);
        }
        catch (IndexOutOfRangeException e)
        {

        }
    }

    // Update is called once per frame
    void Update()
    {
        StaticInfos si = GameObject.FindGameObjectWithTag("StaticInfos").GetComponent<StaticInfos>();
        if (!si.isOnAiBusMode())
        {
            if (Input.GetKeyDown("v") || vCalled)
            {
                vCalled = false;
                switch (active)
                {
                    case 0:
                        break;

                    case 1:
                        material.color = new Color(1f, 1f, 0f);
                        break;

                    case 2:
                        cylinder.SetActive(false);
                        break;

                    case 4:
                        ((GameObject)listOfVisualization[active]).SetActive(false);
                        break;

                    case 5:
                        ((GameObject)listOfVisualization[active]).SetActive(false);
                        RenderSettings.fogDensity = 0f;
                        break;

                    case 6:
                        ((GameObject)listOfVisualization[active]).SetActive(false);
                        break;
                }

                active++;
                if (active == listOfVisualization.Count)
                    active = 0;

                switch (active)
                {
                    case 0:
                        break;

                    case 1:
                        nextTime = Time.time;
                        material.color = new Color(1f, 1f, 0f);
                        break;

                    case 2:
                        cylinder.SetActive(true);
                        break;

                    case 3:
                        ((GameObject)listOfVisualization[active]).SetActive(true);
                        Cubes.GetComponent<CubeView>().activateColorMode();
                        ((GameObject)listOfVisualization[active]).GetComponent<CubeView>().resetAndSetMode(true);
                        break;

                    case 4:
                        Cubes.GetComponent<CubeView>().activateHeightMode();
                        ((GameObject)listOfVisualization[active]).GetComponent<CubeView>().resetAndSetMode(false);
                        break;

                    case 5:
                        Fog.GetComponent<FogController>().updateTime();
                        ((GameObject)listOfVisualization[active]).SetActive(true);
                        RenderSettings.fogDensity = 0f;
                        break;

                    case 6:
                        ((GameObject)listOfVisualization[active]).SetActive(true);
                        Rain.GetComponent<RainScript>().RainIntensity = 1;
                        break;
                }
            }
            if (Time.time >= nextTime)
            {
                try
                {
                    if (!si.getActualGas().Equals("") && !si.getActualGas().Equals(null))
                    {
                        text.text = si.getActualGas() + "\n" + DataStorageObject.getGasValue(si.getActualGas(), new Vector2(((int)Camera.current.transform.position.z), ((int)Camera.current.transform.position.x)));
                        switch (active)
                        {
                            case 1:
                                material.color = DataStorageObject.getColorValue(si.getActualGas(), new Vector2(((int)Camera.current.transform.position.z), ((int)Camera.current.transform.position.x)));
                                break;

                            case 2:
                                cylinder.GetComponent<CylinderController>().updateCylinder(DataStorageObject.getNormValue(si.getActualGas(), new Vector2(((int)Camera.current.transform.position.z), ((int)Camera.current.transform.position.x))), DataStorageObject.getColorValue(si.getActualGas(), new Vector2(((int)Camera.current.transform.position.z), ((int)Camera.current.transform.position.x))));
                                break;

                            case 3:
                                Cubes.GetComponent<CubeView>().updateValues();
                                break;

                            case 4:
                                Cubes.GetComponent<CubeView>().updateValues();
                                break;

                            case 5:
                                RenderSettings.fogDensity = ((float)(DataStorageObject.getNormValue(si.getActualGas(), new Vector2(((int)Camera.current.transform.position.z), ((int)Camera.current.transform.position.x))) / 10));
                                break;

                            case 6:
                                Rain.GetComponent<RainScript>().RainIntensity = ((float)(DataStorageObject.getNormValue(si.getActualGas(), new Vector2(((int)Camera.current.transform.position.z), ((int)Camera.current.transform.position.x)))));
                                break;
                        }
                    }
                    else
                    {
                        text.text = "No values\n---";
                    }
                }
                catch (NullReferenceException e)
                {

                }

                nextTime += 0.5;
            }
        }

        else
        {
            if (Input.GetKeyDown("v") || vCalled)
            {
                vCalled = false;
                GameObject activeAutoBus = null;
                foreach (Transform t in AutoBusParent.transform)
                {
                    //if (t.gameObject.GetComponent<BusAi>().enabled == true)
                    if (t.Find("camera").gameObject.GetComponent<Camera>().enabled == true)
                    {
                        Debug.Log(t.gameObject.name);
                        activeAutoBus = t.gameObject;
                        break;
                    }
                }

                switch (activeAuto)
                {
                    case 0:
                        break;

                    case 1:
                        if (activeAutoBus != null)
                        {
                            activeAutoBus.transform.Find("Cylinder").gameObject.SetActive(false);
                        }
                        break;

                    case 2:
                        Fog.SetActive(false);
                        RenderSettings.fogDensity = 0f;
                        break;
                }

                activeAuto++;
                if (activeAuto == 3)
                    activeAuto = 0;

                switch (activeAuto)
                {
                    case 0:
                        break;

                    case 1:
                        if (activeAutoBus != null)
                        {
                            activeAutoBus.transform.Find("Cylinder").gameObject.SetActive(true);
                        }
                        break;

                    case 2:
                        Fog.GetComponent<FogController>().updateTime();
                        Fog.SetActive(true);
                        RenderSettings.fogDensity = 0f;
                        break;
                }
            }
            if (Time.time >= nextTime)
            {
                GameObject activeAutoBus = null;
                foreach (Transform t in AutoBusParent.transform)
                {
                    if (t.Find("camera").gameObject.GetComponent<Camera>().enabled == true)
                    {
                        activeAutoBus = t.gameObject;
                        Debug.Log(activeAutoBus.name);
                        break;
                    }
                }
                if (activeAutoBus != null)
                {
                    text.text = activeAutoBus.GetComponent<BusAi>().getReaderText(si.getActualAiGas());
                }

                if (activeAuto == 1)
                {
                    activeAutoBus.transform.Find("Cylinder").gameObject.GetComponent<CylinderController>().updateCylinder(GameObject.FindGameObjectWithTag("DataColorHandler").GetComponent<DataColorHandler>().getNormValue(si.getActualAiGas(), activeAutoBus.GetComponent<BusAi>().getGasValue(si.getActualAiGas())), activeAutoBus.GetComponent<BusAi>().getGasColor(si.getActualAiGas()));
                }

                else if (activeAuto == 2)
                {
                    RenderSettings.fogDensity = ((float)(GameObject.FindGameObjectWithTag("DataColorHandler").GetComponent<DataColorHandler>().getNormValue(si.getActualAiGas(), activeAutoBus.GetComponent<BusAi>().getGasValue(si.getActualAiGas())) / 10));
                }
            }
        }
    }

    public void reset()
    {
        active = 0;
        activeAuto = 0;
        material.color = new Color(1f, 1f, 0f);
        cylinder.SetActive(false);
        ((GameObject)listOfVisualization[4]).SetActive(false);
        ((GameObject)listOfVisualization[5]).SetActive(false);
        RenderSettings.fogDensity = 0f;
        ((GameObject)listOfVisualization[6]).SetActive(false);
        foreach (Transform t in AutoBusParent.transform)
        {
            t.Find("Cylinder").gameObject.SetActive(false);
        }
        text.text = "No values\n---";
    }

    public void callForVisualization()
    {
        vCalled = true;
    }
}
