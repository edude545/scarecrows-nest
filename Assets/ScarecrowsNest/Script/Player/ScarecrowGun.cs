using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScarecrowGun : BeltObject
{

    public float Charge = 0f;
    public float MaxCharge = 100f;

    public float PowerFalloff = 0.5f;
    public bool SprayAttenuation = false; // false for noise gun, true for pepper spray
    [Range(0,360)] public float AngularRange = 45f;
    [Range(0, 1)] public float AccuracyImportance = 0.5f;
    public float Power = 10f;

    public override void Use(float triggerValue) {
        Charge += triggerValue;
        for (int i = 0; i < GameController.Instance.Birds.childCount; i++) {
            Bird bird = GameController.Instance.Birds.transform.GetChild(i).GetComponent<Bird>();
            Vector3 between = bird.transform.position - transform.position;
            float angle = Mathf.Clamp(AngularRange - Vector3.Angle(transform.rotation * Vector3.forward, between), 0, AngularRange);
            float divisor;
            if (SprayAttenuation) {
                float solidAngle = 2 * Mathf.PI * (1 - Mathf.Cos(angle / 2));
                divisor = solidAngle * between.magnitude * between.magnitude;
            } else {
                divisor = between.magnitude;
            }
            bird.Spook(Power*triggerValue
                * (AccuracyImportance * Mathf.InverseLerp(0, AngularRange, angle) + 1 - AccuracyImportance)
                / divisor);
        }
    }

    public void ReplenishCharge() {
        Charge = MaxCharge;
    }

}
