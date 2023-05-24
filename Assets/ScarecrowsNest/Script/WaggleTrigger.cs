using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaggleTrigger : MonoBehaviour
{

    [Range(0, 360)] public float VisualContactAngle = 30f;
    public float MaxWaggle = 1f;
    public float WaggleDecay = 0.002f;
    public float Waggle = 0f;

    public bool AffectedByBodySize;

    protected void updateWaggleScore()
    {
        float angle;
        if ((transform.position - GameController.Instance.Player.transform.position).magnitude < 5f)
        {
            angle = 0f;
        } else
        {
            angle = Mathf.Clamp(VisualContactAngle - Vector3.Angle(Camera.main.transform.rotation * Vector3.forward, transform.position - Camera.main.transform.position), 0, VisualContactAngle);
        }
        Waggle += GameController.GlobalWaggleMultiplier * (AffectedByBodySize ? GameController.Instance.BodySize : 1f) * (0.3f + Mathf.InverseLerp(0, VisualContactAngle, angle));
        Waggle = Mathf.Max(Waggle - WaggleDecay, 0);
    }

}
