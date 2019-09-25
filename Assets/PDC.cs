using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PDC : MonoBehaviour
{
    public GameObject torpedo;

    public float range = 200f;


    void Update()
    {
        Collider[] targets = Physics.OverlapSphere(transform.position, range);

        if (targets.Length > 0)
        {
            GameObject target = targets[0].gameObject;

            var go = Instantiate(torpedo, transform);
            var id = go.GetComponent<InterceptDrive>();
            id.targetDrive = target.GetComponent<Drive>();
            id.target = target.GetComponent<Rigidbody>();
        }
        
    }
}
