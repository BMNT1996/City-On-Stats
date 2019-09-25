using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;

public class MiniMap : MonoBehaviour
{
    public Transform player;

    public GameObject plane;


    Texture2D texture;
    private void Start()
    {
        GameObject go = GameObject.FindGameObjectWithTag("StaticInfos");
        string img = go.GetComponent<StaticInfos>().getImg();
        string osm = go.GetComponent<StaticInfos>().getOsm();
        if (!img.Equals("(Not mandatory) Select an image file") && !osm.Equals("Select an .osm file"))
        {
            Vector3 newPosition = player.position;
            newPosition.y = -20;
            transform.position = newPosition;

            double[] dimentions = getMapDimensions(osm);

            double lat = dimentions[0] / 10;
            double lon = dimentions[1] / 10;

            plane.transform.localScale = new Vector3((float)lon, 1, (float)lat);

            byte[] fileData = File.ReadAllBytes(img);
            texture = new Texture2D(9600, 5400);
            texture.LoadImage(fileData);
            Material material = new Material(Shader.Find("Diffuse"));
            material.mainTexture = texture;
            plane.GetComponent<Renderer>().material = material;
        }
        else
        {
            getMapDimensions(osm);
        }
    }

    private double[] getMapDimensions(string osm)
    {
        /*StreamReader reader = new StreamReader(osm);

        string text;

        do
        {
            text = reader.ReadLine();

            if (text.Contains("bounds minlat=") || text.Contains("bounds minlon="))
            {
                break;
            }
        } while (text != null);

        reader.Close();

        string minLat, maxLat, minLon, maxLon;

        string[] stringSeparators = new string[] { "\"" };
        String[] textSplited = text.Split(stringSeparators, StringSplitOptions.None);

        minLat = textSplited[1];
        minLat = minLat.Replace('.', ',');
        minLon = textSplited[3];
        minLon = minLon.Replace('.', ',');
        maxLat = textSplited[5];
        maxLat = maxLat.Replace('.', ',');
        maxLon = textSplited[7];
        maxLon = maxLon.Replace('.', ',');

        GameObject go = GameObject.FindGameObjectWithTag("StaticInfos");
        StaticInfos si = go.GetComponent<StaticInfos>();
        si.setMinLat(double.Parse(minLat));
        si.setMaxLat(double.Parse(maxLat));
        si.setMinLon(double.Parse(minLon));
        si.setMaxLon(double.Parse(maxLon));
        si.setCenterLat((si.getMaxLat() + si.getMinLat()) / 2);
        si.setCenterLon((si.getMaxLon() + si.getMinLon()) / 2);*/
        GameObject go = GameObject.FindGameObjectWithTag("StaticInfos");
        StaticInfos si = go.GetComponent<StaticInfos>();

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
        return calculateMapDimensions(si.getMinLat(), si.getMaxLat(), si.getMinLon(), si.getMaxLon());
    }

    private double[] calculateMapDimensions(double lat1, double lat2, double lon1, double lon2)
    {
        //double R = 6378.137;
        double R = 6400;
        double dLat = lat2 * Math.PI / 180 - lat1 * Math.PI / 180;
        double dLon = 0;
        double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) + Math.Cos(lat1 * Math.PI / 180) * Math.Cos(lat2 * Math.PI / 180) * Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
        double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        double lat = R * c * 1000;

        dLat = 0;
        dLon = lon2 * Math.PI / 180 - lon1 * Math.PI / 180;
        a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) + Math.Cos(lat1 * Math.PI / 180) * Math.Cos(lat2 * Math.PI / 180) * Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
        c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        double lon = R * c * 1000;
        return new double[] { lat, lon };
    }

    private void LateUpdate()
    {
        try
        {
            Vector3 newPosition = Camera.current.transform.position;
            newPosition.y = transform.position.y;
            transform.position = newPosition;
            transform.rotation = Quaternion.Euler(90f, Camera.current.transform.eulerAngles.y, 0f);
        }
        catch (NullReferenceException e)
        {

        }
    }
}
