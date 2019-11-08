using System;
using UnityEngine;



public class AttitudeController : MonoBehaviour
{
    public Transform referenceSpace;
    public Transform ship;

    public ParticleSystem[] pitchUp = new ParticleSystem[2];
    public ParticleSystem[] pitchDown = new ParticleSystem[2];

    public ParticleSystem[] yawLeft = new ParticleSystem[2];
    public ParticleSystem[] yawRight = new ParticleSystem[2];

    public ParticleSystem[] rollLeft = new ParticleSystem[2];
    public ParticleSystem[] rollRight = new ParticleSystem[2];



    private void Start()
    {
        ForEach(transform.GetComponentsInChildren<ParticleSystem>(), 
            t => { var main = t.main;
                main.simulationSpace = ParticleSystemSimulationSpace.Custom;
                main.customSimulationSpace = referenceSpace; });
    }



    private void FixedUpdate()
    {
        Vector3 attitudeInput = new Vector3(Input.GetAxisRaw("Depth"), Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Roll"));

        UpdateThrusters(attitudeInput.x, pitchUp, pitchDown);
        UpdateThrusters(attitudeInput.y, yawLeft, yawRight);
        UpdateThrusters(attitudeInput.z, rollRight, rollLeft);



        Quaternion attitudeChange = ship.rotation * Quaternion.Euler(attitudeInput * 10);
        ship.rotation = Quaternion.RotateTowards(ship.rotation, attitudeChange, 60 * Time.fixedDeltaTime);
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
