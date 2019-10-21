using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class BusAi : MonoBehaviour
{

    //public Transform path;
    private float maxSteerAngle = 45f;
    private float turnSpeed = 5f;
    public WheelCollider wheelFL;
    public WheelCollider wheelFR;
    public WheelCollider wheelRL;
    public WheelCollider wheelRR;
    float maxMotorTorque = 3000f;
    private float maxBrakeTorque = 150f;
    private float currentSpeed;
    float maxSpeed = (float)25 / 60;
    private Vector3 centerOfMass;
    private Material PlaneMaterial;

    private string name;
    private List<Transform> nodes;
    private int currectNode = 0;
    private bool avoiding = false;
    private float targetSteerAngle = 0;
    private bool reverse = false;
    private bool active = true;
    private double rotationEnd;
    private Material busMaterial;
    private DataColorHandler DCH;
    String start;
    String end;

    [Header("Sensors")]
    private float sensorLength = 3f;
    private Vector3 frontSensorPosition = new Vector3(-0.331f, 1f, 3.5f);
    private float frontSideSensorPosition = 0.6f;
    private float frontSensorAngle = 30f;

    public float Revs { get; internal set; }
    public float AccelInput { get; internal set; }

    private GameObject Sphere;

    private void Start()
    {
        Sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        Sphere.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        Sphere.name = "Banana";
        ignoreCOllisions();
        rotationEnd = Time.time;
        busMaterial = new Material(Shader.Find("Specular"));
        busMaterial.color = Color.black;
        Material[] l = new Material[25];
        for (int i = 0; i < 25; i++)
        {
            l[i] = busMaterial;
        }
        transform.Find("Plane").gameObject.GetComponent<Renderer>().materials = l;
        StaticInfos si = GameObject.FindGameObjectWithTag("StaticInfos").GetComponent<StaticInfos>();
        si.setActualAiGas(nodes[currectNode].GetComponent<NodePath>().getGasNames()[0]);
    }

    public void setPath(Transform pathObject)
    {
        Transform[] pathTransforms = pathObject.GetComponentsInChildren<Transform>();
        nodes = new List<Transform>();

        for (int i = 0; i < pathTransforms.Length; i++)
        {
            if (pathTransforms[i] != pathObject.transform)
            {
                nodes.Add(pathTransforms[i]);
            }
        }
        transform.position = new Vector3(nodes[0].transform.position.x, nodes[0].transform.position.y, nodes[0].transform.position.z - 10);

    }

    private void ignoreCOllisions()
    {
        GameObject map = GameObject.FindGameObjectWithTag("3DMAP");
        foreach (Transform t in map.GetComponentInChildren<Transform>())
        {
            if (t.gameObject.GetComponent<Collider>() != null)
            {
                Physics.IgnoreCollision(transform.Find("Plane").gameObject.GetComponent<Collider>(), t.gameObject.GetComponent<Collider>());
                Physics.IgnoreCollision(wheelFL, t.gameObject.GetComponent<Collider>());
                Physics.IgnoreCollision(wheelFR, t.gameObject.GetComponent<Collider>());
                Physics.IgnoreCollision(wheelRR, t.gameObject.GetComponent<Collider>());
                Physics.IgnoreCollision(wheelRL, t.gameObject.GetComponent<Collider>());
            }

        }
    }

    private void FixedUpdate()
    {
        if (active)
        {
            Sensors();
            ApplySteer();
            Drive();
            CheckWaypointDistance();
            LerpToSteerAngle();
            if ((transform.eulerAngles.z > 85 && transform.eulerAngles.z < 95) || (transform.eulerAngles.z < -85 && transform.eulerAngles.z > -95))
            {
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0);
            }
        }
    }

    private void Sensors()
    {
        RaycastHit hit;
        Vector3 sensorStartPos = transform.position;
        sensorStartPos += transform.forward * frontSensorPosition.z;
        sensorStartPos += transform.up * frontSensorPosition.y;
        sensorStartPos += transform.right * frontSensorPosition.x;
        float avoidMultiplier = 0;
        avoiding = false;
        bool roadleft = false, roadright = false;
        Vector3 f;
        //front right sensor
        sensorStartPos += transform.right * frontSideSensorPosition;
        f = sensorStartPos + transform.forward * 3; //Right
        Debug.DrawLine(sensorStartPos, f, Color.black, 10, false);
        f = sensorStartPos + Quaternion.AngleAxis(frontSensorAngle, transform.up) * transform.forward * 3;
        Debug.DrawLine(sensorStartPos, f, Color.white, 10, false); //Right Angle
        if (Physics.Raycast(sensorStartPos, transform.forward, out hit, sensorLength))
        {
            avoiding = true;
            avoidMultiplier -= 1f;
        }

        //front right angle sensor
        else if (Physics.Raycast(sensorStartPos, Quaternion.AngleAxis(frontSensorAngle, transform.up) * transform.forward, out hit, sensorLength))
        {
            avoiding = true;
            avoidMultiplier -= 0.5f;
        }

        //front left sensor
        sensorStartPos -= transform.right * frontSideSensorPosition * 2;
        f = sensorStartPos + transform.forward * 3;
        Debug.DrawLine(sensorStartPos, f,Color.black,10,false);
        f = sensorStartPos + Quaternion.AngleAxis(-frontSensorAngle, transform.up) * transform.forward * 3;
        Debug.DrawLine(sensorStartPos, f,Color.white,10,false);
        if (Physics.Raycast(sensorStartPos, transform.forward, out hit, sensorLength))
        {
            avoiding = true;
            avoidMultiplier += 1f;
        }

        //front left angle sensor
        else if (Physics.Raycast(sensorStartPos, Quaternion.AngleAxis(-frontSensorAngle, transform.up) * transform.forward, out hit, sensorLength))
        {
            avoiding = true;
            avoidMultiplier += 0.5f;
        }

        // left and right sensor angled, trying to keep bus on road 
        /*else
        {
            if (Physics.Raycast(sensorStartPos, Quaternion.AngleAxis(frontSensorAngle, transform.right) * Quaternion.AngleAxis(-5, transform.up) * transform.forward, out hit, sensorLength + 2))
            {
                string nameleft = hit.transform.gameObject.name;
                f = sensorStartPos + Quaternion.AngleAxis(frontSensorAngle, transform.right) * Quaternion.AngleAxis(-5, transform.up) * transform.forward * 5;
                Debug.DrawLine(sensorStartPos, f, Color.green, 10, false);
                sensorStartPos += transform.right * frontSideSensorPosition * 2;
                if (Physics.Raycast(sensorStartPos, Quaternion.AngleAxis(frontSensorAngle, transform.right) * Quaternion.AngleAxis(frontSensorAngle, transform.up) * transform.forward, out hit, sensorLength + 2))
                {
                    string nameright = hit.transform.gameObject.name;
                    f = sensorStartPos + Quaternion.AngleAxis(frontSensorAngle, transform.right) * Quaternion.AngleAxis(5, transform.up) * transform.forward * 5;
                    Debug.DrawLine(sensorStartPos, f, Color.red, 10, false);
                    if (nameleft.Contains("Road") && !nameright.Contains("Road"))
                    {
                        avoiding = true;
                        avoidMultiplier -= 1f;
                    }
                    else if (nameright.Contains("Road") && !nameleft.Contains("Road"))
                    {
                        avoiding = true;
                        avoidMultiplier += 1f;
                    }
                }

            }




        }*/

        //front center sensor
        if (avoidMultiplier == 0)
        {
            if (Physics.Raycast(sensorStartPos, transform.forward, out hit, sensorLength))
            {
                Debug.DrawLine(sensorStartPos, hit.point, Color.red, 2, false);
                avoiding = true;
                if (hit.normal.x < 0)
                {
                    avoidMultiplier = -1;
                }
                else
                {
                    avoidMultiplier = 1;
                }
            }
        }


        if (avoiding)
        {
            targetSteerAngle = maxSteerAngle * avoidMultiplier;
        }

    }

    private void LerpToSteerAngle()
    {
        wheelFL.steerAngle = Mathf.Lerp(wheelFL.steerAngle, targetSteerAngle, Time.deltaTime * turnSpeed);
        wheelFR.steerAngle = Mathf.Lerp(wheelFR.steerAngle, targetSteerAngle, Time.deltaTime * turnSpeed);
    }

    private void ApplySteer()
    {
        Vector3 relativeVector = transform.InverseTransformPoint(nodes[currectNode].position);
        float newSteer = (relativeVector.x / relativeVector.magnitude) * maxSteerAngle;
        wheelFL.steerAngle = newSteer;
        wheelFR.steerAngle = newSteer;
    }

    private void Drive()
    {
        currentSpeed = 2 * Mathf.PI * wheelFL.radius * wheelFL.rpm * 60 / 1000;

        if (currentSpeed < maxSpeed)
        {
            wheelFL.motorTorque = maxMotorTorque;
            wheelFR.motorTorque = maxMotorTorque;
        }
        else
        {
            wheelFL.motorTorque = 0;
            wheelFR.motorTorque = 0;
        }
    }

    private void CheckWaypointDistance()
    {
        if (Vector3.Distance(transform.position, nodes[currectNode].position) < 10f)
        {
            StaticInfos si = GameObject.FindGameObjectWithTag("StaticInfos").GetComponent<StaticInfos>();
            busMaterial.color = GameObject.FindGameObjectWithTag("DataColorHandler").GetComponent<DataColorHandler>().getGasColor(si.getActualAiGas(), nodes[currectNode].GetComponent<NodePath>().getGasValue(si.getActualAiGas()));
            /*if (currectNode == nodes.Count - 1)
            {
                currectNode = 0;
            }
            else
            {
                currectNode++;
            }*/
            if (!reverse)
            {
                if (currectNode != nodes.Count - 1)
                {
                    currectNode++;
                }

                else
                {
                    if (Vector3.Distance(transform.position, nodes[currectNode].position) < 5f)
                    {
                        //gameObject.transform.eulerAngles = transform.eulerAngles + (180f * Vector3.up);
                        gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
                        gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
                        rotationEnd = Time.time + 3;
                        gameObject.transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, transform.eulerAngles + (180f * Vector3.up), Time.time + 3);
                        reverse = !reverse;
                    }
                }
            }

            else
            {
                if (currectNode != 0)
                {
                    currectNode--;
                }

                else
                {
                    if (Vector3.Distance(transform.position, nodes[currectNode].position) < 5f)
                    {
                        gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
                        gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
                        rotationEnd = Time.time + 3;
                        gameObject.transform.eulerAngles = transform.eulerAngles + (180f * Vector3.up);
                        reverse = !reverse;
                    }
                }
            }
        }
    }

    public string getReaderText(string gas)
    {
        return gas + "\n" + nodes[currectNode].gameObject.GetComponent<NodePath>().getGasValue(gas).ToString("F3");
    }

    public double getGasValue(string gas)
    {
        return nodes[currectNode].gameObject.GetComponent<NodePath>().getGasValue(gas);
    }

    public Color getGasColor(string gas)
    {
        return GameObject.FindGameObjectWithTag("DataColorHandler").GetComponent<DataColorHandler>().getGasColor(gas, nodes[currectNode].GetComponent<NodePath>().getGasValue(gas));
    }

    public string getFirstGas()
    {
        return GameObject.FindGameObjectWithTag("DataColorHandler").GetComponent<DataColorHandler>().getGasNames()[0];
    }

    public string getName()
    {
        return name;
    }

    public void setName(String name)
    {
        this.name = name;
    }

    public void restoreAutonomy()
    {
        maxSpeed = (float)25 / 60;
        active = true;
        gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
    }

    public void setStartnEnd(String start, String end)
    {
        this.start = start;
        this.end = end;
    }

    public String getStartPoint()
    {
        return start;
    }

    public String getEndPoint()
    {
        return end;
    }

    public float getPathPercentage()
    {
        int value = currectNode * 100;

        return ((float)(value / nodes.Count));
    }

    public void jumpToPercentage(float percentage)
    {
        double nodeVal = percentage * (nodes.Count - 1) / 100;
        currectNode = ((int)nodeVal);
        transform.position = nodes[currectNode].transform.position;
        gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

        if (!reverse)
        {
            if (currectNode < nodes.Count - 2)
                transform.rotation = Quaternion.LookRotation(((Vector3)nodes[currectNode + 1].transform.position) - ((Vector3)nodes[currectNode].transform.position));
        }
        else
            if (currectNode > 0)
            transform.rotation = Quaternion.LookRotation(((Vector3)nodes[currectNode - 1].transform.position) - ((Vector3)nodes[currectNode].transform.position));
    }

    public void play()
    {
        active = true;
        gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
    }

    public void stop()
    {
        active = false;
        currentSpeed = 0;
        gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
    }

    public void doReverse()
    {
        reverse = !reverse;
        if (!reverse)
            currectNode++;
        else
            currectNode--;
        gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        gameObject.transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, transform.eulerAngles + (180f * Vector3.up), Time.time + 3);
    }

    public void speedUp()
    {
        if (maxSpeed < (float)98 / 60)
        {
            maxSpeed += (float)5 / 60;
        }
    }

    public void speedDown()
    {
        if (maxSpeed > (float)7 / 60)
        {
            maxSpeed -= (float)5 / 60;
        }
    }

    public String getMaxSpeed()
    {
        int[] speedList = { 0, 5, 10, 15, 20, 25, 30, 35, 40, 45, 50, 55, 60, 65, 70, 75, 80, 85, 90, 95, 100 };
        int maxVel = 0;
        foreach (int i in speedList)
        {
            if (Math.Abs(((int)(maxSpeed * 60)) - i) < 3)
            {
                maxVel = i;
                break;
            }
        }
        return ((int)(gameObject.GetComponent<Rigidbody>().velocity.magnitude * 3.6)) + "/" + maxVel;
    }

    public void setDCH(DataColorHandler DCH)
    {
        this.DCH = DCH;
        StaticInfos si = GameObject.FindGameObjectWithTag("StaticInfos").GetComponent<StaticInfos>();
        si.setActualAiGas(DCH.getGasNames()[0]);
    }
}
