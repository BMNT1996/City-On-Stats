using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Vehicles.Car;

public class CursorListener : MonoBehaviour
{
    public GameObject panelController;
    public GameObject buses;
    public GameObject boarder;
    public GameObject visual;
    public GameObject panels;
    public GameObject busControl;
    public Window_Graph WG;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            if (!panelController.GetComponent<PanelController>().isPanelActive())
            {
                if (Camera.main != null)
                {
                    var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                    RaycastHit hit = new RaycastHit();

                    if (Physics.Raycast(ray, out hit))
                    {
                        double lat, lon;
                        GameObject go = GameObject.FindGameObjectWithTag("StaticInfos");
                        StaticInfos si = go.GetComponent<StaticInfos>();
                        lat = hit.point.z;
                        lon = hit.point.x;
                        double Radius = 6400;
                        double m = (1 / ((2 * Math.PI / 360) * Radius)) / 1000;
                        lat = si.getCenterLat() - (lat * m);
                        lon = si.getCenterLon() - (lon * m) / Math.Cos(si.getCenterLat() * (Math.PI / 180));
                        openTimeChart(hit.point, lat, lon);
                    }
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            try
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, 100))
                {
                    if (hit.transform.gameObject.name.Equals("Autocarro"))
                    {
                        bool proceed = true;

                        foreach (Transform t in panels.transform)
                        {
                            if (t.gameObject.active == true)
                            {
                                proceed = false;
                                break;
                            }
                        }

                        if (hit.transform.Find("camera").parent.gameObject != Camera.main.transform.parent.gameObject && proceed)
                        {
                            busControl.GetComponent<BusUserControl>().open(hit.transform.gameObject);
                            visual.GetComponent<VisualizationManager>().reset();
                            GameObject camara = Camera.main.gameObject;
                            GameObject parentt = camara.transform.parent.gameObject;
                            hit.transform.Find("camera").GetComponent<Camera>().enabled = true;
                            hit.transform.Find("camera").GetComponent<AudioListener>().enabled = true;
                            camara.GetComponent<Camera>().enabled = false;
                            camara.GetComponent<AudioListener>().enabled = false;
                            parentt.GetComponent<CarController>().enabled = false;
                            parentt.GetComponent<CarUserControl>().enabled = false;
                            parentt.GetComponent<CarAudio>().enabled = false;
                            parentt.GetComponent<Rigidbody>().velocity = Vector3.zero;
                            parentt.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
                            GameObject go = GameObject.FindGameObjectWithTag("StaticInfos");
                            StaticInfos si = go.GetComponent<StaticInfos>();
                            si.setOnAiBus(true);
                            si.setActualGas(hit.transform.gameObject.GetComponent<BusAi>().getFirstGas());
                        }
                    }

                    else if (hit.transform.gameObject.name.Equals("PlayerView"))
                    {
                        if (hit.transform.Find("camera").parent.gameObject != Camera.main.transform.parent.gameObject)
                        {
                            visual.GetComponent<VisualizationManager>().reset();
                            GameObject camara = Camera.main.gameObject;
                            hit.transform.Find("camera").GetComponent<Camera>().enabled = true;
                            hit.transform.Find("camera").GetComponent<AudioListener>().enabled = true;
                            hit.transform.gameObject.GetComponent<CarController>().enabled = true;
                            hit.transform.gameObject.GetComponent<CarUserControl>().enabled = true;
                            hit.transform.gameObject.GetComponent<CarAudio>().enabled = true;
                            camara.GetComponent<Camera>().enabled = false;
                            camara.GetComponent<AudioListener>().enabled = false;
                            GameObject go = GameObject.FindGameObjectWithTag("StaticInfos");
                            StaticInfos si = go.GetComponent<StaticInfos>();
                            si.setOnAiBus(false);
                        }
                    }

                    else if (hit.transform.gameObject.name.Contains("Path:"))
                    {
                        foreach (Transform Bus in buses.transform)
                        {
                            if (Bus.gameObject.active)
                            {
                                String busName = hit.transform.gameObject.name;
                                if (Bus.gameObject.GetComponent<BusAi>().getName().Equals(busName.Replace("Path:", "")))
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
                                    parentt.GetComponent<CarAudio>().enabled = false;
                                    parentt.GetComponent<Rigidbody>().velocity = Vector3.zero;
                                    parentt.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
                                    GameObject go = GameObject.FindGameObjectWithTag("StaticInfos");
                                    StaticInfos si = go.GetComponent<StaticInfos>();
                                    si.setOnAiBus(true);
                                    si.setActualGas(Bus.gameObject.GetComponent<BusAi>().getFirstGas());
                                }
                            }
                        }
                    }

                    else
                    {
                        Debug.Log(hit.transform.gameObject.name);
                    }
                }
            }
            catch (NullReferenceException e)
            {
                Debug.Log(e.ToString());
            }
        }
    }

    private void openTimeChart(Vector3 position, double lat, double lon)
    {
        updateGraphValues(lat, lon);
        boarder.transform.position = new Vector3(position.x, 0, position.z);
        boarder.GetComponent<BoardController>().updateImage(lat, lon);
    }

    private void updateGraphValues(double lat, double lon)
    {
        System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
        String path = Application.dataPath;
        path += "/Resources/data.csv";
        List<Dictionary<string, object>> data = CSVReader.Read(path);

        double[] values = new double[24];
        int[] count = new int[24];

        StaticInfos si = GameObject.FindGameObjectWithTag("StaticInfos").GetComponent<StaticInfos>();
        if (data.Count != 0 && si.getActualGas() != null && !si.getActualGas().Equals(""))
        {
            foreach (Dictionary<string, object> dict in data)
            {
                double latf = double.Parse(dict["lat"].ToString().Replace(".", ","));
                double lonf = double.Parse(dict["lon"].ToString().Replace(".", ","));
                if (Math.Abs(lat - latf) < 0.0001 && Math.Abs(lon - lonf) < 0.00015)
                {
                    int hour = dtDateTime.AddSeconds(double.Parse(dict["time"].ToString().Replace(".", ","))).ToLocalTime().Hour;
                    values[hour] += double.Parse(dict[si.getActualGas()].ToString().Replace(".", ","));
                    count[hour]++;
                }
            }
            values = smooth(values, count);

            //Normal
            /*for (int i=0; i < 24; i++)
            {
                WG.UpdateValue(i, (int)values[i]);
            }*/

            //Test only
            values = new double[24] { 11, 10, 9, 8, 7, 5, 7, 7.5, 8, 8.5, 9, 9.5, 10, 10.5, 11, 12, 13, 14, 15, 15, 15, 14, 13, 12 };
            for (int i = 0; i < 24; i++)
            {
                WG.UpdateValue(i, (int)values[i]);
            }

        }

    }

    private double[] smooth(double[] values, int[] count)
    {
        double[] result = new double[24];

        for (int i = 0; i < 24; i++)
        {
            result[i] = values[i] / count[i];
        }

        for (int j = 0; j < 24; j++)
        {
            if (count[j] == 0)
            {
                double down = 0, up = 0;
                int downi = -1, upi = -1;
                for (int k = 1; k < 24; k++)
                {
                    Debug.Log(((j - k) % 24 + 24) % 24);
                    if (downi == -1 && count[((j - k) % 24 + 24) % 24] != 0)
                    {
                        down = result[((j - k) % 24 + 24) % 24];
                        downi = k;
                    }
                    Debug.Log(((j + k) % 24 + 24) % 24);
                    if (upi == -1 && count[((j + k) % 24 + 24) % 24] != 0)
                    {
                        up = result[((j + k) % 24 + 24) % 24];
                        upi = k;
                    }
                    if (downi != -1 && upi != -1)
                        break;
                }
                if (downi == -1 && upi == -1)
                    break;
                else
                {
                    result[j] = (((double)down) * ((double)upi / ((double)(downi + upi)))) + (((double)up) * ((double)downi / ((double)(downi + upi))));
                }
            }

        }


        return result;
    }
}
