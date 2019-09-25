using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class CountScript : MonoBehaviour
{
    private bool run;
    private string start;
    private int simple;
    private int texture;
    private int bar;
    private int color;
    private int height;
    private int fog;
    private int rain;
    private double nextCheck;
    private double nextSave;
    public Text visualizationOn;

    // Start is called before the first frame update
    void Start()
    {
        simple=0;
        texture=0;
        bar=0;
        color=0;
        height=0;
        fog=0;
        rain=0;
    }

    // Update is called once per frame
    void Update()
    {
            if (Input.GetKeyDown("h"))
            {
                run = true;
            }

        if (run)
        {
            if (start == null)
            {
                Debug.Log(System.DateTime.Now.ToString());
                start = System.DateTime.Now.ToString().Replace("/", "").Replace(":","");
            }

            if (nextCheck < Time.time)
            {
                checkVisualization();
                nextCheck += 0.2;
            }

            if (nextSave < Time.time)
            {
                save();
                nextSave += 5;
            }
        }
    }

    private void save()
    {
        String path = Application.dataPath;
        path += "/Resources/"+start+".txt";
        StreamWriter writer;
        if (!System.IO.File.Exists(path))
            using (File.Create(path)) ;
        writer = new StreamWriter(path, false);
        string result = "Visualization use:\nSimple: " + simple/5 + " seconds\nTexture: " + texture/5 + " seconds\nBar: " + bar/5 + " seconds\nColor Cubes: " + color/5 + " seconds\nHeight Cubes: " + height/5 + " seconds\nFog: " + fog/5 + " seconds\nRain: " + rain/5 + " seconds";
        writer.WriteLine(result);
        writer.Close();
    }

    private void checkVisualization()
    {
        if (visualizationOn.text.Contains("SIMPLE"))
        {
            simple++;
            return;
        }

        if (visualizationOn.text.Contains("TEXTURE"))
        {
            texture++;
            return;
        }

        if (visualizationOn.text.Contains("BAR"))
        {
            bar++;
            return;
        }

        if (visualizationOn.text.Contains("COLOR CUBE"))
        {
            color++;
            return;
        }

        if (visualizationOn.text.Contains("HEIGHT CUBE"))
        {
            height++;
            return;
        }

        if (visualizationOn.text.Contains("FOG"))
        {
            fog++;
            return;
        }

        if (visualizationOn.text.Contains("RAIN"))
        {
            rain++;
            return;
        }
    }
}
