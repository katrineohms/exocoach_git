using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpperArmPositioner : MonoBehaviour
{
    public Transform elbow;
    public Transform shoulder;

    Vector3 endV;
    Vector3 startV;
    Vector3 rotAxisV;
    Vector3 dirV;
    Vector3 cylDefaultOrientation = new Vector3(0, 1, 0);

    public float shoulderYCorrection = 0.05f;

    float dist;

    // Use this for initialization
    void Start()
    {
        endV = shoulder.position;
        startV = elbow.position;

    }

    // Update is called once per frame
    void Update()
    {
        endV = shoulder.position + new Vector3(0,-shoulderYCorrection, 0);
        startV = elbow.position;

        // Position
        gameObject.transform.position = (endV + startV) / 2.0F;

        // Rotation
        dirV = Vector3.Normalize(endV - startV);

        rotAxisV = dirV + cylDefaultOrientation;

        rotAxisV = Vector3.Normalize(rotAxisV);

        gameObject.transform.rotation = new Quaternion(rotAxisV.x, rotAxisV.y, rotAxisV.z, 0);

        // Scale        
        dist = Vector3.Distance(endV, startV);

        gameObject.transform.localScale = new Vector3(0.09f,dist/4f,0.09f);

    }
}