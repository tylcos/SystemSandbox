using System;
using System.Collections;
using System.Linq;
using System.Text;
using UnityEngine;

class PlaneControl : MonoBehaviour
{
    public float speed = 10f;
    public float accel = 10f;
    public Rigidbody rb;
    public Camera camera;

    public float[] times;



    private void Start()
    {
        rb.velocity = transform.forward * speed;

        StartCoroutine(ChangeSkyBox());
    }

    private void FixedUpdate()
    {
        transform.rotation *= Quaternion.Euler(-2 * Time.fixedDeltaTime, 0, 0);
        rb.velocity = transform.forward * rb.velocity.magnitude;
        rb.AddForce(transform.forward * accel * Time.fixedDeltaTime, ForceMode.Acceleration);
    }


    private IEnumerator ChangeSkyBox()
    {
        yield return new WaitForSeconds(times[0]);

        print("Color");
        camera.backgroundColor = UnityEngine.Random.ColorHSV();
    }
}