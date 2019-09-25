using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameClock : MonoBehaviour
{
    public Text textField;
    static int hour;
    static int minute;
    private int totalMinutes;
    public GameObject sun;
    public GameObject moon;

    private double nextTime;
    // Start is called before the first frame update
    void Start()
    {
        nextTime = Time.time;
        hour = 12;
        minute = 0;
        totalMinutes = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= nextTime)
        {
            updateTime();
            nextTime += 2;

        }
    }

    void updateTime()
    {
        minute += 1;
        totalMinutes += 1;

        if (minute >= 60)
        {
            minute -= 60;
            hour += 1;
        }

        if (hour == 24)
            hour = 0;

        if (hour < 10)
            if (minute < 10)
                textField.text = "TIME: 0" + hour + ":0" + minute;
            else
                textField.text = "TIME: 0" + hour + ":" + minute;
        else
            if (minute < 10)
            textField.text = "TIME: " + hour + ":0" + minute;
        else
            textField.text = "TIME: " + hour + ":" + minute;

        if (totalMinutes >= 1440)
        {
            totalMinutes -= 1440;
        }

        if (totalMinutes % 4 == 0)
        {
            sun.GetComponent<SunCrontrol>().rotateSun();
            moon.GetComponent<SunCrontrol>().rotateSun();
        }
    }

}
