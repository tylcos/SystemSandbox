using UnityEngine;



public class SimpleDrive : Drive
{
    public Vector3 iV = new Vector3(0, 0, 0);



    new void Start()
    {
        transform.LookAt(transform.position + accelVec);

        rb.velocity = iV;
    }



    void FixedUpdate()
    {
        rb.AddForce(accelVec, ForceMode.Acceleration);
    }
}
