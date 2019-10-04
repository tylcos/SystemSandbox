using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MathNet.Numerics;
using System.Linq;

public class InterceptDrive : MonoBehaviour
{
    public Rigidbody rb;

    public Drive targetDrive;
    public Rigidbody target;

    public float speed = 20f;

    public PDC parent;

    int i = 0;


    void Start()
    {
        Vector3 rv = target.velocity - rb.velocity;
        Vector3 rp = target.position - rb.position;

        double[] coefficients = {
            rp.sqrMagnitude,
            2d * Vector3.Dot(rv, rp),
            Vector3.Dot(targetDrive.accel, rp) + rv.sqrMagnitude - speed * speed,
            Vector3.Dot(targetDrive.accel, rv),
            .25d * targetDrive.accel.sqrMagnitude
        };

        var realSolutions = FindRoots.Polynomial(coefficients).Where(r => r.IsReal() && r.Real > 0);
        if (realSolutions.Any())
        {
            float t = (float)realSolutions.Min(r => r.Real);

            Vector3 interceptPos = .5f * targetDrive.accel * t * t + target.velocity * t + target.position;

            rb.velocity = (interceptPos - rb.position).normalized * speed;
            transform.LookAt(interceptPos);
        }
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "torpedo")
        {
            parent.TargetHit(other.gameObject);

            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
}
