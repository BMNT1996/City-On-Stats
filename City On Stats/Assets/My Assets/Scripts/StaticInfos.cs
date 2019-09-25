using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticInfos : MonoBehaviour
{
    public static string objpath;
    public static string osmpath;
    public static string imgpath;
    public static double minLat;
    public static double maxLat;
    public static double minLon;
    public static double maxLon;
    public static double centerLat;
    public static double centerLon;
    public static bool PlayerViewActive = true;
    public static bool mode;
    public static string actualGas;
    public static string actualAIGas;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public string getObj()
    {
        return objpath;
    }

    public void setObj(string obj)
    {
        objpath = obj;
    }

    public string getOsm()
    {
        return osmpath;
    }

    public void setOsm(string osm)
    {
        osmpath = osm;
    }

    public string getImg()
    {
        return imgpath;
    }

    public void setImg(string img)
    {
        imgpath = img;
    }

    public double getMinLat()
    {
        return minLat;
    }

    public void setMinLat(double MinLat)
    {
        minLat = MinLat;
    }

    public double getMaxLat()
    {
        return maxLat;
    }

    public void setMaxLat(double MaxLat)
    {
        maxLat = MaxLat;
    }

    public double getMinLon()
    {
        return minLon;
    }

    public void setMinLon(double MinLon)
    {
        minLon = MinLon;
    }

    public double getMaxLon()
    {
        return maxLon;
    }

    public void setMaxLon(double MaxLon)
    {
        maxLon = MaxLon;
    }

    public double getCenterLat()
    {
        return centerLat;
    }

    public void setCenterLat(double CenterLat)
    {
        centerLat = CenterLat;
    }

    public double getCenterLon()
    {
        return centerLon;
    }

    public void setCenterLon(double CenterLon)
    {
        centerLon = CenterLon;
    }

    public bool isPlayerViewActive()
    {
        return PlayerViewActive;
    }

    public void setPlayerViewActive(bool isActive)
    {
        PlayerViewActive = isActive;
    }

    public bool isOnAiBusMode()
    {
        return mode;
    }

    public void setOnAiBus(bool onAiBus)
    {
        mode = onAiBus;
    }

    public string getActualGas()
    {
        return actualGas;
    }

    public void setActualGas(string Gas)
    {
        actualGas = Gas;
    }

    public string getActualAiGas()
    {
        return actualAIGas;
    }

    public void setActualAiGas(string Gas)
    {
        actualAIGas = Gas;
    }

}
