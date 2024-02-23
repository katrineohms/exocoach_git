using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Udp;

public class Controller : MonoBehaviour
{
    public UdpHost udpHost;
    public Transform objectToMove;
    private float motor_angle;

    void Start()
    {
        Receive.current.onMKeyHit += OnMKeyhit;
        UdpHost.OnReceiveMsg += OnMotorValue;
    }

    //Output to exoskeleton
    void Update()
    {
        // Checking when the key "m" is pressed
        if (Input.GetKeyDown("m"))
        {
            // If it's pressed, trigger the event which we specified in our ExampleEvents.cs
            Receive.current.MKeyHit();
        }

        objectToMove.position = new Vector3(motor_angle / 100f, 0f, 0f);
    }

    //Output to exoskeleton 
    private void OnMKeyhit()
    {
        Debug.Log("pressed");
        udpHost.SendMsg("move");

    }   

    //Input from exoskeleton
    private void OnMotorValue(string value)
    {
        motor_angle = float.Parse(value);
        //Debug.Log("This is the received value: " + value);
    }
}