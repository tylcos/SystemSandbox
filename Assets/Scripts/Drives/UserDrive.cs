using UnityEngine;



public class UserDrive : Drive
{
    public Vector3 iV = new Vector3(0, 0, 0);

    public Transform drivePlume;



    private float drivePower;
    private const float drivePowerChangeSpeed = .5f;

    private Transform ship;
    private Vector3 drivePlumeSize;




    new void Start()
    {
        ship = transform.GetChild(0);
        drivePlumeSize = drivePlume.localScale;



        transform.LookAt(transform.position + accelVec);

        rb.velocity = iV;
    }



    void FixedUpdate()
    {
        if      (Input.GetKey(KeyCode.LeftShift))
            drivePower += drivePowerChangeSpeed * Time.fixedDeltaTime;
        else if (Input.GetKey(KeyCode.LeftControl))
            drivePower -= drivePowerChangeSpeed * Time.fixedDeltaTime;

        drivePower = Mathf.Clamp(drivePower, 0f, 1f);
            


        if (drivePower > .3f)
        {
            drivePlume.localScale = drivePlumeSize * drivePower;



            float currentAccel = accel * drivePower * Time.fixedDeltaTime;

            rb.AddForce(ship.forward * currentAccel, ForceMode.Acceleration);
        }
        else
            drivePlume.localScale = Vector3.zero;
    }
}
