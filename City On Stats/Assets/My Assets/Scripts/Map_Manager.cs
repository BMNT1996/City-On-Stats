using Dummiesman;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map_Manager : MonoBehaviour
{
    Vector3 ClosestRoadToZero;
    // Start is called before the first frame update
    void Start()
    {
        ClosestRoadToZero = new Vector3(9999, 9999, 9999);
        GameObject obj = GameObject.FindGameObjectWithTag("StaticInfos");
        string path = obj.GetComponent<StaticInfos>().getObj();
        var loadedObj = new OBJLoader().Load(path);
        foreach (Transform objecto in loadedObj.GetComponentsInChildren<Transform>())
        {
            if (!objecto.name.Contains("Rail"))
                objecto.gameObject.AddComponent<MeshCollider>();
            if (objecto.name.Contains("Surface"))
                objecto.position = new Vector3(0, -0.001f, 0);
            if (objecto.name.Contains("Road "))
            {
                objecto.position = new Vector3(0, 0.001f, 0);
                double minx = objecto.GetComponent<Renderer>().bounds.max.x - objecto.GetComponent<Renderer>().bounds.min.x;
                double minz = objecto.GetComponent<Renderer>().bounds.max.z - objecto.GetComponent<Renderer>().bounds.min.z;
                if (minx > 100 && minz > 100 && Vector3.Distance(Vector3.zero, objecto.GetComponent<Renderer>().bounds.center) < Vector3.Distance(Vector3.zero, ClosestRoadToZero))
                {
                    ClosestRoadToZero = new Vector3(objecto.GetComponent<Renderer>().bounds.center.x, 5, objecto.GetComponent<Renderer>().bounds.center.z);
                }
            }
            if (objecto.name.Contains("Tunnel"))
                objecto.gameObject.SetActive(false);
        }

        loadedObj.tag = "3DMAP";
        GameObject go = GameObject.FindGameObjectWithTag("StaticInfos");
        StaticInfos si = go.GetComponent<StaticInfos>();
        si.setPlayerViewActive(true);
        //var loadedObj = new OBJLoader().Load(path);
    }

    public Vector3 getClosestRoad()
    {
        return ClosestRoadToZero;
    }
    // Update is called once per frame
    void Update()
    {

    }
}
