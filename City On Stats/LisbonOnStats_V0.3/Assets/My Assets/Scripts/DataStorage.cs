using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;
using static DataColorHandler;

public class DataStorage : MonoBehaviour
{
    public class MapPoint
    {
        Vector2 position;
        Dictionary<string, double> gases = new Dictionary<string, double>();
        int n = 1;

        public MapPoint(Vector2 position, Dictionary<string, double> gases)
        {
            this.position = position;
            this.gases = gases;
        }

        public Vector2 GetVector()
        {
            return position;
        }

        public double getGas(string name)
        {
            if (gases.ContainsKey(name))
                return gases[name];
            else
                return -1;
        }

        public void addEntry(Dictionary<string, double> gases)
        {
            n++;

            Dictionary<string, double> aux = new Dictionary<string, double>();

            foreach (string key in gases.Keys)
            {
                aux.Add(key, (gases[key] + ((1 - n) * this.gases[key])) / n);
            }

            this.gases = aux;
        }
    }

    Dictionary<Vector2, MapPoint> mapPoints;
    DataColorHandler DCH = new DataColorHandler();

    // Start is called before the first frame update
    void Start()
    {
        mapPoints = new Dictionary<Vector2, MapPoint>();
        setDataColor();
        String path = Application.dataPath;
        path += "/Resources/data.csv";
        List<Dictionary<string, object>> data = CSVReader.Read(path);
        loadData(data);
    }

    private void setDataColor()
    {
        XmlDocument xmlDoc = new XmlDocument();
        String path = Application.dataPath;
        path += "/Resources/Configs.xml";
        if (System.IO.File.Exists(path))
        {
            xmlDoc.LoadXml(System.IO.File.ReadAllText(path));
            XmlNode nodeBase = xmlDoc.SelectSingleNode("CONFIGURATIONS");
            string name = "";
            XmlElement color = ((XmlElement)nodeBase.SelectSingleNode("color"));
            string[] colormin = color.GetAttribute("min").Split(',');
            string[] colorave = color.GetAttribute("ave").Split(',');
            string[] colormax = color.GetAttribute("max").Split(',');

            Color cmin = new Color(float.Parse(colormin[0]), float.Parse(colormin[1]), float.Parse(colormin[2]));
            Color cave = new Color(float.Parse(colorave[0]), float.Parse(colorave[1]), float.Parse(colorave[2]));
            Color cmax = new Color(float.Parse(colormax[0]), float.Parse(colormax[1]), float.Parse(colormax[2]));

            foreach (XmlElement colorCode in nodeBase.SelectNodes("gas"))
            {
                DataColorGas DCG = new DataColorGas(colorCode.GetAttribute("name"), Double.Parse(colorCode.GetAttribute("min").Replace(".", ",")), Double.Parse(colorCode.GetAttribute("ave").Replace(".", ",")), Double.Parse(colorCode.GetAttribute("max").Replace(".", ",")), cmin, cave, cmax);
                DCH.setGas(DCG);
            }
        }
    }

    private void loadData(List<Dictionary<string, object>> list)
    {
        if (list.Count != 0 && DCH.getGasNames().Length != 0)
        {
            ArrayList header = new ArrayList(list[0].Keys);
            header.Remove("time");
            header.Remove("");
            header.Remove("lat");
            header.Remove("lon");
            header.Remove("eletrico");

            foreach (Dictionary<string, object> dict in list)
            {
                Vector2 coordinates = generateVec2Int(Double.Parse(dict["lat"].ToString().Replace(".", ",")), Double.Parse(dict["lon"].ToString().Replace(".", ","))); ;
                Dictionary<string, double> dictaux = new Dictionary<string, double>();
                foreach (string key in header)
                {
                    dictaux.Add(key, Double.Parse(dict[key].ToString().Replace(".", ",")));
                }

                if (mapPoints.ContainsKey(coordinates))
                    mapPoints[coordinates].addEntry(dictaux);
                else
                    mapPoints.Add(coordinates, new MapPoint(coordinates, dictaux));
            }
        }
    }

    private void readCSV()
    {
        StaticInfos si = GameObject.FindGameObjectWithTag("StaticInfos").GetComponent<StaticInfos>();
        String path = Application.dataPath;
        path += "/Resources/data.csv";
        FileInfo file = new FileInfo(path);
        StreamReader reader = file.OpenText();
        string text = reader.ReadLine();
        string[] header = text.Split(',');
        string[] line;
        text = reader.ReadLine();
        while (text != null)
        {
            line = text.Split(',');
            double lat = Double.Parse(line[1].Replace(".", ","));
            double lon = Double.Parse(line[2].Replace(".", ","));
            if (lat < 0.002 + si.getMaxLat() && lat > si.getMinLat() - 0.002 && lon < 0.002 + si.getMaxLon() && lon > si.getMinLon() - 0.002)
            {
                Dictionary<string, double> gases = new Dictionary<string, double>();
                for (int i = 4; i < line.Length; i++)
                {
                    gases.Add(header[i], Double.Parse(line[i].Replace(".", ",")));
                }
                Vector2 point = generateVec2Int(lat, lon);

                if (!mapPoints.ContainsKey(point))
                    mapPoints.Add(point, new MapPoint(point, gases));
                else
                    mapPoints[point].addEntry(gases);
            }
        }

    }

    private Vector2 generateVec2Int(double lat, double lon)
    {
        StaticInfos si = GameObject.FindGameObjectWithTag("StaticInfos").GetComponent<StaticInfos>();
        if (si.getCenterLat() == 0 || si.getCenterLon() == 0)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(System.IO.File.ReadAllText(si.getOsm()));
            XmlNode nodeBase = xmlDoc.SelectSingleNode("osm");
            foreach (XmlElement node in nodeBase.SelectNodes("bounds"))
            {
                si.setMinLat(double.Parse(node.GetAttribute("minlat").Replace(".", ",")));
                si.setMaxLat(double.Parse(node.GetAttribute("maxlat").Replace(".", ",")));
                si.setMinLon(double.Parse(node.GetAttribute("minlon").Replace(".", ",")));
                si.setMaxLon(double.Parse(node.GetAttribute("maxlon").Replace(".", ",")));
                si.setCenterLat((si.getMaxLat() + si.getMinLat()) / 2);
                si.setCenterLon((si.getMaxLon() + si.getMinLon()) / 2);
                break;
            }
        }
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
        return new Vector2((Mathf.RoundToInt((float)latitude)), (Mathf.RoundToInt((float)longitude)));
    }

    public double getGasValue(string gas, Vector2 point)
    {
        double bestDist = 500;
        double value = 0;

        foreach (Vector2 v in mapPoints.Keys)
        {
            if (Vector2.Distance(point, v) < bestDist)
            {
                bestDist = Vector2.Distance(point, v);
                value = mapPoints[v].getGas(gas);
            }
        }
        return value;
    }

    public ArrayList getClosestPoints(Vector2 point)
    {
        ArrayList result = new ArrayList();

        foreach (Vector2 p in mapPoints.Keys)
        {
            if (Vector2.Distance(point, p) < 100)
            {
                result.Add(mapPoints[p]);
            }

        }

        return result;
    }

    public object[] getNormAndColor(string gas, double val)
    {
        return new object[] { DCH.getNormValue(gas, val), DCH.getGasColor(gas, val) };
    }

    public double getNormValue(string gas, Vector2 point)
    {
        return DCH.getNormValue(gas, getGasValue(gas, point));
    }

    public Color getColorValue(string gas, Vector2 point)
    {
        return DCH.getGasColor(gas, getGasValue(gas, point));
    }

    public string[] getGasList()
    {
        return DCH.getGasNames();
    }
    // Update is called once per frame
    void Update()
    {

    }
}
