using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaggleMeter : MonoBehaviour
{

    public GameObject Left;
    public GameObject Right;
    public GameObject Head;

    public GameObject LeftDistanceMeter;
    public GameObject RightDistanceMeter;
    public GameObject LeftWaggleMeter;
    public GameObject RightWaggleMeter;

    // Horizontal distances of arms from the head.
    float leftDist;
    float rightDist;

    Vector3 leftLastPos;
    Vector3 rightLastPos;

    float leftDelta;
    float rightDelta;

    private void Update()
    {

        leftDelta = (leftLastPos - Left.transform.position).magnitude;
        rightDelta = (rightLastPos - Right.transform.position).magnitude;

        leftLastPos = Left.transform.position;
        rightLastPos = Right.transform.position;

        leftDist = (Head.transform.position - Left.transform.position).magnitude
        rightDist = (Head.transform.position - Right.transform.position).magnitude;

        LeftDistanceMeter.transform.localScale = new Vector3(1, leftDist, 1);
        RightDistanceMeter.transform.localScale = new Vector3(1, rightDist, 1);
        LeftWaggleMeter.transform.localScale = new Vector3(1, leftDelta*leftDelta*1024, 1);
        RightWaggleMeter.transform.localScale = new Vector3(1, rightDelta*rightDelta*1024, 1);
    }

}
