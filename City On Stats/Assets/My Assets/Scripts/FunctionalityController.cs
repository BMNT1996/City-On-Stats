using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FunctionalityController : MonoBehaviour
{
    private int value;

    public Text text;
    public GameObject image;
    public Button nextButton;
    public Texture[] images;
    public GameObject PanelHandler;

    // Start is called before the first frame update
    void Start()
    {
        value = 0;
        actions();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void actions()
    {
        switch (value)
        {
            case 0:
                image.GetComponent<RawImage>().texture = images[0];
                text.text = "Hello User";
                break;

            case 1:
                image.GetComponent<RawImage>().texture = images[0];
                text.text = "Let's get start and see 4 functionalitys in City On Stats";
                break;

            case 2:
                image.GetComponent<RawImage>().texture = images[0];
                text.text = "First functionality is an eye helper";
                break;

            case 3:
                image.GetComponent<RawImage>().texture = images[0];
                text.text = "You can switch your data visualization";
                break;

            case 4:
                image.GetComponent<RawImage>().texture = images[0];
                text.text = "We have 7 modes for you";
                break;

            case 5:
                image.GetComponent<RawImage>().texture = images[1];
                text.text = "Simple";
                break;

            case 6:
                image.GetComponent<RawImage>().texture = images[1];
                text.text = "You only have the value at the botton right";
                break;

            case 7:
                image.GetComponent<RawImage>().texture = images[2];
                text.text = "Texture";
                break;

            case 8:
                image.GetComponent<RawImage>().texture = images[2];
                text.text = "The color of the bus will change with the value";
                break;

            case 9:
                image.GetComponent<RawImage>().texture = images[3];
                text.text = "BAR";
                break;

            case 10:
                image.GetComponent<RawImage>().texture = images[3];
                text.text = "A bar will grow on top of your bus depending of the value";
                break;

            case 11:
                image.GetComponent<RawImage>().texture = images[4];
                text.text = "Color Cubes";
                break;

            case 12:
                image.GetComponent<RawImage>().texture = images[4];
                text.text = "Cubes like tiles will appear under the bus and show the value";
                break;

            case 13:
                image.GetComponent<RawImage>().texture = images[5];
                text.text = "Height Cubes";
                break;

            case 14:
                image.GetComponent<RawImage>().texture = images[5];
                text.text = "Like color cubes, but these ones grows with the value";
                break;

            case 15:
                image.GetComponent<RawImage>().texture = images[6];
                text.text = "Fog";
                break;

            case 16:
                image.GetComponent<RawImage>().texture = images[6];
                text.text = "More fog means higher values";
                break;

            case 17:
                image.GetComponent<RawImage>().texture = images[7];
                text.text = "Rain";
                break;

            case 18:
                image.GetComponent<RawImage>().texture = images[7];
                text.text = "Listen the rain, it depends of the values";
                break;

            case 19:
                image.GetComponent<RawImage>().texture = images[0];
                text.text = "These modes will help you to see the polution";
                break;

            case 20:
                image.GetComponent<RawImage>().texture = images[0];
                text.text = "Second functionality is a time saver";
                break;

            case 21:
                image.GetComponent<RawImage>().texture = images[8];
                text.text = "You can teleport yourself to any destination you want";
                break;

            case 22:
                image.GetComponent<RawImage>().texture = images[0];
                text.text = "The third functionality is like a time travel";
                break;

            case 23:
                image.GetComponent<RawImage>().texture = images[9];
                text.text = "RIGHT CLICK and you can see the evolution by hour";
                break;

            case 24:
                image.GetComponent<RawImage>().texture = images[0];
                text.text = "The fourth functionality is like a spa";
                break;

            case 25:
                image.GetComponent<RawImage>().texture = images[10];
                text.text = "You can \"take\" a bus and let him guide you throw its route";
                break;

            case 26:
                image.GetComponent<RawImage>().texture = images[10];
                text.text = "You can \"ESC\" to exit from the bus you \"take\"";
                break;

            case 27:
                image.GetComponent<RawImage>().texture = images[0];
                text.text = "Finaly you are ready, and remember";
                break;

            case 28:
                image.GetComponent<RawImage>().texture = images[0];
                text.text = "DON'T BE AFRAID TO EXPLORE";
                nextButton.GetComponentInChildren<Text>().text = "Next";
                break;

            case 29:
                image.GetComponent<RawImage>().texture = images[11];
                text.text = "Have Fun";
                nextButton.GetComponentInChildren<Text>().text = "Exit";
                break;

            case 30:
                value = 0;
                nextButton.GetComponentInChildren<Text>().text = "Next";
                PanelHandler.GetComponent<PanelController>().closeAllPanels();
                gameObject.SetActive(false);
                break;

        }
    }

    public void buttonNext()
    {
        value++;
        actions();
    }

    public void buttonPreviouns()
    {
        if (value != 0)
        {
            value--;
            actions();
        }
    }

    public void buttonExit()
    {
        value = 0;
        PanelHandler.GetComponent<PanelController>().closeAllPanels();
    }
}
