using UnityEngine;



public class SimpleDrive : Drive
{
    public float a = 5f;
    public Vector3 iV = new Vector3(0, 0, 0);



    new void Start()
    {
        transform.LookAt(transform.position + accel);

        rb.velocity = iV;
    }



    void FixedUpdate()
    {
        rb.AddForce(accel, ForceMode.Acceleration);
    }
}
