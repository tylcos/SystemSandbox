using UnityEngine;



public class SimpleDrive : Drive
{
    public Vector3 iV = new Vector3(0, 0, 0);
    public bool drivePowered = true;

    private float currentAccel;



    new void Start()
    {
        transform.LookAt(transform.position + accelVec);

        rb.velocity = iV;
    }



    void FixedUpdate()
    {
        currentAccel = drivePowered ? accel : 0f;

        rb.AddForce(transform.GetChild(0).forward * currentAccel, ForceMode.Acceleration);
    }
}
