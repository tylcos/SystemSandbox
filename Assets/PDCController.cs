using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using JM.LinqFaster;
using System;
using System.Diagnostics;

public class PDCController : MonoBehaviour
{
    PDC[] pdcs;

    private const float range = 600f;

    public readonly HashSet<GameObject> shotTargets = new HashSet<GameObject>();



    void Start()
    {
        pdcs = GetComponentsInChildren<PDC>();
    }

    void FixedUpdate()
    { 
        Stopwatch sw = Stopwatch.StartNew();

        Collider[] targets = Physics.OverlapSphere(transform.position, range)
                .WhereF(t => t.tag == "torpedo" && !shotTargets.Contains(t.gameObject));

        if (targets.FirstOrDefaultF() != null)
        {
            PDC[] availablePDCs = pdcs.WhereF(p => p.target == null);
            var computedTargets = availablePDCs.Select(p =>
                targets.SelectF(t => new { Distance = (t.transform.position - p.transform.position).sqrMagnitude, Target = t })
                .OrderBy(t => t.Distance)).ToList();



            List<Collider> removedTargets = new List<Collider>();

            for (int i = 0; i < availablePDCs.Length && removedTargets.Count != targets.Length; i++)
            {
                //print($"[PDC] Targets = {targets.Length}, removed = {String.Join(" ", removedTargets.SelectF(t => t.name))}");
                var (index, value) = MinIndex(computedTargets, tList => tList.First(t => !removedTargets.Contains(t.Target)).Distance);
                Collider minTarget = value.First().Target;

                availablePDCs[index].target = minTarget.gameObject;
                shotTargets.Add(minTarget.gameObject);

                computedTargets.RemoveAt(index);
                removedTargets.Add(minTarget);
            }

            print("Target computing time " + sw.ElapsedTicks / 100 + "µs");
        }


        foreach (PDC pdc in pdcs)
            pdc.PDCUpdate();



        (int index, T value) MinIndex<T> (List<T> source, Func<T, float> selector)
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

            return (minIndex, source[minIndex]);
        }
    }
}
