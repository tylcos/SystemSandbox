using UnityEngine;



public class Explosion : MonoBehaviour
{
    public GameObject explosion;

    public void SpawnExplosion(Vector3 inheritedVelocity)
    {
        GameObject boom = Instantiate(explosion, transform.position, Random.rotation, transform.parent);
        boom.transform.localScale = Random.Range(2f, 5f) * Vector3.one;

        Rigidbody rb = boom.AddComponent<Rigidbody>();
        rb.velocity = inheritedVelocity;
    }
}
