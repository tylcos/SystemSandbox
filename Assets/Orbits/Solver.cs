using UnityEngine;
using TMPro;



public class Solver : MonoBehaviour
{
    public BodyProperties sun;
    public BodyProperties earth;



    public OrbitProperties orbit = new OrbitProperties();



    public TMP_Text text;
    public LineRenderer lr;
    public int index;



    public float SimSpeed = 1f;



    void Start()
    {
        orbit.SemiMajorAxis = 0;
        
    }



    void Update()
    {
        SimSpeed += Input.GetAxis("Mouse ScrollWheel");
        text.text = "Sim Speed = " + SimSpeed;
    }



    void FixedUpdate()
    {
        float force = earth.mass * sun.mass / SqrDistance;
        earth.velocity += -RelativePos.normalized * force * SimSpeed;
        earth.transform.position += earth.velocity * Time.fixedDeltaTime * SimSpeed;



        Debug.Log(earth.velocity.magnitude);

        lr.positionCount++;
        lr.SetPosition(index++, earth.transform.position);
    }



    Vector3 RelativePos => earth.transform.position - sun.transform.position;
    float SqrDistance => Vector3.SqrMagnitude(RelativePos);
}
