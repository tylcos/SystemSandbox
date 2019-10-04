using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class PDC : MonoBehaviour
{
    public GameObject pdcRound;

    public float range                         = 300f;
    private readonly int[] magSizeRange        = { 5, 12 };
    private const float angularVelocity        = 60f;  // Degrees per second
    private const float maxDeadzoneAngle       = 3f;   // Degrees
    private const float maxTargetTransferAngle = 20f;  // Degrees
    private const float maxRecoilAngle         = 3f;   // Degrees

    private readonly HashSet<GameObject> shotTargets = new HashSet<GameObject>();

    private GameObject target;
    private Quaternion targetRot;
    private bool transferingTarget;
    private int roundsRemaining;



    void Start()
    {
        //Time.timeScale = 2;
    }



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

                transferingTarget = Quaternion.Angle(transform.rotation, targetRot) < maxTargetTransferAngle;
                roundsRemaining = Random.Range(magSizeRange[0], magSizeRange[1]);
            }
            else
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.identity, angularVelocity * Time.fixedDeltaTime);
                return;
            }
        }



        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, angularVelocity * Time.deltaTime);

        if (Quaternion.Angle(transform.rotation, targetRot) < maxDeadzoneAngle)
        {
            ShootRound();

            if (--roundsRemaining == 0)
            {
                shotTargets.Add(target);
                target = null; // Stop firing on current target
            }
        }
        else if (transferingTarget)
            ShootRound();
    }

    private void ShootRound()
    {
        transform.rotation *= Quaternion.Euler(Random.Range(-maxRecoilAngle, maxRecoilAngle),
                                               Random.Range(-maxRecoilAngle, maxRecoilAngle), 0);

        var go = Instantiate(pdcRound, transform.position, transform.rotation, transform.parent);
        var id = go.GetComponent<InterceptDrive>();

        id.targetDrive = target.GetComponent<Drive>();
        id.target = target.GetComponent<Rigidbody>();
        id.parent = this;
    }



    internal void TargetHit(GameObject targetHit)
    {
        shotTargets.Remove(targetHit);
    }
}
