using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class PDC : MonoBehaviour
{
    public GameObject pdcRound;

    public float range = 300f;
    private int[] magSizeRange = { 2, 8 };
    private const float angularVelocity = 120f; // Degrees per second
    private const float maxAngleDeadzone = 3f; // Degrees
    private const float maxRecoilAngle = 3f; // Degrees

    private readonly HashSet<GameObject> shotTargets = new HashSet<GameObject>();

    private GameObject target;
    private Quaternion targetRot;
    private int roundsRemaining;

    void FixedUpdate()
    {
        if (target == null) // No current target
        {
            target = Physics.OverlapSphere(transform.position, range)
                .Where(t => t.tag == "torpedo" && !shotTargets.Contains(t.gameObject))
                .OrderBy(t => (t.transform.position - transform.position).sqrMagnitude).FirstOrDefault()?.gameObject;

            if (target != null) // New target acqusition
            {
                Vector3 relativePos = target.transform.position - transform.position;
                targetRot = Quaternion.LookRotation(relativePos);

                roundsRemaining = Random.Range(magSizeRange[0], magSizeRange[1]);
                print(roundsRemaining);
            }
            else
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.identity, angularVelocity * Time.fixedDeltaTime);
                return;
            }
        }



        if (Quaternion.Angle(transform.rotation, targetRot) < maxAngleDeadzone)
        {
            transform.rotation *= Quaternion.Euler(Random.Range(-maxRecoilAngle, maxRecoilAngle), 
                                                   Random.Range(-maxRecoilAngle, maxRecoilAngle), 0);

            var go = Instantiate(pdcRound, transform.position, transform.rotation, transform.parent);
            var id = go.GetComponent<InterceptDrive>();

            id.targetDrive = target.GetComponent<Drive>();
            id.target = target.GetComponent<Rigidbody>();
            id.parent = this;

            if (--roundsRemaining == 0)
            {
                shotTargets.Add(target);
                target = null; // Stop firing on current target
            }
        }
        else
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, angularVelocity * Time.deltaTime);
    }

    internal void TargetHit(GameObject target)
    {
        shotTargets.Remove(target);
    }
}
