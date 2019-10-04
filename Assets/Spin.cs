using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin : MonoBehaviour
{
    public Rigidbody rb;

    void Start()
    {
        rb.angularVelocity = new Vector3(0, 0, 20);
    }
}
