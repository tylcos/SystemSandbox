using UnityEngine;



public class ProtectShip : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Player")
            GameManager.GameLost("You crashed into the capital ship!");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "torpedo")
            GameManager.GameLost();
    }
}
