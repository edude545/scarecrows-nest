using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KBMPlayer : MonoBehaviour
{

    public float Speed = 0.5f;
    public float Sensitivity = 3f;

    float dmx = 0f;
    float dmy = 0f;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        Vector3 v = new Vector3(
            Input.GetKey("d") ? 1 : Input.GetKey("a") ? -1 : 0,
            Input.GetKey("e") ? 1 : Input.GetKey("q") ? -1 : 0,
            Input.GetKey("w") ? 1 : Input.GetKey("s") ? -1 : 0
        );

        v *= Speed;

        if (Input.GetKey("left shift")) {
            v *= 3f;
        } else if (Input.GetKey("left ctrl")) {
            v *= 0.2f;
        }

        dmx += Input.GetAxis("Mouse X") * Sensitivity;
        dmy += Input.GetAxis("Mouse Y") * Sensitivity;

        transform.rotation = Quaternion.Euler(-dmy, dmx, 0);

        transform.position += transform.rotation * v;

    }

}
