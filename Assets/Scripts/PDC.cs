using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;



public class PDC : MonoBehaviour
{
    public GameObject pdcRound;
    public Transform roundSpawnPoint;

    private Drive parentDrive;
    private PDCController parentPDCContoller;

    private readonly int[] magSizeRange        = { 5, 12 };
    private const float angularVelocity        = 60f;  // Degrees per second
    private const float maxDeadzoneAngle       = 1f;   // Degrees
    private const float maxTargetTransferAngle = 10;  // Degrees
    private const float maxRecoilAngle         = 2f;  // Degrees

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
        {
            target = Physics.OverlapSphere(transform.position, range)
                .Where(t => t.tag == "torpedo" && !shotTargets.Contains(t.gameObject))
                .OrderBy(t => (t.transform.position - transform.position).sqrMagnitude).FirstOrDefault()?.gameObject;

            // Could loop until a interceptable target is found but is unnecessary
            if (target != null) // New target acqusition
            {
                targetDrive = target.GetComponent<Drive>();
                updateEstimatedIntercept(); 

                transferingTarget = Quaternion.Angle(transform.rotation, targetRot) < maxTargetTransferAngle;
                roundsRemaining = Random.Range(magSizeRange[0], magSizeRange[1]);
            }
            else
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.identity, angularVelocity * Time.fixedDeltaTime);

            return;
        }



        updateEstimatedIntercept();
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, angularVelocity * Time.deltaTime);

        if (Quaternion.Angle(transform.rotation, targetRot) < maxDeadzoneAngle)
        {
            ShootRound(true);

            if (--roundsRemaining == 0)
            {
                shotTargets.Add(target);
                target = null; // Stop firing on current target
            }
        }
        else if (transferingTarget)
            ShootRound(false);



        void updateEstimatedIntercept()
        {
            float predictedT = InterceptSolverNoAccel.FindRealSolutionSmallestT(parentDrive.rb.velocity, roundSpawnPoint.position, pdcRoundSpeed, targetDrive);
            if (float.IsInfinity(predictedT))
            {
                target = null;
                return;
            }

            Vector3 rp = targetDrive.EstimatedPos(predictedT) - roundSpawnPoint.position;

            Vector3 projectedWastedVel = Vector3.Project(parentDrive.rb.velocity, rp);
            Vector3 wastedVel = -(parentDrive.rb.velocity - projectedWastedVel);
            Vector3 towardsTargetVel = rp.normalized * Mathf.Sqrt(pdcRoundSpeed * pdcRoundSpeed - wastedVel.sqrMagnitude);

            targetRot = Quaternion.LookRotation(wastedVel + towardsTargetVel);
        }
    }

    private void ShootRound(bool hasTarget)
    {
        transform.rotation *= Quaternion.Euler((Vector3)Random.insideUnitCircle * maxRecoilAngle);

        var spawnedRound = Instantiate(pdcRound, roundSpawnPoint.position, transform.rotation, GameManager.WorldTransform);
        var spawnedDrive = spawnedRound.GetComponent<InterceptDrive>();

        spawnedDrive.shotTargets = shotTargets;
        spawnedDrive.targetDrive = hasTarget ? targetDrive : null;
        spawnedDrive.rb.velocity = parentDrive.rb.velocity;

        spawnedDrive.Initialize();
    }
}
