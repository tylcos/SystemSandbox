using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PDC : MonoBehaviour
{
    public GameObject torpedo;

    public Drive targetDrive;
    public Rigidbody target;

    private int count = 0;


    void Update()
    {
        if (count++ > 1000)
            return;



        var go = Instantiate(torpedo, transform);
        var id = go.GetComponent<InterceptDrive>();
        id.targetDrive = targetDrive;
        id.target = target;




        go = Instantiate(torpedo, transform);
        id = go.GetComponent<InterceptDrive>();
        id.targetDrive = targetDrive;
        id.target = target;
        id.chooseMin = false;
    }
}
