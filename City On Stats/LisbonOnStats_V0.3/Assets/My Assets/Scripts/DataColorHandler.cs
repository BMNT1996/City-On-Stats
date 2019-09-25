using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DataColorHandler : MonoBehaviour
{
    public class DataColorGas
    {
        string name;
        double min;
        double ave;
        double max;
        Color cmin;
        Color cave;
        Color cmax;

        public DataColorGas(string name, double min, double ave, double max, Color cmin, Color cave, Color cmax)
        {
            this.name = name;
            this.min = min;
            this.ave = ave;
            this.max = max;
            this.cmin = cmin;
            this.cave = cave;
            this.cmax = cmax;
        }

        public string getName()
        {
            return name;
        }

        public double getMinLim()
        {
            return min;
        }

        public double getAveLim()
        {
            return ave;
        }

        public double getMaxLim()
        {
            return max;
        }

        public Color getMinColor()
        {
            return cmin;
        }

        public Color getAveColor()
        {
            return cave;
        }

        public Color getMaxColor()
        {
            return cmax;
        }
    }

    Dictionary<string, DataColorGas> gases = new Dictionary<string, DataColorGas>();
    // Start is called before the first frame update
    void Start()
    {

    }

    public void setGas(DataColorGas dcg)
    {
        if (!gases.ContainsKey(dcg.getName()))
            gases.Add(dcg.getName(), dcg);
    }

    public double getNormValue(string gasName, double value)
    {
        DataColorGas gas = gases[gasName];
        if (value <= gas.getAveLim())
        {
            double v = value - gas.getMinLim();
            if (v <= 0)
            {
                return 0;
            }
            else
            {
                return (v / (gas.getAveLim() - gas.getMinLim())) / 2;
            }
        }
        else
        {
            double v = value - gas.getAveLim();
            if (value > gas.getMaxLim())
            {
                return 1;
            }
            else
            {
                return ((v / (gas.getMaxLim() - gas.getAveLim())) / 2) + 0.5;
            }
        }
    }

    public Color getGasColor(string gasName, double value)
    {
        DataColorGas gas = gases[gasName];
        double norm = getNormValue(gasName, value);
        if (norm < 0.5)
        {
            return ((float)norm) * 2 * gas.getAveColor() + ((float)(0.5 - norm)) * 2 * gas.getMinColor();
        }
        else
        {
            return ((float)(norm - 0.5)) * 2 * gas.getMaxColor() + ((float)(1 - norm)) * 2 * gas.getAveColor();
        }
    }

    public string[] getGasNames()
    {
        string[] result = new string[gases.Keys.Count];
        int i = 0;
        foreach (string key in gases.Keys)
        {
            result[i] = key;
            i++;
        }
        return gases.Keys.ToArray();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
