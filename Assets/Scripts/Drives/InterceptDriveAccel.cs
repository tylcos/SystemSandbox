using System.Collections;
using UnityEngine;



public class InterceptDriveAccel : Drive
{
    public float waitTime = 5f;

    public Drive targetDrive;



    private float lifeTime = 120f;



    new void Start()
    {
        StartCoroutine(StartAccelerating());
    }

    IEnumerator StartAccelerating()
    {
        yield return new WaitForSeconds(waitTime);

        float t = InterceptSolverAccel.FindRealSolutionSmallestT(this, targetDrive);
        if (!float.IsInfinity(t))
        {
            Vector3 rp = targetDrive.EstimatedPos(t) - rb.position;

            Vector3 wastedAccel = -2f * (rb.velocity - Vector3.Project(rb.velocity, rp)) / t; 
            Vector3 towardsTargetAccel = rp.normalized * Mathf.Sqrt(accel * accel - wastedAccel.sqrMagnitude);

            accelVec = wastedAccel + towardsTargetAccel;
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
        if (other.gameObject.tag == "torpedo")
        {
            gameObject.GetComponent<Explosion>().SpawnExplosion();
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
}
