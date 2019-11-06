using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;



public class AttitudeController : MonoBehaviour
{
    public Transform referenceSpace;

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



    private void Update()
    {
        UpdateThrusters("Depth", pitchUp, pitchDown);
        UpdateThrusters("Horizontal", yawLeft, yawRight);
        UpdateThrusters("Roll", rollLeft, rollRight);
    }



    private void UpdateThrusters(string inputAxis, ParticleSystem[] up, ParticleSystem[] down)
    {
        if (Input.GetAxisRaw(inputAxis) > 0f)
            ForEach(down, t => { if (!t.isPlaying) { t.Play(); } });
        else
            ForEach(down, t => t.Stop());

        if (Input.GetAxisRaw(inputAxis) < 0f)
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
