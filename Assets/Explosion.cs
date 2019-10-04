using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public GameObject explosion;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "torpedo")
        {
            GameObject boom = Instantiate(explosion, transform.position, Random.rotation);
            boom.transform.localScale = Random.Range(5f, 10f) * Vector3.one;

            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }
}
