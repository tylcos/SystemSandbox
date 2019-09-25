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

    public bool chooseMin = true;



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

        var roots = FindRoots.Polynomial(coefficients);

        //foreach (var root in roots)
        //    print(root);

        var reals = roots.Where(r => r.IsReal() && r.Real > 0).ToArray();
        if (reals.Length > 0)
        {
            float t = chooseMin ? (float)reals.Min(r => r.Real) : (float)reals.Max(r => r.Real);

            Vector3 interceptPos = .5f * targetDrive.accel * t * t + target.velocity * t + target.position;

            rb.velocity = (interceptPos - rb.position).normalized * speed;
            transform.LookAt(interceptPos);

            GameObject.CreatePrimitive(PrimitiveType.Sphere).transform.position = interceptPos;
        }
        else
            print("Target unreachable");



        /*
        
        float a = targetDrive.accel.magnitude / 2f;
        float b = target.velocity.magnitude - speed;
        float c = target.position.magnitude;

        var zeros = QuadraticSolver.SolveQuadratic(a, b, c);
        //float t = zeros[0] < 0 ? zeros[1] : zeros[0];

        //Vector3 interceptPos = targetDrive.accel * t * t + target.velocity * t + target.position;

        //rb.velocity = interceptPos.normalized * speed;
        


        //print(a + " " + b + " " + c);
        print(zeros[0] + " " + zeros[1]);



        //GameObject.CreatePrimitive(PrimitiveType.Sphere).transform.position = interceptPos;
        */
    }



    void FixedUpdate()
    {
        if ((target.position - rb.position).magnitude < .1f)
            print(Time.fixedTime);
    }
}
