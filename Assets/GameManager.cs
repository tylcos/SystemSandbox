using UnityEngine;



public class GameManager : MonoBehaviour
{
    public static Transform WorldTransform;



    private void Start()
    {
        WorldTransform = transform;
    }



    private void FixedUpdate()
    {
        print(Time.time);
    }
}
