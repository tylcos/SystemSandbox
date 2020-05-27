using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;



public class Drawer : MonoBehaviour
{
    public Dimension[] dimensions = new Dimension[3];
    public float[] extraDimensions = new float[1];

    public GameObject point;



    private GameObject child;
    private string hash;

    private float minDis, maxDis, dDis;

    private int pointCount;



    void Start()
    {
        foreach (Transform child in transform)
            Destroy(child.gameObject);
        child = Instantiate(new GameObject(), transform);

        hash = CalculateHash(dimensions) + CalculateHash(extraDimensions);
        minDis = 0;
        maxDis = 0;
        pointCount = 0;
        Draw();
    }

    void Update()
    {
        if (CalculateHash(dimensions) + CalculateHash(extraDimensions) != hash)
            Start();
    }



    void Draw()
    {
        int[] pointCounts = dimensions.Select(d => (int)((d.Max - d.Min) / d.Step) + 1).ToArray();
        Vector3[,,] points = new Vector3[pointCounts[0], pointCounts[1], pointCounts[2]];
        Vector3[,,] transformedPoints = new Vector3[pointCounts[0], pointCounts[1], pointCounts[2]];


        float xP = dimensions[0].Min;
        for (int x = 0; x < pointCounts[0]; x++)
        {
            float yP = dimensions[1].Min;
            for (int y = 0; y < pointCounts[1]; y++)
            {
                float zP = dimensions[2].Min;
                for (int z = 0; z < pointCounts[2]; z++)
                {
                    Vector3 parameters = new Vector3(xP, (float)(yP * MathNet.Numerics.Constants.Degree), (float)(zP * MathNet.Numerics.Constants.Degree));

                    points[x, y, z] = parameters;
                    transformedPoints[x, y, z] = Transformation(parameters);
                    pointCount++;

                    zP += dimensions[2].Step;
                }

                yP += dimensions[1].Step;
            }

            xP += dimensions[0].Step;
        }

        if (pointCount > 10000)
        {
            print("To many points");
            return;
        }



        for (int x = 0; x < pointCounts[0]; x++)
        {
            for (int y = 0; y < pointCounts[1]; y++)
            {
                for (int z = 0; z < pointCounts[2]; z++)
                {
                    if (x < pointCounts[0] - 1)
                        UpdateDistance(points[x, y, z], points[x + 1, y, z], transformedPoints[x, y, z], transformedPoints[x + 1, y, z]);
                    if (y < pointCounts[1] - 1)
                        UpdateDistance(points[x, y, z], points[x, y + 1, z], transformedPoints[x, y, z], transformedPoints[x, y + 1, z]);
                    if (z < pointCounts[2] - 1)
                        UpdateDistance(points[x, y, z], points[x, y, z + 1], transformedPoints[x, y, z], transformedPoints[x, y, z + 1]);
                }
            }
        }


        print(maxDis + "   " + minDis);
        dDis = maxDis - minDis;
        for (int x = 0; x < pointCounts[0]; x++)
        {
            for (int y = 0; y < pointCounts[1]; y++)
            {
                for (int z = 0; z < pointCounts[2]; z++)
                {
                    if (x < pointCounts[0] - 1)
                        CreateLine(points[x, y, z], points[x + 1, y, z], transformedPoints[x, y, z], transformedPoints[x + 1, y, z]);
                    if (y < pointCounts[1] - 1)
                        CreateLine(points[x, y, z], points[x, y + 1, z], transformedPoints[x, y, z], transformedPoints[x, y + 1, z]);
                    if (z < pointCounts[2] - 1)
                        CreateLine(points[x, y, z], points[x, y, z + 1], transformedPoints[x, y, z], transformedPoints[x, y, z + 1]);
                }
            }
        }



        print(pointCount + " Total Points");
    }

    void UpdateDistance(Vector3 start, Vector3 end, Vector3 startT, Vector3 endT)
    {
        float dis = CalculateDistance(start, end, startT, endT);

        if (dis < minDis)
            minDis = dis;
        else if (dis > maxDis)
            maxDis = dis;
    }

    void CreateLine(Vector3 start, Vector3 end, Vector3 startT, Vector3 endT)
    {
        GameObject currentPoint = Instantiate(point, Vector3.zero, Quaternion.identity, child.transform);
        LineRenderer currentLr = currentPoint.GetComponent<LineRenderer>();

        currentLr.positionCount = 2;
        currentLr.SetPositions(new Vector3[] { startT, endT });

        float dis = (CalculateDistance(start, end, startT, endT) - minDis) / dDis;
        Color color = Color.HSVToRGB(dis / 2, 1, 1);
        currentLr.startColor = color;
        currentLr.endColor = color;
    }



    Vector3 Transformation(Vector3 parameters)
    {
        //return parameters;



        
        double r = parameters.x;
        double p = parameters.y; 
        double t = parameters.z;

        return new Vector3(
            (float)(r * Math.Sin(p) * Math.Cos(t)),
            (float)(r * Math.Cos(p)),
            (float)(r * Math.Sin(p) * Math.Sin(t))
        );
    }

    float CalculateDistance(Vector3 start, Vector3 end, Vector3 startT, Vector3 endT)
    {
        float cDis = (startT - endT).magnitude;
        if (Math.Abs(cDis) < .001)
            return 1;

        Vector3 d = start - end;
        float temp = extraDimensions[0] * extraDimensions[0] + (float)Math.Pow((start.x + end.x) / 2, 2);
        return ((float)Math.Sqrt(d.sqrMagnitude + temp * (d.y * d.y + Math.Pow(Math.Sin(d.y), 2) * d.z * d.z))) / cDis;
    }



    string CalculateHash<T>(params IEnumerable<T>[] es)
    {
        string hash = "";

        foreach (var e in es)
            hash += string.Join("", e.Select(d => d.GetHashCode()));

        return hash;
    }
}
