using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private ElbowPositioner elbowPositioner;

    // Start is called before the first frame update
    void Start()
    {
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    public void SetElbowPositioner(ElbowPositioner elbowPositioner)
    {
        this.elbowPositioner = elbowPositioner;
    }

    public ElbowPositioner GetElbowPositioner()
    {
        return elbowPositioner;
    }
}
