using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using static DataColorHandler;

public class BusGenerator : MonoBehaviour
{
    public class bus
    {
        string name;
        string start;
        string end;
        GameObject path;

        public bus(string name, GameObject path, string start, string end)
        {
            this.name = name;
            this.path = path;
            this.start = start;
            this.end = end;
        }

        public string getName()
        {
            return name;
        }

        public GameObject getPath()
        {
            return path;
        }

        public string getStart()
        {
            return start;
        }

        public string getEnd()
        {
            return end;
        }

    }

    public GameObject paths;
    public GameObject buses;
    public GameObject BusClone;
    public GameObject SphereModel;
    public GameObject streetHandler;

    ArrayList busList = new ArrayList();

    // Start is called before the first frame update
    void Start()
    {
        readXML();
        generateBuses();
    }

    private void generateBuses()
    {
        foreach (bus Bus in busList)
        {
            var copy = Instantiate(BusClone);
            copy.transform.parent = buses.transform;
            copy.name = "Autocarro";
            copy.GetComponent<BusAi>().setName(Bus.getName());
            copy.GetComponent<BusAi>().setPath(Bus.getPath().transform);
            copy.GetComponent<BusAi>().setStartnEnd(Bus.getStart(), Bus.getEnd());
            copy.SetActive(true);
        }
    }

    private Vector3 generateNodePosition(double lat, double lon)
    {
        GameObject go = GameObject.FindGameObjectWithTag("StaticInfos");
        StaticInfos si = go.GetComponent<StaticInfos>();

        double R = 6400;
        double dLat = lat * Math.PI / 180 - si.getCenterLat() * Math.PI / 180;
        double dLon = lon * Math.PI / 180 - si.getCenterLon() * Math.PI / 180;
        double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2);
        double b = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        double latitude = R * b * 1000;

        a = Math.Cos(si.getCenterLat() * Math.PI / 180) * Math.Cos(lat * Math.PI / 180) * Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
        b = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        double longitude = R * b * 1000;

        if (si.getCenterLat() - lat < 0)
        {
            latitude *= -1;
        }

        if (si.getCenterLon() - lon < 0)
        {
            longitude *= -1;
        }

        return new Vector3((float)longitude, 1, (float)latitude);
    }

    private void readXML()
    {
        XmlDocument xmlDoc = new XmlDocument();
        String path = Application.dataPath;
        path += "/Resources/Bus.xml";
        if (System.IO.File.Exists(path))
        {
            xmlDoc.LoadXml(System.IO.File.ReadAllText(path));
            XmlNode nodeBase = xmlDoc.SelectSingleNode("BUSES");
            string name = "";

            /*foreach (XmlElement busAux in nodeBase.SelectNodes("bus"))
            {
                name = busAux.GetAttribute("name");
                GameObject pathNode = new GameObject();
                pathNode.name = name;
                pathNode.AddComponent<PathPoints>();
                foreach(XmlElement node in busAux.SelectSingleNode("path").SelectNodes("node"))
                {
                    GameObject aux = new GameObject();
                    aux.transform.position = generateNodePosition(Double.Parse(node.GetAttribute("lat").Replace(".",",")), Double.Parse(node.GetAttribute("lon").Replace(".", ",")));
                    aux.transform.parent = pathNode.transform;
                }
                busList.Add(new bus(name, pathNode));
            }*/

            DataColorHandler DCH = GameObject.FindGameObjectWithTag("DataColorHandler").GetComponent<DataColorHandler>();

            XmlElement color = ((XmlElement)nodeBase.SelectSingleNode("color"));
            string[] colormin = color.GetAttribute("min").Split(',');
            string[] colorave = color.GetAttribute("ave").Split(',');
            string[] colormax = color.GetAttribute("max").Split(',');

            Color cmin = new Color(float.Parse(colormin[0]), float.Parse(colormin[1]), float.Parse(colormin[2]));
            Color cave = new Color(float.Parse(colorave[0]), float.Parse(colorave[1]), float.Parse(colorave[2]));
            Color cmax = new Color(float.Parse(colormax[0]), float.Parse(colormax[1]), float.Parse(colormax[2]));

            foreach (XmlElement colorCode in nodeBase.SelectNodes("colorcode"))
            {
                DataColorGas DCG = new DataColorGas(colorCode.GetAttribute("name"), Double.Parse(colorCode.GetAttribute("min").Replace(".", ",")), Double.Parse(colorCode.GetAttribute("ave").Replace(".", ",")), Double.Parse(colorCode.GetAttribute("max").Replace(".", ",")), cmin, cave, cmax);
                DCH.setGas(DCG);
            }



            foreach (XmlElement busAux in nodeBase.SelectNodes("bus"))
            {
                name = busAux.GetAttribute("name");
                GameObject pathNode = new GameObject();
                GameObject go = GameObject.FindGameObjectWithTag("StaticInfos");
                StaticInfos si = go.GetComponent<StaticInfos>();
                pathNode.name = name;
                pathNode.AddComponent<PathPoints>();
                ArrayList nodesFromXML = new ArrayList();
                ArrayList gasesFromXML = new ArrayList();
                foreach (XmlElement node in busAux.SelectSingleNode("path").SelectNodes("node"))
                {
                    double lat = Double.Parse(node.GetAttribute("lat").Replace(".", ","));
                    double lon = Double.Parse(node.GetAttribute("lon").Replace(".", ","));
                    if (lat < 0.002 + si.getMaxLat() && lat > si.getMinLat() - 0.002 && lon < 0.002 + si.getMaxLon() && lon > si.getMinLon() - 0.002)
                    {
                        nodesFromXML.Add(generateNodePosition(lat, lon));
                        Dictionary<string, double> dicaux = new Dictionary<string, double>();
                        foreach (XmlElement gas in node.SelectNodes("gas"))
                        {
                            dicaux.Add(gas.GetAttribute("name"), Double.Parse(gas.GetAttribute("value").Replace(".", ",")));
                        }
                        gasesFromXML.Add(dicaux);
                    }
                }
                Dictionary<string, double> comolativeDict = new Dictionary<string, double>();
                try
                {
                    foreach (string key in ((Dictionary<string, double>)gasesFromXML[0]).Keys)
                    {
                        comolativeDict.Add(key, 0);
                    }
                }
                catch (ArgumentOutOfRangeException e)
                {

                }
                if (nodesFromXML.Count != 0)
                {
                    string start = streetHandler.GetComponent<StreetsHandler>().calculateClosestStreet(((Vector3)nodesFromXML[0]));
                    string end = streetHandler.GetComponent<StreetsHandler>().calculateClosestStreet(((Vector3)nodesFromXML[nodesFromXML.Count - 1]));
                    var copy2 = Instantiate(SphereModel);
                    copy2.transform.position = ((Vector3)nodesFromXML[0]);
                    copy2.transform.parent = pathNode.transform;
                    copy2.name = "Path:" + name;
                    copy2.SetActive(true);
                    copy2.AddComponent<NodePath>().addGases(((Dictionary<string, double>)gasesFromXML[0]));
                    var lastNode = copy2;
                    double comolativeDist = 0;
                    for (int i = 0; i < nodesFromXML.Count; i++)
                    {
                        Vector3 vector = Vector3.zero;
                        if (i < nodesFromXML.Count - 1)
                        {
                            vector = ((Vector3)nodesFromXML[i + 1]) - ((Vector3)nodesFromXML[i]);
                            double scale = (vector.magnitude / 50); //Space of 25 meters
                            comolativeDist += scale;
                            foreach (string key in ((Dictionary<string, double>)gasesFromXML[i]).Keys)
                            {
                                if (comolativeDist <= 1)
                                {
                                    comolativeDict[key] += (scale * ((Dictionary<string, double>)gasesFromXML[i])[key]);
                                }
                                else
                                {
                                    comolativeDict[key] += ((-(comolativeDist - 1 - scale)) * ((Dictionary<string, double>)gasesFromXML[i])[key]);
                                }
                            }
                            if (comolativeDist >= 1)
                            {
                                while (comolativeDist >= 1)
                                {
                                    var copy = Instantiate(SphereModel);
                                    copy.transform.position = ((Vector3)nodesFromXML[i]) + (vector * ((float)(((-(comolativeDist - 1 - scale)) / (scale)))));
                                    copy.transform.parent = pathNode.transform;
                                    copy.name = "Path:" + name;
                                    copy.SetActive(true);
                                    copy.AddComponent<NodePath>().addGases(comolativeDict);
                                    lastNode = copy;
                                    comolativeDist -= 1;
                                    if (comolativeDist < 1)
                                    {
                                        Dictionary<string, double> auxdict = new Dictionary<string, double>();
                                        foreach (string key in comolativeDict.Keys)
                                        {
                                            auxdict.Add(key, (comolativeDist * ((Dictionary<string, double>)gasesFromXML[i])[key]));
                                        }
                                        comolativeDict.Clear();
                                        comolativeDict = auxdict;
                                    }
                                    else
                                    {
                                        Dictionary<string, double> auxdict = new Dictionary<string, double>();
                                        foreach (string key in comolativeDict.Keys)
                                        {
                                            auxdict.Add(key, ((1 - ((scale - comolativeDist) / scale)) * ((Dictionary<string, double>)gasesFromXML[i])[key]) + (((scale - comolativeDist) / scale) * ((Dictionary<string, double>)gasesFromXML[i + 1])[key]));
                                        }
                                        comolativeDict.Clear();
                                        comolativeDict = auxdict;
                                    }
                                }
                            }
                        }

                        else
                        {
                            var copy = Instantiate(SphereModel);
                            copy.transform.position = ((Vector3)nodesFromXML[i]);
                            copy.transform.parent = pathNode.transform;
                            copy.name = "Path:" + name;
                            copy.SetActive(true);
                            copy.AddComponent<NodePath>().addGases(((Dictionary<string, double>)gasesFromXML[i]));
                            lastNode = copy;
                        }
                    }
                    busList.Add(new bus(name, pathNode, start, end));
                }
            }

            /*
            XmlNode nodeBase = xmlDoc.SelectSingleNode("osm");
            ArrayList nodes = new ArrayList();
            Dictionary<long, Coordenates> dicaux = new Dictionary<long, Coordenates>();

            foreach (XmlElement node in nodeBase.SelectNodes("node"))
            {
                long idnode = long.Parse(node.GetAttribute("id"));
                Coordenates c = new Coordenates(double.Parse(node.GetAttribute("lat").Replace(".", ",")), double.Parse(node.GetAttribute("lon").Replace(".", ",")));
                dicaux.Add(idnode, c);
            }

            XmlNodeList nodeList = nodeBase.SelectNodes("way");
            Boolean proceed = false;
            foreach (XmlElement node in nodeList)
            {
                Boolean proceeds = false;
                string name = "";
                foreach (XmlElement tag in node.SelectNodes("tag"))
                {
                    if (tag.GetAttribute("k").Equals("name"))
                    {
                        name = tag.GetAttribute("v");
                        proceeds = true;
                        break;
                    }

                }

                if (proceeds)
                {
                    ArrayList coordenatesArray = new ArrayList();
                    foreach (XmlElement nd in node.SelectNodes("nd"))
                    {
                        coordenatesArray.Add((Coordenates)dicaux[long.Parse(nd.GetAttribute("ref"))]);
                    }
                    StreetList.Add(new StreetCoordenates(name, coordenatesArray));
                }
            }
        }
        else
        {
            Debug.Log("Cannot load file");
        }

    */
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public ArrayList getBusList()
    {
        return busList;
    }
}
