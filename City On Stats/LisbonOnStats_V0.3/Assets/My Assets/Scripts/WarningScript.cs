using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarningScript : MonoBehaviour
{
    double closeTime = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(closeTime!=0 && Time.time > closeTime)
        {
            gameObject.SetActive(false);
        }
    }

    public void setTimer(int seconds)
    {
        closeTime = Time.time + seconds;
    }
}
