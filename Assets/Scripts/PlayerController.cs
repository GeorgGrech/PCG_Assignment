using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    Rigidbody rigidbody;
    private float speed = 700;
    private float sensitivity = 1f;
    Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 xMov = new Vector2(Input.GetAxisRaw("Horizontal") * transform.right.x, Input.GetAxisRaw("Horizontal") * transform.right.z);
        Vector2 zMov = new Vector2(Input.GetAxisRaw("Vertical") * transform.forward.x, Input.GetAxisRaw("Vertical") * transform.forward.z);

        Vector2 velocity = (xMov + zMov).normalized * speed * Time.deltaTime;

        float yRot = Input.GetAxisRaw("Mouse X") * sensitivity;
        rigidbody.rotation *= Quaternion.Euler(0, yRot, 0);

        float xRot = Input.GetAxisRaw("Mouse Y") * sensitivity;
        float x_rot = cam.transform.rotation.eulerAngles.x;
        x_rot -= xRot;

        float camEulerAngleX = cam.transform.localEulerAngles.x;

        camEulerAngleX -= xRot * sensitivity;

        cam.transform.localEulerAngles = new Vector3(camEulerAngleX, 0, 0);
        rigidbody.velocity = new Vector3(velocity.x, rigidbody.velocity.y, velocity.y);
    }
}
