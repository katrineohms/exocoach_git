using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Difficulty : MonoBehaviour
{
    public GameObject hitboxSpawner;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        setSize();
    }

    public void setSize()
    {
        if (PlayerPrefs.GetString("difficulty") == "easy")
        {
            hitboxSpawner.transform.localScale = new Vector3(4f, 4f, hitboxSpawner.transform.localScale.z);
        }

        else if (PlayerPrefs.GetString("difficulty") == "medium")
        {
            hitboxSpawner.transform.localScale = new Vector3(2.5f, 2.5f, hitboxSpawner.transform.localScale.z);
        }

        else if (PlayerPrefs.GetString("difficulty") == "hard")
        {
            hitboxSpawner.transform.localScale = new Vector3(1f, 1f, hitboxSpawner.transform.localScale.z);
        }

        else
        {
            hitboxSpawner.transform.localScale = new Vector3(3f, 3f, hitboxSpawner.transform.localScale.z);
            Debug.Log("No assigned difficulty");
        }
    }
}
