using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;



public class PDC : MonoBehaviour
{
    public GameObject torpedo;

    public float range = 200f;

    int i = 0;



    void Update()
    {
        if (i++ % 5 != 0)
            return;


        var targets = Physics.OverlapSphere(transform.position, range).OrderBy(t => (t.transform.position - transform.position).sqrMagnitude);
        Collider targetC = targets.Where(t => t.tag == "torpedo").FirstOrDefault();

        if (targetC != null)
        {
            GameObject target = targetC.gameObject;

            var go = Instantiate(torpedo, transform);
            var id = go.GetComponent<InterceptDrive>();
            id.targetDrive = target.GetComponent<Drive>();
            id.target = target.GetComponent<Rigidbody>();
        }
    }
}
