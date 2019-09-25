using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NodePath : MonoBehaviour
{
    Dictionary<string, double> gases = new Dictionary<string, double>();

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void addGas(string name, double value)
    {
        if (!gases.ContainsKey(name))
            gases.Add(name, value);
    }

    public void addGases(Dictionary<string, double> dict)
    {
        foreach (string key in dict.Keys)
        {
            double val = dict[key];
            gases.Add(key, val);
        }
    }

    public double getGasValue(string gasName)
    {
        if (gases.ContainsKey(gasName))
            return gases[gasName];
        else
            return -1;
    }

    public string[] getGasNames()
    {
        return gases.Keys.ToArray<String>();
    }
}
