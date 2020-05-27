using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using JM.LinqFaster;
using System;
using System.Diagnostics;



public class PDCController : MonoBehaviour
{
    PDC[] pdcs;

    private const float effectivePDCRange = 1250f;
    private const float detectionRange    = 1500f;

    internal readonly HashSet<GameObject> shotTargets = new HashSet<GameObject>();



    void Start()
    {
        pdcs = GetComponentsInChildren<PDC>();
    }



    void FixedUpdate()
    { 
        Collider[] targets = Physics.OverlapSphere(transform.position, detectionRange)
                .WhereF(t => t.tag == "torpedo" && !shotTargets.Contains(t.gameObject));

        if (targets.FirstOrDefaultF() != null)
        {
            Stopwatch sw = Stopwatch.StartNew();

            PDC[] availablePDCs = pdcs.WhereF(p => p.target == null);

            // Computes the distance to each target for each PDC. List<Enumerable<Anonymous TargetInfo>>
            var computedTargets = availablePDCs.Select(p =>
                targets.SelectF(t => new { Dis = Distance(p, t), Target = t })
                .OrderBy(t => t.Dis)).ToList();



            List<Collider> removedTargets = new List<Collider>(); // Used to prevent 2 PDCs from targeting the same target

            // Assigns the closest target to each PDC, a perfect metric would include the target velocity and targeting time for the PDC
            for (int i = 0; i < availablePDCs.Length && removedTargets.Count < targets.Length; i++)
            {
                var bestTargets = computedTargets.Select(tInfos => tInfos.First(t => !removedTargets.Contains(t.Target))).ToList();
                int indexMinDis = MinIndex(bestTargets, t => t.Dis);
                Collider minTarget = bestTargets[indexMinDis].Target;

                availablePDCs[indexMinDis].target = minTarget.gameObject;
                shotTargets.Add(minTarget.gameObject);

                computedTargets.RemoveAt(indexMinDis); // Remove selected PDC from future calculations
                removedTargets.Add(minTarget);
            }

            print("Target computing time " + sw.ElapsedTicks / 100 + "µs");
        }


        foreach (PDC pdc in pdcs)
            pdc.PDCUpdate();



        // Returns the index of the minimum value in the source based on the given projection
        int MinIndex<T> (List<T> source, Func<T, float> selector)
        {
            float min = selector(source[0]);
            int minIndex = 0;

            for (int i = 1; i < source.Count; i++)
            {
                float current = selector(source[i]);
                if (current < min)
                {
                    min = current;
                    minIndex = i;
                }
            }

            return minIndex;
        }
    }



    public float Distance(Component t1, Component t2) => (t1.transform.position - t2.transform.position).sqrMagnitude;

    public bool TargetInRange(Transform pdc, GameObject target)
        => (target.transform.position - pdc.position).magnitude < effectivePDCRange;
}
