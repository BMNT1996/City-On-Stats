
using UnityEngine;
using System.Collections;
using UnityStandardAssets.Characters.ThirdPerson;
using UnityStandardAssets.Cameras;
using UnityStandardAssets.Vehicles.Car;

public class ViewChanger : MonoBehaviour
{
    StaticInfos si;

    public GameObject[] characters;
    public GameObject WarningPanel;
    GameObject currentCharacter;
    int charactersIndex;
    bool cCalled = false;

    void Start()
    {
        GameObject go = GameObject.FindGameObjectWithTag("StaticInfos");
        si = go.GetComponent<StaticInfos>();
        si.setOnAiBus(false);
        charactersIndex = 0;
        currentCharacter = characters[0];
    }

    void Update()
    {
        if ((Input.GetKeyDown("c") || cCalled) && !si.isOnAiBusMode())
        {
            cCalled = false;
            charactersIndex++;
            if (charactersIndex == characters.Length)
            {
                charactersIndex = 0;
            }

            characters[charactersIndex].transform.Find("camera").GetComponents<Camera>()[0].enabled = true;
            foreach (Transform aux in characters[charactersIndex].transform)
            {
                if (aux.tag.Equals("PlayerMesh"))
                {
                    GameObject parentt = aux.parent.gameObject;
                    if (parentt.name == "PlayerView")
                    {
                        Debug.Log(parentt.name);
                        parentt.GetComponent<CarController>().enabled = true;
                        parentt.GetComponent<CarUserControl>().enabled = true;
                        parentt.GetComponent<CarAudio>().enabled = true;
                    }
                    else
                    {
                        aux.gameObject.SetActive(true);
                    }
                    break;
                }
            }
            currentCharacter.transform.Find("camera").GetComponents<Camera>()[0].enabled = false;
            foreach (Transform aux in currentCharacter.transform)
            {
                if (aux.tag.Equals("PlayerMesh"))
                {
                    GameObject parentt = aux.parent.gameObject;
                    if (parentt.name == "PlayerView")
                    {
                        parentt.GetComponent<CarController>().enabled = false;
                        parentt.GetComponent<CarUserControl>().enabled = false;
                        parentt.GetComponent<CarAudio>().enabled = false;
                        parentt.GetComponent<Rigidbody>().velocity = Vector3.zero;
                        parentt.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
                    }
                    else
                    {
                        aux.gameObject.SetActive(false);
                    }
                    break;
                }
            }

            if (currentCharacter.name.Equals("PlayerView"))
            {
                characters[charactersIndex].transform.position = currentCharacter.transform.position;
                characters[charactersIndex].GetComponent<CameraUserControl>().enabled = true;
                currentCharacter.GetComponent<CarUserControl>().enabled = false;
                GameObject go = GameObject.FindGameObjectWithTag("StaticInfos");
                StaticInfos si = go.GetComponent<StaticInfos>();
                si.setPlayerViewActive(false);
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
            else
            {
                characters[charactersIndex].GetComponent<CarUserControl>().enabled = true;
                currentCharacter.GetComponent<CameraUserControl>().enabled = false;
                GameObject go = GameObject.FindGameObjectWithTag("StaticInfos");
                StaticInfos si = go.GetComponent<StaticInfos>();
                si.setPlayerViewActive(true);
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }

            currentCharacter = characters[charactersIndex];
        }

        if ((Input.GetKeyDown("c") || cCalled || Input.GetKeyDown("JoystickButton3")) && si.isOnAiBusMode())
        {
            WarningPanel.SetActive(true);
            WarningPanel.GetComponent<WarningScript>().setTimer(5);
        }
    }

    public void cCall()
    {
        cCalled = true;
    }
}