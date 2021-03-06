﻿using System.Collections.Generic;
using UnityEngine;



public class InterceptDrive : Drive
{
    public HashSet<GameObject> shotTargets;

    public Drive targetDrive;

    public float speed = 100f;



    private float lifeTime = 10f;



    new void Start()
    {
        base.Start();

        float t = InterceptSolverNoAccel.FindRealSolutionSmallestT(this, targetDrive); //print(t + Time.time);
        if (targetDrive == null || float.IsInfinity(t)) // Shooting at nothing or cannot hit the target
        {
            rb.velocity += transform.forward.normalized * speed;
            return;
        }



        Vector3 rp = targetDrive.EstimatedPos(t) - rb.position;

        Vector3 projectedWastedVel = Vector3.Project(rb.velocity, rp);
        Vector3 wastedVel = -(rb.velocity - projectedWastedVel);
        Vector3 towardsTargetVel = rp.normalized * Mathf.Sqrt(speed * speed - wastedVel.sqrMagnitude);
        Vector3 resultVel = wastedVel + towardsTargetVel;



        rb.velocity += resultVel;
        transform.rotation = Quaternion.LookRotation(resultVel);


        /*
        var g = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        g.transform.position = targetDrive.EstimatedPos(t);
        Destroy(g.GetComponent<SphereCollider>()); */
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
            shotTargets.Remove(other.gameObject);
            GameManager.TorpedosDestroyed++;

            gameObject.GetComponent<Explosion>().SpawnExplosion(rb.velocity);
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
}
