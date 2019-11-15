using System;
using System.Collections;
using UnityEngine;



public class InterceptDriveAccel : Drive
{
    public ParticleSystem ps;

    public float waitTime = 5f;

    public Drive targetDrive;



    private float lifeTime = 30f;



    new void Start()
    {
        ps.Stop();

        StartCoroutine(StartAccelerating());
    }

    IEnumerator StartAccelerating()
    {
        yield return new WaitForSeconds(waitTime);

        ps.Play();

        float t = InterceptSolverAccel.FindRealSolutionSmallestT(this, targetDrive);
        if (!float.IsInfinity(t))
        {
            //GameObject go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            //go.transform.position = targetDrive.EstimatedPos(t);



            Vector3 rp = targetDrive.EstimatedPos(t) - rb.position;

            Vector3 wastedAccel = -2f * (rb.velocity - Vector3.Project(rb.velocity, rp)) / t; 
            Vector3 towardsTargetAccel = rp.normalized * Mathf.Sqrt(accel * accel - wastedAccel.sqrMagnitude);
            accelVec = wastedAccel + towardsTargetAccel;

            //accelVec = (2f * (rp - rb.velocity * t) / (t * t)).normalized * accel;

            //accelVec = rp.normalized * accel;
            

            transform.rotation = Quaternion.LookRotation(accelVec);
        }
    }



    private void FixedUpdate()
    {
        if ((lifeTime -= Time.fixedDeltaTime) < 0)
            Destroy(gameObject);

        rb.AddForce(accelVec, ForceMode.Acceleration);
    }



    private void OnCollisionEnter(Collision other)
    {
        try
        {
            if (other.gameObject.tag == "torpedo")
            {
                gameObject.GetComponent<Explosion>().SpawnExplosion(rb.velocity);

                Destroy(other.gameObject);
                Destroy(gameObject);
            }
        }
        catch (NullReferenceException)
        {
            print("[InterceptDriveAccel] NRE");
        }
        
    }
}
