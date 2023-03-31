using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KBMPlayer : MonoBehaviour
{

    public GameObject Cam;

    public float Speed = 0.5f;
    public float Sensitivity = 3f;
    public float SpookAmount = 0.01f;

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

        Cam.transform.rotation = Quaternion.Euler(-dmy, dmx, 0);

        transform.position += Cam.transform.rotation * v;

        if (Input.GetKey("space"))
        {
            foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Bird"))
            {
                obj.GetComponent<Crow>().Spook(SpookAmount);
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
            GameObject crop = Instantiate(GameController.Instance.BasicCropPrefab);
            crop.transform.position = ray.GetPoint(-ray.origin.y / ray.direction.y); // Get point at which ray intersects y=0
        }
            

    }

}
