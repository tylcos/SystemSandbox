using UnityEngine;



public class Drive : MonoBehaviour
{
    public Rigidbody rb;
    public Vector3 accelVec;
    public float accel;



    internal void Start()
    {
        rb = GetComponent<Rigidbody>();
    }



    private void FixedUpdate()
    {
        rb.AddForce(accelVec, ForceMode.Acceleration);
    }



    public Vector3 EstimatedPos(float t)
    {
        return .5f * accelVec * t * t + rb.velocity * t + rb.position;
    }
}