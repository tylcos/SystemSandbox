using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public GameObject explosion;

    public void SpawnExplosion()
    {
        GameObject boom = Instantiate(explosion, transform.position, Random.rotation, transform.parent);
        boom.transform.localScale = Random.Range(2f, 5f) * Vector3.one;
    }
}
