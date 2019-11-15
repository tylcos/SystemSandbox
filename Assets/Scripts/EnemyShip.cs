using UnityEngine;



public class EnemyShip : MonoBehaviour
{
    public Drive targetShip;



    void Start()
    {
        foreach (TorpedoLauncher torpedoLauncher in transform.GetComponentsInChildren<TorpedoLauncher>())
            torpedoLauncher.target = targetShip;
    }
}
