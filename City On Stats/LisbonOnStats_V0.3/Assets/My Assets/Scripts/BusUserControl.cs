using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BusUserControl : MonoBehaviour
{
    GameObject bus;
    double nextUpdate;

    // Start is called before the first frame update
    void Start()
    {
        nextUpdate = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Find("Slider").Find("Handle Slide Area").Find("Handle").Find("Text").GetComponent<Text>().text = transform.Find("Slider").GetComponent<Slider>().value.ToString();
        transform.Find("MaxSpeed").GetComponent<Text>().text = bus.GetComponent<BusAi>().getMaxSpeed().ToString();
        if (!Input.GetMouseButton(0) && !Input.GetMouseButton(1) && nextUpdate < Time.time)
        {
            transform.Find("Slider").GetComponent<Slider>().value = bus.GetComponent<BusAi>().getPathPercentage();
            nextUpdate = Time.time + 1;
        }
    }

    public void open(GameObject bus)
    {
        gameObject.SetActive(true);
        this.bus = bus;
        transform.Find("BusName").GetComponent<Text>().text = bus.GetComponent<BusAi>().getName();
        transform.Find("StartPoint").GetComponent<Text>().text = "Start Point: \n" + bus.GetComponent<BusAi>().getStartPoint();
        transform.Find("EndPoint").GetComponent<Text>().text = "End Point: \n" + bus.GetComponent<BusAi>().getEndPoint();
    }

    public void close()
    {
        gameObject.SetActive(false);
    }

    public void jumpToPercentage()
    {
        bus.GetComponent<BusAi>().jumpToPercentage(transform.Find("Slider").GetComponent<Slider>().value);
    }

    public void buttonClicked(Button b)
    {
        if (b.name.Equals("Play"))
        {
            bus.GetComponent<BusAi>().play();
        }

        if (b.name.Equals("Stop"))
        {
            bus.GetComponent<BusAi>().stop();
        }

        if (b.name.Equals("Reverse"))
        {
            bus.GetComponent<BusAi>().doReverse();
        }

        if (b.name.Equals("-"))
        {
            bus.GetComponent<BusAi>().speedDown();
        }

        if (b.name.Equals("+"))
        {
            bus.GetComponent<BusAi>().speedUp();
        }
    }
}
