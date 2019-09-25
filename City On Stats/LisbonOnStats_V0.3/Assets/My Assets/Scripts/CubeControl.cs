using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class CubeControl : MonoBehaviour
{
    public GameObject Player;
    public GameObject Drone;
    public DataStorage DS;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        GameObject go = GameObject.FindGameObjectWithTag("StaticInfos");
        StaticInfos si = go.GetComponent<StaticInfos>();
        if (si.isPlayerViewActive())
        {
            if (transform.position.x - Player.transform.position.x > 16)
            {
                transform.position = new Vector3(transform.position.x - 32, transform.position.y, transform.position.z);
                resetCube();
            }

            if (transform.position.x - Player.transform.position.x < -16)
            {
                transform.position = new Vector3(transform.position.x + 32, transform.position.y, transform.position.z);
                resetCube();
            }

            if (transform.position.z - Player.transform.position.z > 16)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - 32);
                resetCube();
            }

            if (transform.position.z - Player.transform.position.z < -16)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + 32);
                resetCube();
            }
        }

        else
        {
            if (transform.position.x - Drone.transform.position.x > 16)
            {
                transform.position = new Vector3(transform.position.x - 32, transform.position.y, transform.position.z);
            }

            if (transform.position.x - Drone.transform.position.x < -16)
            {
                transform.position = new Vector3(transform.position.x + 32, transform.position.y, transform.position.z);
            }

            if (transform.position.z - Drone.transform.position.z > 16)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - 32);
            }

            if (transform.position.z - Drone.transform.position.z < -16)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + 32);
            }
        }
    }

    public void updateColor(Color c)
    {
        gameObject.GetComponent<Renderer>().material.color = c;
    }

    public void updateHeight(double norm)
    {
        transform.localScale = new Vector3(4, ((float)norm * 6), 4);
    }

    public void resetCube()
    {
        transform.localScale = new Vector3(4, 0.01f, 4);
        gameObject.GetComponent<Renderer>().material.color = new Color(0.5f, 0.5f, 0.5f, 0.2f);
    }

}
