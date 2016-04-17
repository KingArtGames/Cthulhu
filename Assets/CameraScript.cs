using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour
{
    public float ScrollSpeed = 100.0f;
    public float MoveSpeed = 20.0f;
	// Update is called once per frame
	void Update ()
    {
        float mouseScroll = Input.GetAxis("Mouse ScrollWheel");
        if (mouseScroll != 0)
        {
            Camera.main.transform.position += ScrollSpeed * mouseScroll * Camera.main.transform.forward * Time.deltaTime;
        }
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        Camera.main.transform.position += MoveSpeed*(new Vector3(moveX, 0, moveZ))*Time.deltaTime;
	}
}
