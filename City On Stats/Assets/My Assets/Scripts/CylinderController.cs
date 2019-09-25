using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CylinderController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void updateCylinder(double norm, Color c)
    {
        gameObject.GetComponent<Renderer>().material.color = c;
        transform.localScale = new Vector3(1, ((float)(norm * 1.5)), 1);
        transform.localPosition = new Vector3(-0.331f, (float)(2.29 + norm), 0);
    }
}
