using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Udp;

public class ElbowPositioner : MonoBehaviour
{
    public float valueElbow { get; set; }
    public float valueWrist { get; set; }

    private bool isRightHanded = true;
    
    [Header("Elbow settings")]
    [SerializeField] float elbowRotationValue = 21f / 360f;
    [SerializeField] float elbowOffset = -340f;

    [Header("Wrist settings")]
    [SerializeField] float wristRotationValue = 2f;
    [SerializeField] float wristOffset = 90f;

    // Start is called before the first frame update
    void Start()
    {
        valueElbow = 0;
        valueWrist = 0;
        // Subscribe to the OnReceiveMsg event
        UdpHost.OnReceiveMsg += UpdateAngles;
    }

    // Update is called once per frame
    void Update()
    {
        Quaternion wristRot;
        if (!isRightHanded)
        {
            wristRot = Quaternion.AngleAxis(-valueWrist, Vector3.right);
        }
        else
        {
            wristRot = Quaternion.AngleAxis(valueWrist, Vector3.right);
        }

        transform.rotation = Quaternion.Euler(0, -90, valueElbow);
        transform.rotation = transform.rotation * wristRot;
    }

    public void SetElbow(float angle, float wristAngle)
    {
        valueElbow = (angle / elbowRotationValue) + elbowOffset;
        valueWrist = (wristAngle / wristRotationValue) - wristOffset;
    }

    public void SetRighthanded(bool value)
    {
        isRightHanded = value;
    }


    private void UpdateAngles(string message)
    {
        string[] values = message.Split(':');
        if (values.Length == 2)
        {
            // Parse and assign the wrist and elbow angles
            valueElbow = float.Parse(values[0]);
            valueWrist = float.Parse(values[1]);
        }

        // Parse the received message and update the wrist and elbow angles
        string[] angles = message.Split(':');
        if (angles.Length == 2 && float.TryParse(angles[0], out float wristAngle) && float.TryParse(angles[1], out float elbowAngle))
        {
            SetElbow(elbowAngle, wristAngle);
        }
    }

}
