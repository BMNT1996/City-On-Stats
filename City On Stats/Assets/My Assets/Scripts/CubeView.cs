using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using static DataStorage;

public class CubeView : MonoBehaviour
{
    public GameObject cube;
    public GameObject parent;

    private Boolean isColorActive;
    private double nextTime;
    public DataStorage DS;
    public Transform Player;

    // Start is called before the first frame update
    void Start()
    {
        generateCubes();
        isColorActive = true;
        nextTime = Time.time;
    }

    private void generateCubes()
    {
        for (int y = -4; y <= 4; y++)
        {
            for (int x = -4; x <= 4; x++)
            {
                var copy = Instantiate(cube);
                copy.transform.parent = parent.transform;
                copy.transform.localPosition = new Vector3(4 * x, 0, 4 * y);
                copy.name = x.ToString() + "," + y.ToString();
            }
        }

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void activateHeightMode()
    {
        isColorActive = false;
    }

    public void activateColorMode()
    {
        isColorActive = true;
    }

    public void updateValues()
    {
        /*ArrayList points = DS.getClosestPoints(new Vector2(Player.position.z,Player.position.x));

        if (points.Count != 0) {
            StaticInfos si = GameObject.FindGameObjectWithTag("StaticInfos").GetComponent<StaticInfos>();
            foreach (Transform t in transform) {
                MapPoint mp = (MapPoint) points[0];
                double d = 100;
                foreach(MapPoint MP in points)
                {
                    if(Vector2.Distance(new Vector2(t.position.z, t.position.x), MP.GetVector()) < d)
                    {
                        mp = MP;
                        d = Vector2.Distance(new Vector2(t.position.z, t.position.x), MP.GetVector());
                    }
                }
                if (isColorActive)
                {
                    t.gameObject.GetComponent<CubeControl>().updateColor(DS.getColorValue(si.getActualGas(), mp.GetVector()));
                    //t.gameObject.GetComponent<CubeControl>().updateColor(DS.getColorValue(si.getActualGas(), new Vector2(t.position.z,t.position.x)));
                }
                else
                {
                    t.gameObject.GetComponent<CubeControl>().updateHeight(DS.getNormValue(si.getActualGas(), mp.GetVector()));
                    //t.gameObject.GetComponent<CubeControl>().updateHeight(DS.getNormValue(si.getActualGas(), new Vector2(t.position.z, t.position.x)));
                }
            }
        }*/
        StaticInfos si = GameObject.FindGameObjectWithTag("StaticInfos").GetComponent<StaticInfos>();
        foreach (Transform t in transform)
        {
            if (isColorActive)
            {
                //t.gameObject.GetComponent<CubeControl>().updateColor(DS.getColorValue(si.getActualGas(), mp.GetVector()));
                t.gameObject.GetComponent<CubeControl>().updateColor(DS.getColorValue(si.getActualGas(), new Vector2(t.position.z, t.position.x)));
            }
            else
            {
                //t.gameObject.GetComponent<CubeControl>().updateHeight(DS.getNormValue(si.getActualGas(), mp.GetVector()));
                t.gameObject.GetComponent<CubeControl>().updateHeight(DS.getNormValue(si.getActualGas(), new Vector2(t.position.z, t.position.x)));
            }
        }

    }

    public void resetAndSetMode(bool isColorActive)
    {
        foreach (Transform objecto in transform)
        {
            objecto.gameObject.GetComponent<CubeControl>().resetCube();
            isColorActive = true;
        }
    }
}
