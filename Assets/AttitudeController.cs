using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;



public class AttitudeController : MonoBehaviour
{
    public ParticleSystem[] pitchUp = new ParticleSystem[2];
    public ParticleSystem[] pitchDown = new ParticleSystem[2];

    public ParticleSystem[] rollLeft = new ParticleSystem[2];
    public ParticleSystem[] rollRight = new ParticleSystem[2];

    public ParticleSystem[] yawLeft = new ParticleSystem[2];
    public ParticleSystem[] yawRight = new ParticleSystem[2];


    private float emissionRate;


    private void Start()
    {
        emissionRate = pitchUp[0].emissionRate;
    }



    private void Update()
    {
        if (Input.GetAxisRaw("Depth") > 0f)
            ForEach(pitchDown, t => t.Play(), t => t.emissionRate = emissionRate);
        else
            ForEach(pitchDown, t => t.Pause(), t => t.emissionRate = 0f);

        if (Input.GetAxisRaw("Depth") < 0f)
            ForEach(pitchUp, t => t.Play(), t => t.emissionRate = emissionRate);
        else
            ForEach(pitchUp, t => t.Pause(), t => t.emissionRate = 0f);

    }



    private void ForEach<T>(T[] array, params Action<T>[] actions)
    {
        foreach (T item in array)
            foreach (var action in actions)
                action(item);

    }
}
