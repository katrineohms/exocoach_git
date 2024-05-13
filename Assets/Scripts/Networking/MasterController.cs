using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Udp;

public class MasterController : MonoBehaviour
{
    public UdpHostMulti udpHost;
    public Transform cursor;
    private float wrist_angle;
    private float elbow_angle;
    public GameObject hitbox;

    [Header("Debug settings")]
    public float x_speed = -40f;
    public float y_speed = 200f;
    public float y_offset = -3f;
    public float x_offset = -4f;
    private float z_position = 10f;
    public bool floatingArm = false;

    // Start is called before the first frame update
    void Start()
    {
        //ExampleEvents.current.onMKeyHit += OnMKeyhit;
        UdpHostMulti.OnReceiveMsg += OnMotorValue;
        floatingArm = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (floatingArm == true)
        {

        }

        else
        {
            cursor.position = new Vector3(x_offset + wrist_angle / x_speed, y_offset + elbow_angle / y_speed, z_position);
        }

    }

    private void OnMotorValue(int clientId, string value)
    {

        if (floatingArm == true)
        {

        }
        else
        {
            if (clientId == 1) // Master client
            {
                string[] values = value.Split(':');
                if (values.Length == 2)
                {
                    string message = $"{values[0]},{values[1]}";
                    udpHost.SendSecondaryMsg(message);
                }
            }

            else if (clientId == 0) // Secondary client
            {
                string[] values = value.Split(':');
                if (values.Length == 2)
                {
                    // Parse and assign the wrist and elbow angles
                    wrist_angle = float.Parse(values[0]);
                    elbow_angle = float.Parse(values[1]);
                }

            }
        }
    }

    public void floatArm(){
        string message = "float";
        udpHost.SendSecondaryMsg(message);
        udpHost.SendMasterMsg(message);
        floatingArm = true;
    }

    public void unfloatArm()
    {
        floatingArm = false;
    }

}
