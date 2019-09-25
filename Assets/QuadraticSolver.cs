using System;
using System.Linq;
using UnityEngine;

class QuadraticSolver
{
    public static float[] SolveQuadratic(float a, float b, float c)
    {
        float insideSquareRoot = (b * b) - 4 * a * c;

        if (insideSquareRoot < 0)
            return new float[] { float.NaN, float.NaN };
        else
        {
            float t = (float)(-0.5f * (b + Math.Sign(b) * Math.Sqrt(insideSquareRoot)));
            return new float[] { c / t, t / a };
        }
    }
}