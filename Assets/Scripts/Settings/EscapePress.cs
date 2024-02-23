using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapePress : MonoBehaviour
{
    public GameObject settings;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (settings != null)
            {
                settings.SetActive(!settings.activeSelf);
            }
        }
    }
}
