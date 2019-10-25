using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTorpedos : MonoBehaviour
{
    public int SpawnRate = 30;
    public GameObject torpedo;
    public Transform target;

    public float a = 20f;

    private int i;

    void Update()
    {
        if (i++ % SpawnRate != 0)
            return;

        Vector3 spawnPos = new Vector3(Random.Range(-100f, 100f), Random.Range(-100f, 100f), Random.Range(200f, 300f));
        GameObject spawnedTorpedo = Instantiate(torpedo, spawnPos, Quaternion.identity, transform);
    }
}
