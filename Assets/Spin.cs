using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin : MonoBehaviour
{
    void Update()
    {
        transform.rotation *= Quaternion.Euler(0, 360 * Time.deltaTime, 0);
    }
}
