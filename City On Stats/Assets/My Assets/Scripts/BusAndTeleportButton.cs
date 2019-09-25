using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Vehicles.Car;
using static StreetsHandler.StreetCoordenates;

public class BusAndTeleportButton : MonoBehaviour
{
    public GameObject StreetHandler;
    public GameObject Player;
    public GameObject teleportPanel;
    public GameObject visual;
    public GameObject buses;
    public GameObject busControl;
    public PanelController panelController;
    public void buttonClicked()
    {
        Debug.Log(transform.parent.parent.parent.name);
        if (transform.parent.parent.parent.name.Equals("TeleportList"))
        {
            String location = GetComponentInChildren<Text>().text;
            ArrayList StreetList = StreetHandler.GetComponent<StreetsHandler>().getStreetList();
            foreach (StreetsHandler.StreetCoordenates sc in StreetList)
            {
                if (sc.getStreetName().Equals(location))
                {
                    double lat = 0;
                    double lon = 0;
                    int i = 0;

                    foreach (Coordenates coordenate in sc.getCoorenates())
                    {
                        lat += coordenate.getLatitude();
                        lon += coordenate.getLongitude();
                        i++;
                    }

                    GameObject go = GameObject.FindGameObjectWithTag("StaticInfos");
                    StaticInfos si = go.GetComponent<StaticInfos>();
                    Coordenates c = new Coordenates((lat / i), (lon / i));
                    double R = 6400;
                    double dLat = c.getLatitude() * Math.PI / 180 - si.getCenterLat() * Math.PI / 180;
                    double dLon = c.getLongitude() * Math.PI / 180 - si.getCenterLon() * Math.PI / 180;
                    double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2);
                    double b = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
                    double latitude = R * b * 1000;

                    a = Math.Cos(si.getCenterLat() * Math.PI / 180) * Math.Cos(c.getLatitude() * Math.PI / 180) * Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
                    b = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
                    double longitude = R * b * 1000;

                    if (si.getCenterLat() - c.getLatitude() < 0)
                    {
                        latitude *= -1;
                    }

                    if (si.getCenterLon() - c.getLongitude() < 0)
                    {
                        longitude *= -1;
                    }
                    Player.transform.position = new Vector3((float)longitude, Player.transform.position.y, (float)latitude);
                    teleportPanel.SetActive(false);
                    panelController.closeAllPanels();
                    break;
                }
            }
        }
        else if (transform.parent.parent.parent.name.Equals("BusesList"))
        {
            foreach (Transform Bus in buses.transform)
            {
                if (Bus.gameObject.active)
                {
                    if (Bus.gameObject.GetComponent<BusAi>().getName().Equals(GetComponentInChildren<Text>().text))
                    {
                        busControl.GetComponent<BusUserControl>().open(Bus.gameObject);
                        visual.GetComponent<VisualizationManager>().reset();
                        GameObject camara = Camera.main.gameObject;
                        GameObject parentt = camara.transform.parent.gameObject;
                        Bus.transform.Find("camera").GetComponent<Camera>().enabled = true;
                        Bus.transform.Find("camera").GetComponent<AudioListener>().enabled = true;
                        camara.GetComponent<Camera>().enabled = false;
                        camara.GetComponent<AudioListener>().enabled = false;
                        parentt.GetComponent<CarController>().enabled = false;
                        parentt.GetComponent<CarUserControl>().enabled = false;
                        //parentt.GetComponent<CarAudio>().enabled = false;
                        parentt.GetComponent<Rigidbody>().velocity = Vector3.zero;
                        parentt.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
                        GameObject go = GameObject.FindGameObjectWithTag("StaticInfos");
                        StaticInfos si = go.GetComponent<StaticInfos>();
                        si.setOnAiBus(true);
                        transform.parent.parent.parent.gameObject.SetActive(false);
                        panelController.closeAllPanels();
                        //si.setActualGas(Bus.gameObject.GetComponent<BusAi>().getFirstGas());
                        si.setActualAiGas(Bus.gameObject.GetComponent<BusAi>().getFirstGas());
                    }
                }
            }
        }
        else if (transform.parent.parent.parent.name.Equals("GasesList"))
        {
            StaticInfos si = GameObject.FindGameObjectWithTag("StaticInfos").GetComponent<StaticInfos>();
            if (si.isOnAiBusMode())
                si.setActualAiGas(GetComponentInChildren<Text>().text);
            else
                si.setActualGas(GetComponentInChildren<Text>().text);
            panelController.closeAllPanels();
        }
    }

}
