using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PDCController : MonoBehaviour
{
    PDC[] pdcs;
    Vector3[] pdcPositions;

    private const float range = 600f;

    public readonly HashSet<GameObject> shotTargets = new HashSet<GameObject>();



    void Start()
    {
        pdcs = GetComponentsInChildren<PDC>();
        pdcPositions = pdcs.Select(p => p.transform.position).ToArray();
    }

    void FixedUpdate()
    {
        var targets = Physics.OverlapSphere(transform.position, range)
                .Where(t => t.tag == "torpedo" && !shotTargets.Contains(t.gameObject));

        var availablePDCs = pdcs.Where(p => p.target == null);
        var distances = availablePDCs.Select(p => targets.Select(t =>
                (t.transform.position - p.transform.position).sqrMagnitude)).ToArray();

        List<int> remainingPDCIndexes = Enumerable.Range(0, distances.Length).ToList();

        for (int i = 0; i < distances.Length; i++)
        {
            if (!remainingPDCIndexes.Any())
            {
                pdcs[i] = null;
                return;
            }

            int minIndex = 0;
            int minValue;

            for (int p = 0; p < distances.Length; p++)
            {
                

            }

        }



        foreach (PDC pdc in pdcs)
            pdc.PDCUpdate();
    }
}
