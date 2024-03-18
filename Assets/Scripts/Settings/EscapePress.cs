using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapePress : MonoBehaviour
{
    public GameObject settings;
    public GameObject timer;
    public GameObject startScreen;
    public GameObject endScreen;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {

            settings.SetActive(!settings.activeSelf);

            if (startScreen.activeSelf == false && endScreen.activeSelf == false)
            {
                timer.SetActive(!settings.activeSelf);

            }



        }
    }
}
