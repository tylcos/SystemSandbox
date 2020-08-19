using UnityEngine;

public class TestObject : MonoBehaviour
{
    private readonly Vector2 Velocity = new Vector2(20f, 1f);



    void Update()
    {
        transform.position += (Vector3)MetricSpace.DeltaPos(transform.position, Velocity);
    }
}
