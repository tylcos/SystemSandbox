using UnityEngine;
using MathNet.Numerics.LinearAlgebra;

public static class MetricSpace
{
    static Vector<float> Metric(Vector2 pos) => Vector<float>.Build.Dense(new float[] { pos.x * pos.x + 1, 1000f }).PointwiseAbs();

    public static Vector2 DeltaPos(Vector2 pos, Vector2 vel)
    {
        var deltaPos = Convert(vel * Time.deltaTime);
        var deltaPosMetric = deltaPos.PointwiseMultiply(Metric(pos));

        var deltaPosRatio = deltaPos.PointwiseDivide(deltaPosMetric);
        var trueDeltaPos = deltaPosRatio.PointwiseMultiply(deltaPos);

        return Convert(trueDeltaPos);
    }



    public static Vector<float> Convert(Vector2 v) => Vector<float>.Build.Dense(new float[] { v.x, v.y });
    public static Vector2 Convert(Vector<float> v) => new Vector2(v[0], v[1]);
}
