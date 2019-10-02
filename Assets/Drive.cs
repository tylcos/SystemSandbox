﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Drive : MonoBehaviour
{
    public Rigidbody rb;

    public float a = 5f;
    public Vector3 iV = new Vector3(0, 0, 0);

    [HideInInspector]
    public Vector3 accel;

    void Start()
    {
        transform.LookAt(transform.position + accel);

        rb.velocity = iV;
    }



    void FixedUpdate()
    {
        rb.AddForce(accel, ForceMode.Acceleration);
    }
}
