using UnityEngine;



public class Drive : MonoBehaviour
{
    public Rigidbody rb;
    public Vector3 accel;



    internal void Start()
    {
        rb = GetComponent<Rigidbody>();
    }



    public Vector3 EstimatedPos(float t)
    {
        return .5f * accel * t * t + rb.velocity * t + rb.position;
    }
}