using MathNet.Numerics;
using System.Linq;
using UnityEngine;



public static class InterceptSolver
{
    public static float FindRealSolutionSmallestT(double[] coefficients)
    {
        var realSolutions = FindRoots.Polynomial(coefficients).Where(r => r.IsReal() && r.Real > 0).OrderBy(r => r.Real);
        return realSolutions.Any() ? (float)realSolutions.First().Real : float.PositiveInfinity;
    }
}



public static class InterceptSolverNoAccel 
{
    public static float FindRealSolutionSmallestT(InterceptDrive drive, Drive targetDrive)
    {
        if (drive == null || targetDrive == null)
            return float.PositiveInfinity;

        Vector3 rv = targetDrive.rb.velocity - drive.rb.velocity;
        Vector3 rp = targetDrive.rb.position - drive.rb.position;

        double[] coefficients = {
            rp.sqrMagnitude,
            2d * Vector3.Dot(rv, rp),
            Vector3.Dot(targetDrive.accelVec, rp) + rv.sqrMagnitude - drive.speed * drive.speed,
            Vector3.Dot(targetDrive.accelVec, rv),
            .25d * targetDrive.accelVec.sqrMagnitude
        };

        return InterceptSolver.FindRealSolutionSmallestT(coefficients);
    }

    public static float FindRealSolutionSmallestT(Vector3 currentVelocity, Vector3 currentPos, float speed, Drive targetDrive)
    {
        if (targetDrive == null)
            return float.PositiveInfinity;

        Vector3 rv = targetDrive.rb.velocity - currentVelocity;
        Vector3 rp = targetDrive.rb.position - currentPos;

        double[] coefficients = {
            rp.sqrMagnitude,
            2d * Vector3.Dot(rv, rp),
            Vector3.Dot(targetDrive.accelVec, rp) + rv.sqrMagnitude - speed * speed,
            Vector3.Dot(targetDrive.accelVec, rv),
            .25d * targetDrive.accelVec.sqrMagnitude
        };

        return InterceptSolver.FindRealSolutionSmallestT(coefficients);
    }
}



public static class InterceptSolverAccel
{
    public static float FindRealSolutionSmallestT(InterceptDriveAccel drive, Drive targetDrive)
    {
        if (drive == null || targetDrive == null)
            return float.PositiveInfinity;

        Vector3 rv = targetDrive.rb.velocity - drive.rb.velocity;
        Vector3 rp = targetDrive.rb.position - drive.rb.position;

        double[] coefficients = {
            4d * rp.sqrMagnitude,
            8d * Vector3.Dot(rv, rp),
            4d * (Vector3.Dot(targetDrive.accelVec, rp) + rv.sqrMagnitude),
            4d * Vector3.Dot(targetDrive.accelVec, rv),
            targetDrive.accelVec.sqrMagnitude - drive.accel * drive.accel
        };

        return InterceptSolver.FindRealSolutionSmallestT(coefficients);
    }
}
