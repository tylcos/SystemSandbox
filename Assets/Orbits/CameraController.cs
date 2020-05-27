using UnityEngine;



public class CameraController : MonoBehaviour
{
    public float speed = 1f;
    public float lookSensitivity = 1f;



    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
    }



    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            Vector3 look = new Vector3(
            Input.GetAxis("Mouse X"),
            Input.GetAxis("Mouse Y"));

            transform.eulerAngles += look * lookSensitivity;
        }
        


        Vector3 move = new Vector3(
            Input.GetAxis("Horizontal"),
            0,
            Input.GetAxis("Depth"));

        move.y += Input.GetKey(KeyCode.LeftShift)   ? 1 : 0;
        move.y += Input.GetKey(KeyCode.LeftControl) ? -1 : 0;

        transform.position += Quaternion.Euler(transform.eulerAngles) * move * speed;
    }
}
