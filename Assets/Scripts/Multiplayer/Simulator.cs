using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Simulator : MonoBehaviour
{
    public static event Action<string> OnDoSimulation;

    // Update is called once per frame
    void Update()
    {
        string message = GenerateMessage();

        OnDoSimulation?.Invoke(message);
    }

    private string GenerateMessage()
    {
        float elbow = UnityEngine.Random.Range(100.0f, 250.0f);
        float wrist = UnityEngine.Random.Range(0.0f, 100.0f);
        string message = "00:00:00.0000," + elbow + "," + wrist;
        return message;
    }
}
