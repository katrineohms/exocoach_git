using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private GameObject target;

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            transform.position = new Vector3(target.transform.position.x, target.transform.position.y + 0.5f, target.transform.position.z);
            transform.rotation = target.transform.rotation;
        }
    }

    public void SetTarget(GameObject target)
    {
        this.target = target;
    }

    public void FindTarget()
    {
        this.target = FindObjectOfType<PlayerController>().gameObject;
    }
}
