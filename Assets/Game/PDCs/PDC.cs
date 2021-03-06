﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;



public class PDC : MonoBehaviour
{
    public GameObject pdcRound;
    public Transform roundSpawnPoint;

    private Drive parentDrive;
    private PDCController parentPDCContoller;

    private readonly int[] magSizeRange        = { 15, 30 };
    private const float angularVelocity        = 150f; // Degrees per second
    private const float maxDeadzoneAngle       = 1f;   // Degrees
    private const float maxTargetTransferAngle = 15f;  // Degrees
    private const float maxRecoilAngle         = 2f;   // Degrees

    internal GameObject target;
    private Drive targetDrive;
    private Quaternion targetRot;
    private bool transferingTarget;
    private int roundsRemaining;

    private float pdcRoundSpeed;
    private HashSet<GameObject> shotTargets;



    void Start()
    {
        //Time.timeScale = 2;
        parentDrive = transform.GetComponentInParent<Drive>();
        parentPDCContoller = transform.GetComponentInParent<PDCController>();
        shotTargets = parentPDCContoller.shotTargets;

        pdcRoundSpeed = pdcRound.GetComponent<InterceptDrive>().speed;
    }



    public void PDCUpdate()
    {
        if (target == null) // No current target
            return;

        if (target != null && targetDrive == null) // New target acqusition
        {
            targetDrive = target.GetComponent<Drive>();
            updateEstimatedIntercept();

            transferingTarget = Quaternion.Angle(transform.rotation, targetRot) < maxTargetTransferAngle;
            roundsRemaining = Random.Range(magSizeRange[0], magSizeRange[1]);
        }



        updateEstimatedIntercept(); 
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, angularVelocity * Time.deltaTime);
        transform.rotation = Quaternion.LookRotation(transform.forward, transform.parent.up); // Prevent weird rotations

        if (parentPDCContoller.TargetInRange(transform, target)) // Only shoot when target is in range
        {
            if (Quaternion.Angle(transform.rotation, targetRot) < maxDeadzoneAngle)
            {
                ShootRound(true);

                if (--roundsRemaining == 0) // Stop firing on current target, could also stop from round destroying target
                    ClearTarget();
            }
            else if (transferingTarget) // Shooting just for looks
                ShootRound(false);
        }



        void updateEstimatedIntercept()
        {
            float predictedT = InterceptSolverNoAccel.FindRealSolutionSmallestT(
                parentDrive.rb.velocity, roundSpawnPoint.position, pdcRoundSpeed, targetDrive);

            if (float.IsInfinity(predictedT)) // Can never hit the target
            {
                ClearTarget();
                return;
            }
                


            // 99% Sure these calculations are slightly off
            Vector3 rp = targetDrive.EstimatedPos(predictedT) - roundSpawnPoint.position;

            Vector3 projectedWastedVel = Vector3.Project(parentDrive.rb.velocity, rp);
            Vector3 wastedVel = -(parentDrive.rb.velocity - projectedWastedVel);
            Vector3 towardsTargetVel = rp.normalized * Mathf.Sqrt(pdcRoundSpeed * pdcRoundSpeed - wastedVel.sqrMagnitude);

            targetRot = Quaternion.LookRotation(wastedVel + towardsTargetVel, transform.parent.up);
        }
    }

    private void ShootRound(bool roundHasTarget)
    {
        transform.rotation *= Quaternion.Euler((Vector3)Random.insideUnitCircle * maxRecoilAngle);

        var spawnedRound = Instantiate(pdcRound, roundSpawnPoint.position, transform.rotation, GameManager.WorldTransform);
        var spawnedDrive = spawnedRound.GetComponent<InterceptDrive>();

        spawnedDrive.shotTargets = shotTargets;
        spawnedDrive.targetDrive = roundHasTarget ? targetDrive : null;
        spawnedDrive.rb.velocity = parentDrive.rb.velocity;
    }



    private void ClearTarget()
    {
        target = null;
        targetDrive = null;
        targetRot = Quaternion.LookRotation(transform.parent.forward, transform.parent.up);
    }
}
