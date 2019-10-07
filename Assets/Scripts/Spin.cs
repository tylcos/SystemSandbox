using UnityEngine;



public class Spin : MonoBehaviour
{
    void Update()
    {
        transform.rotation *= Quaternion.Euler(0, 720 * Time.deltaTime, 0);
    }
}
