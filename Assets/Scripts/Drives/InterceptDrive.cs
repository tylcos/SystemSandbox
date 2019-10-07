using UnityEngine;



public class InterceptDrive : Drive
{
    public PDC parent;

    public Drive targetDrive;

    public float speed = 100f;



    private float lifeTime = 30f;



    public void Initialize()
    {
        base.Start();

        if (targetDrive == null)
        {
            rb.velocity = transform.forward.normalized * speed;
            return;
        }



        float t = InterceptSolverNoAccel.FindRealSolutionSmallestT(this, targetDrive); print(t + Time.time);
        if (!float.IsInfinity(t))
        {
            Vector3 rp = targetDrive.EstimatedPos(t) - rb.position;

            // PROBLEM
            Vector3 projectedWastedVel = Vector3.Project(rb.velocity, rp);
            Vector3 wastedVel = -(rb.velocity - projectedWastedVel);
            Vector3 towardsTargetVel = rp.normalized * speed;

            print(wastedVel.magnitude);
            print(towardsTargetVel.magnitude);
            print((wastedVel + towardsTargetVel).magnitude);

            rb.velocity += wastedVel + towardsTargetVel;
            transform.rotation = Quaternion.LookRotation(rp);



            var g = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            g.transform.position = targetDrive.EstimatedPos(t);
            Destroy(g.GetComponent<SphereCollider>());
        }
    }



    private void FixedUpdate()
    {
        if ((lifeTime -= Time.fixedDeltaTime) < 0)
            Destroy(gameObject);
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "torpedo")
        {
            parent?.TargetHit(other.gameObject);

            gameObject.GetComponent<Explosion>().SpawnExplosion();
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
}
