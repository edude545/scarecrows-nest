using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaggleTrigger : MonoBehaviour
{

    [Range(0, 360)] public float VisualContactAngle = 30f;
    public float MaxWaggle = 1f;
    public float WaggleDecay = 0.002f;
    public float Waggle = 0f;

    protected void updateWaggleScore()
    {
        var angle = Mathf.Clamp(VisualContactAngle - Vector3.Angle(Camera.main.transform.rotation * Vector3.forward, transform.position - Camera.main.transform.position), 0, VisualContactAngle);
        Waggle += GameController.GlobalWaggleMultiplier * (0.3f + Mathf.InverseLerp(0, VisualContactAngle, angle));
        Waggle = Mathf.Max(Waggle - WaggleDecay, 0);
    }

}
