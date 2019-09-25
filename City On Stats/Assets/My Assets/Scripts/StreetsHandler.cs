using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;
using UnityEngine.UI;
using static StreetsHandler.StreetCoordenates;

public class StreetsHandler : MonoBehaviour
{
    public class StreetCoordenates
    {
        public class Coordenates
        {
            double lat, lon;

            public Coordenates(double lat, double lon)
            {
                this.lat = lat;
                this.lon = lon;
            }

            public double getLatitude()
            {
                return lat;
            }

            public double getLongitude()
            {
                return lon;
            }
        }
        string name;
        ArrayList coordenates;

        public StreetCoordenates(string name, ArrayList coordenates)
        {
            this.name = name;
            this.coordenates = coordenates;
            //this.lat = lat;
            //this.lon = lon;
        }

        public string getStreetName()
        {
            return name;
        }

        public double bestDistance(double[] localCoordenate)
        {
            double d = 9999;
            double td;
            foreach (Coordenates c in coordenates)
            {
                td = calculateDistance(localCoordenate[0], c.getLatitude(), localCoordenate[1], c.getLongitude());
                if (d > td)
                {
                    d = td;
                }
            }

            return d;
        }

        private double calculateDistance(double lat1, double lat2, double lon1, double lon2)
        {
            //double R = 6378.137;
            double R = 6400;
            double dLat = lat2 * Math.PI / 180 - lat1 * Math.PI / 180;
            double dLon = lon2 * Math.PI / 180 - lon1 * Math.PI / 180;
            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) + Math.Cos(lat1 * Math.PI / 180) * Math.Cos(lat2 * Math.PI / 180) * Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            double D = R * c * 1000;

            return D;
        }

        public ArrayList getCoorenates()
        {
            return coordenates;
        }
    }

    class nodeSupport
    {
        int id;
        double lat;
        double lon;

        public nodeSupport(int id, double lat, double lon)
        {
            this.id = id;
            this.lat = lat;
            this.lon = lon;
        }

        public int getId()
        {
            return id;
        }

        public double getLat()
        {
            return lat;
        }

        public double getLon()
        {
            return lon;
        }
    }

    private ArrayList StreetList;
    private double nextTime;
    public GameObject player;
    public GameObject camera;
    public Text CoordenatesText;

    void Start()
    {
        //loadStreets();
        loadXMLStreets();
        //double[] aux = cartesiansToCoordenates();
        nextTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= nextTime)
        {
            calculateClosestStreet();
            nextTime += 1;
        }
    }

    void loadXMLStreets()
    {
        StreetList = new ArrayList();
        XmlDocument xmlDoc = new XmlDocument();
        GameObject go = GameObject.FindGameObjectWithTag("StaticInfos");
        StaticInfos si = go.GetComponent<StaticInfos>();
        if (System.IO.File.Exists(si.getOsm()))
        {
            xmlDoc.LoadXml(System.IO.File.ReadAllText(si.getOsm()));
            XmlNode nodeBase = xmlDoc.SelectSingleNode("osm");
            ArrayList nodes = new ArrayList();
            /*foreach (XmlNode node in nodeBase.ChildNodes)
            {
                nodes.Add(node);

                if (node.HasChildNodes)
                {
                    XmlNodeList nodeList = node.SelectNodes("tag");
                    if (nodeList != null)
                    {
                        foreach (XmlElement tnode in nodeList)
                        {
                            if (tnode.GetAttribute("k") == "addr:place" || tnode.GetAttribute("k") == "name")
                            {
                                ArrayList sc = new ArrayList();
                                foreach (XmlElement enode in nodes)
                                {
                                    if (!enode.GetAttribute("lat").Equals("")) { 
                                        double lat = double.Parse(enode.GetAttribute("lat").Replace(".", ","));
                                        double lon = double.Parse(enode.GetAttribute("lon").Replace(".", ","));
                                        sc.Add(new Coordenates(lat, lon));
                                    }
                                }
                                StreetList.Add(new StreetCoordenates(tnode.GetAttribute("v"), sc));
                                Debug.Log(tnode.GetAttribute("v"));
                            }
                        }
                        nodes = new ArrayList();
                    }
                    else
                    {
                        nodes = new ArrayList();
                    }
                }
                else
                {
                    nodes = new ArrayList();
                }
            }*/

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
                        if (dicaux.ContainsKey(long.Parse(nd.GetAttribute("ref"))))
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


    }



    public double[] cartesiansToCoordenates(GameObject Player)
    {
        double lat, lon;
        GameObject go = GameObject.FindGameObjectWithTag("StaticInfos");
        StaticInfos si = go.GetComponent<StaticInfos>();
        lat = Player.transform.position.z;
        lon = Player.transform.position.x;
        double Radius = 6400;
        double m = (1 / ((2 * Math.PI / 360) * Radius)) / 1000;
        lat = si.getCenterLat() - (lat * m);
        lon = si.getCenterLon() - (lon * m) / Math.Cos(si.getCenterLat() * (Math.PI / 180));
        return new double[] { lat, lon };
    }

    private void loadStreets()
    {
        GameObject go = GameObject.FindGameObjectWithTag("StaticInfos");
        StaticInfos si = go.GetComponent<StaticInfos>();
        StreamReader reader = new StreamReader(si.getOsm());
        StreetList = new ArrayList();
        string line;
        string[] aux;
        string[] coordenates = null;
        string street = "";

        do
        {
            line = reader.ReadLine();
            if (line == null)
                break;
            if (line.Contains("node id") && line.Contains("lat") && line.Contains("lon"))
            {
                aux = line.Split('"');
                coordenates = new String[2];
                coordenates[0] = aux[15];
                coordenates[0] = coordenates[0].Replace(".", ",");
                coordenates[1] = aux[17];
                coordenates[1] = coordenates[1].Replace(".", ",");
            }
            if (line.Contains("addr:street"))
            {
                aux = line.Split('"');
                street = aux[3];
            }
            if (coordenates != null && !street.Equals(""))
            {
                //StreetList.Add(new StreetCoordenates(street, double.Parse(coordenates[0]), double.Parse(coordenates[1])));
                coordenates = null;
                street = "";
            }
        } while (line != null);
    }

    public void calculateClosestStreet()
    {
        CoordenatesText.text = calculateClosestStreet(player.transform.position);
    }

    public String calculateClosestStreet(Vector3 position)
    {
        string closestStreet = "";
        double closestDistance = 9999;
        double distance;
        GameObject go = GameObject.FindGameObjectWithTag("StaticInfos");
        StaticInfos si = go.GetComponent<StaticInfos>();
        GameObject aux = new GameObject();
        aux.transform.position = position;
        if (si.isPlayerViewActive())
        {
            foreach (StreetCoordenates sc in StreetList)
            {
                distance = sc.bestDistance(cartesiansToCoordenates(aux));
                if (distance < closestDistance)
                {
                    closestStreet = sc.getStreetName();
                    closestDistance = distance;
                }
            }
        }
        else
        {
            foreach (StreetCoordenates sc in StreetList)
            {
                distance = sc.bestDistance(cartesiansToCoordenates(camera));
                if (distance < closestDistance)
                {
                    closestStreet = sc.getStreetName();
                    closestDistance = distance;
                }
            }
        }
        //CoordenatesText.text = "Closest registered Street:" + System.Environment.NewLine + closestStreet;
        UnityEngine.Object.Destroy(aux);
        return closestStreet;
    }

    private double calculateDistance(double lat1, double lat2, double lon1, double lon2)
    {
        //double R = 6378.137;
        double R = 6400;
        double dLat = lat2 * Math.PI / 180 - lat1 * Math.PI / 180;
        double dLon = lon2 * Math.PI / 180 - lon1 * Math.PI / 180;
        double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) + Math.Cos(lat1 * Math.PI / 180) * Math.Cos(lat2 * Math.PI / 180) * Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
        double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        double D = R * c * 1000;

        return D;
    }

    public ArrayList getStreetList()
    {
        return StreetList;
    }

}
