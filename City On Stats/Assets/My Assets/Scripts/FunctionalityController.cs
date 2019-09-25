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
                text.text = "Simple - Just with the numeric value on the coner";
                break;

            case 6:
                image.GetComponent<RawImage>().texture = images[2];
                text.text = "Color Changing - Your bus color will change";
                break;

            case 7:
                image.GetComponent<RawImage>().texture = images[3];
                text.text = "Bar - You get a bar in top of the bus";
                break;

            case 8:
                image.GetComponent<RawImage>().texture = images[4];
                text.text = "Color Tiles - 81 tiles with colors just for you";
                break;

            case 9:
                image.GetComponent<RawImage>().texture = images[5];
                text.text = "Height Tiles - Same tiles that can also grow up";
                break;

            case 10:
                image.GetComponent<RawImage>().texture = images[6];
                text.text = "Fog - Sometimes values are so high that nobody can see nothing";
                break;

            case 11:
                image.GetComponent<RawImage>().texture = images[7];
                text.text = "Rain - Listen the rain, it can help you";
                break;

            case 12:
                image.GetComponent<RawImage>().texture = images[0];
                text.text = "Second functionality is a time saver";
                break;

            case 13:
                image.GetComponent<RawImage>().texture = images[8];
                text.text = "You can teleport yourself to any destination you want";
                break;

            case 14:
                image.GetComponent<RawImage>().texture = images[0];
                text.text = "The third functionality is like a time travel";
                break;

            case 15:
                image.GetComponent<RawImage>().texture = images[9];
                text.text = "RIGHT CLICK and you can see the evolution of the data";
                break;

            case 16:
                image.GetComponent<RawImage>().texture = images[0];
                text.text = "The fourth functionality is like a spa";
                break;

            case 17:
                image.GetComponent<RawImage>().texture = images[10];
                text.text = "You can \"take\" a bus and let him guide you throw his route";
                break;

            case 18:
                image.GetComponent<RawImage>().texture = images[0];
                text.text = "Finaly you are ready, and remember";
                break;

            case 19:
                image.GetComponent<RawImage>().texture = images[0];
                text.text = "DON'T BE AFRAID TO EXPLORE";
                nextButton.GetComponentInChildren<Text>().text = "Next";
                break;

            case 20:
                image.GetComponent<RawImage>().texture = images[11];
                text.text = "Have Fun";
                nextButton.GetComponentInChildren<Text>().text = "Exit";
                break;

            case 21:
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
        gameObject.SetActive(false);
    }
}
