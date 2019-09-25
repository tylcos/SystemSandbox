using UnityEngine;



public class BodyProperties : MonoBehaviour
{
    public float mass;



    public float radius;



    public new Transform transform;
    public Vector3 velocity = new Vector3();



    private void Start()
    {
        transform = base.transform;
    }
}
