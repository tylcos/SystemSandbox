using UnityEngine;



public class DestroyEffect : MonoBehaviour
{
    private float lifeTime = 10f;



    private void FixedUpdate()
    {
        if ((lifeTime -= Time.fixedDeltaTime) < 0)
            Destroy(gameObject);
    }
}
