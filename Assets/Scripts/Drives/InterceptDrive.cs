using UnityEngine;



public class InterceptDrive : Drive
{
    public float speed = 100f;

    public Drive targetDrive;

    public PDC parent;



    private float lifeTime = 30f;



    new void Start()
    {
        base.Start();

        if (targetDrive == null)
        {
            rb.velocity = transform.forward.normalized * speed;
            return;
        }



        float t = EquationSolver.FindRealSolutionSmallestT(this, targetDrive);
        if (!float.IsInfinity(t))
        {
            Vector3 interceptPos = targetDrive.EstimatedPos(t);

            rb.velocity = (interceptPos - rb.position).normalized * speed;
            transform.LookAt(interceptPos);
        }
    }



    private void FixedUpdate()
    {
        if ((lifeTime -= Time.fixedDeltaTime) < 0)
            Destroy(gameObject);
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "torpedo")
        {
            parent.TargetHit(other.gameObject);

            gameObject.GetComponent<Explosion>().SpawnExplosion();
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
}
