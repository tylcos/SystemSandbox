using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTorpedos : MonoBehaviour
{
    public GameObject torpedo;
    public Transform target;

    public float a = 20f;

    void Update()
    {
        GameObject spawnedTorpedo = Instantiate(torpedo, transform);
        Drive drive = spawnedTorpedo.GetComponent<Drive>();

        spawnedTorpedo.transform.position = new Vector3(Random.Range(-200f, 200f), Random.Range(-200f, 200f), Random.Range(500f, 600f));
        drive.accel = (target.position - spawnedTorpedo.transform.position).normalized * a;
    }
}
