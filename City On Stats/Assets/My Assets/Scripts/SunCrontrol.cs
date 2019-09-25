using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SunCrontrol : MonoBehaviour
{

    private bool on;

    public void Start()
    {
        if (transform.position.y < -900)
        {
            on = false;
            gameObject.GetComponent<Light>().shadows = LightShadows.None;
        }
        else
        {
            on = true;
            gameObject.GetComponent<Light>().shadows = LightShadows.Soft;
        }
    }

    public void Update()
    {
        if (transform.position.y < -900 && on)
        {
            on = false;
            gameObject.GetComponent<Light>().shadows = LightShadows.None;
        }
        else if (transform.position.y >= -900 && !on)
        {
            on = true;
            gameObject.GetComponent<Light>().shadows = LightShadows.Soft;
        }
        //transform.RotateAround(Vector3.zero, Vector3.right, 10f*Time.deltaTime);
        //transform.LookAt(Vector3.zero);
    }

    public void rotateSun()
    {
        transform.RotateAround(Vector3.zero, Vector3.right, 1);
        transform.LookAt(Vector3.zero);
    }

}
