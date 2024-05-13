using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Udp;

public class SecondaryController : MonoBehaviour
{
    public UdpHost MainUdpHost;
    public UdpHost SecondaryUdpHost;
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

    public bool arrowkeyDebug = false;

    // Start is called before the first frame update
    void Start()
    {
        //ExampleEvents.current.onMKeyHit += OnMKeyhit;
        MainUdpHost.OnReceiveMsg += OnMotorValue;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("m"))
        {
            Debug.Log("3");
            SecondaryUdpHost.SendMsg("3");
            ExampleEvents.current.MKeyHit();
        }
    }


    private void OnMKeyhit()
    {
        Debug.Log("pressed");
        SecondaryUdpHost.SendMsg("move");
    }

    private void OnMotorValue(string value)
    {
        // Split the received message into wrist and elbow angles
        string[] values = value.Split(':');
        if (values.Length == 2)
        {
            // Parse and assign the wrist and elbow angles
            wrist_angle = float.Parse(values[0]);
            elbow_angle = float.Parse(values[1]);
        }
    }
}
