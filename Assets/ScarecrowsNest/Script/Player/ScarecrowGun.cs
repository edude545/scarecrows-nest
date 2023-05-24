using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScarecrowGun : BeltObject
{

    public float Charge = 600f;
    public float MaxCharge = 600f;

    public float PowerFalloff = 0.5f;
    public bool SprayAttenuation = false; // false for noise gun, true for pepper spray
    public bool ScalePowerWithDepletion = false;
    [Range(0,360)] public float AngularRange = 45f;
    [Range(0, 1)] public float AccuracyImportance = 0.5f;
    public float Power = 10f;

    public ParticleSystem Particles;

    public override void Use(float triggerValue) {
        
        if (Particles != null)
        {
            if (triggerValue <= 0f)
            {
                Particles.Stop();
            } else
            {
                Particles.Play();
                Particles.gameObject.SetActive(true);
                var main = Particles.main;
                main.startSpeed = Mathf.Lerp(0f, Mathf.Lerp(0f,2000f,Charge/MaxCharge), triggerValue);
            }
        }
        Charge -= triggerValue;
        for (int i = 0; i < GameController.Instance.Birds.childCount; i++) {
            Bird bird = GameController.Instance.Birds.transform.GetChild(i).GetComponent<Bird>();
            Vector3 between = bird.transform.position - transform.position;
            float angle = Mathf.Clamp(AngularRange - Vector3.Angle(transform.rotation * Vector3.forward, between), 0.1f, AngularRange);
            float divisor;
            if (SprayAttenuation) {
                float solidAngle = 2 * Mathf.PI * (1 - Mathf.Cos(angle / 2));
                divisor = solidAngle * between.magnitude * between.magnitude;
            } else {
                divisor = between.magnitude;
            }
            bird.Waggle += Power*triggerValue*(ScalePowerWithDepletion ? Charge/MaxCharge : 1)
                * (AccuracyImportance * Mathf.InverseLerp(0, AngularRange, angle) + 1 - AccuracyImportance)
                / divisor;
        }
    }

    public void Refill()
    {
        Charge = MaxCharge;
    }

}
