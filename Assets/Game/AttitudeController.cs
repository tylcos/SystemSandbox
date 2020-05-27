using System;
using UnityEngine;



public class AttitudeController : MonoBehaviour
{
    public Transform referenceSpace;
    public Rigidbody shipRb;

    public ParticleSystem[] pitchUp = new ParticleSystem[2];
    public ParticleSystem[] pitchDown = new ParticleSystem[2];

    public ParticleSystem[] yawLeft = new ParticleSystem[2];
    public ParticleSystem[] yawRight = new ParticleSystem[2];

    public ParticleSystem[] rollLeft = new ParticleSystem[2];
    public ParticleSystem[] rollRight = new ParticleSystem[2];



    private const float thrusterForce = 100f;



    private void Start()
    {
        ForEach(transform.GetComponentsInChildren<ParticleSystem>(), 
            t => { var main = t.main;
                main.simulationSpace = ParticleSystemSimulationSpace.Custom;
                main.customSimulationSpace = referenceSpace; });

            

        shipRb.maxAngularVelocity = 2f;
    }



    private void FixedUpdate()
    {
        Vector3 attitudeInput = new Vector3(-Input.GetAxisRaw("Roll"), Input.GetAxisRaw("Horizontal"), -Input.GetAxisRaw("Depth"));



        UpdateThrusters(attitudeInput.z, pitchDown, pitchUp);
        UpdateThrusters(attitudeInput.y, yawLeft, yawRight);
        UpdateThrusters(attitudeInput.x, rollRight, rollLeft);



        Vector3 attitudeTorque = Quaternion.LookRotation(transform.forward, transform.up) * attitudeInput;
        shipRb.AddTorque(attitudeTorque * thrusterForce * Time.fixedDeltaTime, ForceMode.Acceleration);
    }



    private void UpdateThrusters(float input, ParticleSystem[] up, ParticleSystem[] down)
    {
        if (input > 0f)
            ForEach(down, t => { if (!t.isPlaying) { t.Play(); } });
        else
            ForEach(down, t => t.Stop());

        if (input < 0f)
            ForEach(up, t => { if (!t.isPlaying) { t.Play(); } });
        else
            ForEach(up, t => t.Stop());
    }

    private void ForEach<T>(T[] array, Action<T> action)
    {
        foreach (T item in array)
            action(item);
    }
}
