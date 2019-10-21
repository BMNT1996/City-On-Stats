using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveTutorial : MonoBehaviour
{
    bool[] checklist = { false, false, false, false }; //0-Visualization 1-Chart 2-Teleport 3-Autonomous bus
    public GameObject[] panels;
    bool start = false;
    int taskNumber = 0;
    double nextTutorial;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
            if (Input.GetKeyDown("h"))
            {
                start = true;
                nextTutorial = Time.time + 30;
            }

        if (start)
        {
            if (Input.GetKeyDown("v"))
            {
                visualizationUsed();
            }
            if (Input.GetMouseButtonDown(1))
            {
                checklist[1] = true;
                if (panels[1].active)
                {
                    panels[1].SetActive(false);
                    nextTutorial = Time.time + 30;
                }
                if (taskNumber == 1)
                    taskNumber++;
                checkTaskNumber();
            }
            if (Input.GetKeyDown("t"))
            {
                teleportUsed();
            }
            if (Input.GetKeyDown("b"))
            {
                busesUsed();
            }

            if(nextTutorial!=0 && Time.time > nextTutorial)
            {
                panels[taskNumber].SetActive(true);
                nextTutorial = 0;
            }
        }
    }

    private void checkTaskNumber()
    {
        if (taskNumber != -1)
        {
            if (checklist[taskNumber])
            {
                taskNumber++;
                if (taskNumber == 4)
                {
                    taskNumber = -1;
                    nextTutorial = 0;
                }
                else
                    checkTaskNumber();
            }
        }
    }

    public void visualizationUsed()
    {
        checklist[0] = true;
        if (panels[0].active)
        {
            panels[0].SetActive(false);
            nextTutorial = Time.time + 30;
        }
        if (taskNumber == 0)
            taskNumber++;
        checkTaskNumber();
    }

    public void teleportUsed()
    {
        checklist[2] = true;
        if (panels[2].active)
        {
            panels[2].SetActive(false);
            nextTutorial = Time.time + 30;
        }
        if (taskNumber == 2)
            taskNumber++;
        checkTaskNumber();
    }

    public void busesUsed()
    {
        checklist[3] = true;
        if (panels[3].active)
        {
            panels[3].SetActive(false);
            nextTutorial = 0;
        }
        if (taskNumber == 3)
            taskNumber = -1;
        checkTaskNumber();
    }

}
