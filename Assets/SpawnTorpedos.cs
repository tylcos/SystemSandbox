using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTorpedos : MonoBehaviour
{
    public int SpawnRate = 30;
    public GameObject torpedo;
    public Transform target;

    public float a = 20f;
    public int spawnRate = 5;

    private int i;

    int i = 0;

    void Update()
    {
        if (i++ % spawnRate != 0)
            return;

        Vector3 spawnPos = new Vector3(Random.Range(-100f, 100f), Random.Range(-100f, 100f), Random.Range(200f, 300f));
        GameObject spawnedTorpedo = Instantiate(torpedo, spawnPos, Quaternion.identity, transform);
        Drive drive = spawnedTorpedo.GetComponent<Drive>();

        drive.accel = (target.position - spawnedTorpedo.transform.position).normalized * a;
    }
}
