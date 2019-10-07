using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorpedoLauncher : MonoBehaviour
{
    public Drive parent;

    public GameObject torpedo;
    public Drive target;

    public float launchSpeed = 20f;
    public float waitTime = 3f;
    public float torpedoAccel = 20f;
    public Vector3 initalV = Vector3.forward;
    public int SpawnRate = 50;

    private int i;



    void Update()
    {
        if (i++ % SpawnRate != 0)
            return;



        GameObject spawnedTorpedo = Instantiate(torpedo, transform.position, transform.rotation, GameManager.WorldTransform);
        InterceptDriveAccel drive = spawnedTorpedo.GetComponent<InterceptDriveAccel>();

        drive.targetDrive = target;
        drive.accel = torpedoAccel;
        drive.rb.velocity = parent.rb.velocity + initalV.normalized * launchSpeed;
        drive.waitTime = waitTime;
    }
}
