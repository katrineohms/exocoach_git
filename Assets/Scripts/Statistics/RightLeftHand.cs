using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Udp;
using System;

public class RightLeftHand : MonoBehaviour
{
    public UdpHostMulti udpHost;
    private float Master_wrist_angle;
    private float Master_elbow_angle;

    private float Secondary_wrist_angle;
    private float Secondary_elbow_angle;
    private float Total_Master_wrist_angle = 0;
    private float Total_Master_elbow_angle = 0;
    private bool masterMessageReceived = false;
    private bool secondaryMessageReceived = false;
    public float total_wristAngleDifference;
    public float total_elbowAngleDifference;

    // Start is called before the first frame update
    void Start()
    {
        //ExampleEvents.current.onMKeyHit += OnMKeyhit;
        UdpHostMulti.OnReceiveMsg += OnMotorValue;
    }

    // Update is called once per frame
    void Update()
    {
      
    }

    private void OnMotorValue(int clientId, string value)
    {
        if (clientId == 1) // Master client
        {
            string[] values = value.Split(':');
            if (values.Length == 2)
            {
                // Parse and assign the wrist and elbow angles
                Master_wrist_angle = float.Parse(values[0]);
                Master_elbow_angle = float.Parse(values[1]);

                // Set master message received flag
                masterMessageReceived = true;
            }
        }

        else if (clientId == 0) // Secondary client
        {
            string[] values = value.Split(':');
            if (values.Length == 2)
            {
                // Parse and assign the wrist and elbow angles
                Secondary_wrist_angle = float.Parse(values[0]);
                Secondary_elbow_angle = float.Parse(values[1]);

                // Set secondary message received flag
                secondaryMessageReceived = true;
            }
        }

        // Check if both master and secondary messages are received
        if (masterMessageReceived && secondaryMessageReceived)
        {
            // Calculate the difference between the angles

            total_wristAngleDifference += Difference(Secondary_wrist_angle, Master_wrist_angle);
            total_elbowAngleDifference += Difference(Secondary_elbow_angle, Master_elbow_angle);

            Total_Master_elbow_angle += Mathf.Abs(Master_elbow_angle);
            Total_Master_wrist_angle += Mathf.Abs(Master_wrist_angle);

            // Reset message received flags
            masterMessageReceived = false;
            secondaryMessageReceived = false;
        }
    }

    public float Difference(float secondary, float main)
    {
        float difference = Mathf.Abs(main - secondary);
        return difference;
    }

    public (float, float) GetDifference()
    {
        Debug.Log("Wrist difference:" + total_wristAngleDifference + ". Elbow difference:" + total_elbowAngleDifference + " Master wrist: " + Total_Master_wrist_angle + " Master elbow: " + Total_Master_elbow_angle);
        float WristPercentage = (1-(total_wristAngleDifference / Total_Master_wrist_angle)) * 100f;
        float ElbowPercentage = (1-(total_elbowAngleDifference / Total_Master_elbow_angle)) * 100f;

        Debug.Log("Wrist Angle Percentage: " + WristPercentage + "%");
        Debug.Log("Elbow Angle Percentage: " + ElbowPercentage + "%");


        if (float.IsNaN(WristPercentage))
        {
            //No movement in master controller, divided by 0 = NaN. Set to 75%, a middle value
            WristPercentage = 75;
        }

        if (float.IsNaN(ElbowPercentage))
        {
            //No movement in master controller, divided by 0 = NaN. Set to 75%, a middle value
            ElbowPercentage = 75;
        }

          
        return (WristPercentage, ElbowPercentage);
    }

    public void Reset()
    {
        Total_Master_elbow_angle = 0;
        Total_Master_wrist_angle = 0;
        total_wristAngleDifference = 0;
        total_elbowAngleDifference = 0;
    }
}
