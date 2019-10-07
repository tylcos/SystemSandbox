using MathNet.Numerics;
using System.Linq;
using UnityEngine;



public static class EquationSolver
{
    public static float FindRealSolutionSmallestT(double[] coefficients)
    {
        var realSolutions = FindRoots.Polynomial(coefficients).Where(r => r.IsReal() && r.Real > 0).OrderBy(r => r.Real);
        return realSolutions.Any() ? (float)realSolutions.First().Real : float.PositiveInfinity;
    }

    public static float FindRealSolutionSmallestT(InterceptDrive drive, Drive targetDrive)
    {
        Vector3 rv = targetDrive.rb.velocity - drive.rb.velocity;
        Vector3 rp = targetDrive.rb.position - drive.rb.position;

        double[] coefficients = {
            rp.sqrMagnitude,
            2d * Vector3.Dot(rv, rp),
            Vector3.Dot(targetDrive.accel, rp) + rv.sqrMagnitude - drive.speed * drive.speed,
            Vector3.Dot(targetDrive.accel, rv),
            .25d * targetDrive.accel.sqrMagnitude
        };

        return FindRealSolutionSmallestT(coefficients);
    }

    public static float FindRealSolutionSmallestT(Vector3 currentVelocity, Vector3 currentPos, float speed, Drive targetDrive)
    {

        Vector3 rv = targetDrive.rb.velocity - currentVelocity;
        Vector3 rp = targetDrive.rb.position - currentPos;

        double[] coefficients = {
            rp.sqrMagnitude,
            2d * Vector3.Dot(rv, rp),
            Vector3.Dot(targetDrive.accel, rp) + rv.sqrMagnitude - speed * speed,
            Vector3.Dot(targetDrive.accel, rv),
            .25d * targetDrive.accel.sqrMagnitude
        };

        return FindRealSolutionSmallestT(coefficients);
    }
}
