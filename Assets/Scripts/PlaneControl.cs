using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class PlaneControl : MonoBehaviour
{
    public float speed = 10f;
    public float accel = 10f;
    public Rigidbody rb;



    private void Start()
    {
        rb.velocity = transform.forward * speed;
    }



    private void FixedUpdate()
    {
        transform.rotation *= Quaternion.Euler(-2 * Time.fixedDeltaTime, 0, 0);
        rb.velocity = transform.forward * rb.velocity.magnitude;
        rb.AddForce(transform.forward * accel * Time.fixedDeltaTime, ForceMode.Acceleration);
    }
}