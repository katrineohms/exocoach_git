using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Udp;
using Fusion;

public class CursorController : NetworkBehaviour
{
    public GameObject udpHostObject;
    private UdpHost udpHost;

    private float wrist_angle;
    private float elbow_angle;
    public GameObject hitbox;
    public GameObject cursor;

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
        Debug.Log("Cursor Controller active");
        udpHostObject = GameObject.FindGameObjectWithTag("UdpHost");
        udpHost = udpHostObject.GetComponent<UdpHost>();

        cursor = GameObject.FindGameObjectWithTag("Cursor");
        //ExampleEvents.current.onMKeyHit += OnMKeyhit;
        UdpHost.OnReceiveMsg += OnMotorValue;

        if (arrowkeyDebug == true)
        {
            cursor.transform.position = new Vector3(0, 0, z_position);
        }
    }

    // Update is called once per frame
    void Update()
    {
        hitbox = GameObject.FindGameObjectWithTag("HitBox");
        cursor = GameObject.FindGameObjectWithTag("Cursor");

        if (arrowkeyDebug == true)
        {
            MoveCursorWithArrowKeys();
        }

        else
        {
            cursor.transform.position = new Vector3(x_offset + wrist_angle / x_speed, y_offset + elbow_angle / y_speed, z_position);
        }


        // Checking when the key "m" is pressed
        if (Input.GetKeyDown("m"))
        {
            if (hitbox != null && cursor != null)
            {
                Vector3 difference = cursor.transform.position - hitbox.transform.position;
                Debug.Log("Difference in position: " + difference);
                if (difference.y <= -0.4)
                {
                    Debug.Log("up");
                    udpHost.SendMsg("up");
                }

                if (difference.y >= 0.4)
                {
                    Debug.Log("down");
                    udpHost.SendMsg("down");
                }

                if (difference.x <= -0.3)
                {
                    Debug.Log("right");
                    udpHost.SendMsg("right");
                }

                if (difference.x >= 0.3)
                {
                    Debug.Log("left");
                    udpHost.SendMsg("left");
                }

                udpHost.SendMsg("sleep");
            }
             ExampleEvents.current.MKeyHit();
        }
    }

    private void MoveCursorWithArrowKeys()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        // Adjust cursor position based on arrow key inputs
        cursor.transform.position += new Vector3(moveHorizontal, moveVertical, 0) * Time.deltaTime * 5f;
    }


    private void OnMKeyhit()
    {
        Debug.Log("pressed");
        udpHost.SendMsg("move");
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
