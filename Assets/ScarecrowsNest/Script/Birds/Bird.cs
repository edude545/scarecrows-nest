using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Bird : MonoBehaviour {

    public float FlySpeed = 0.01f;
    public float Braveness = 1f;
    public float FearDecay = 0.002f;
    public float VisualContactAngle = 30f;
    public float CorrectionWeight = 0.5f;
    public Animator animator;

    public GameObject Target; // most birds target crops, aggressive birds target the player
    public bool IsAggressive = false;
    public Plant FavoritePlant;

    public float Fear = 0f;
    public bool Fleeing = false;

    public float InteractRange = 2f;
    public float DespawnRange = 200f;
    public float FleeSpeed = 20f;

    public Vector3 Velocity;

    public States State;
    public enum States {
        Flying,
        Eating,
        Attacking,
        Fleeing
    }

    private void Start() {
        ChangeState(States.Flying);
    }

    public void ChangeState(States state) {
        State = state;
        if (state == States.Flying) {
            animator.SetBool("airborne", true);
            animator.SetBool("moving", true);
            animator.SetBool("attacking", false);
            animator.SetBool("eating", false);
            findTarget();
        } else if (state == States.Eating) {
            animator.SetBool("airborne", false);
            animator.SetBool("moving", false);
            animator.SetBool("attacking", false);
            animator.SetBool("eating", true);
        } else if (state == States.Attacking) {
            animator.SetBool("airborne", true);
            animator.SetBool("moving", false);
            animator.SetBool("attacking", true);
            animator.SetBool("eating", false);
        } else if (state == States.Fleeing) {
            animator.SetBool("airborne", true);
            animator.SetBool("moving", true);
            animator.SetBool("attacking", false);
            animator.SetBool("eating", false);
        }
    }

    private void Update() {
        if (State == States.Flying) {
            Vector3 velocity = (Target.transform.position - transform.position).normalized * FlySpeed;
            transform.position += velocity;
            if ((transform.position - Target.transform.position).magnitude <= InteractRange) {
                if (Target.Equals(GameController.Instance.Player)) {
                    ChangeState(States.Attacking);
                } else {
                    ChangeState(States.Eating);
                }
            }
            transform.rotation = Quaternion.LookRotation(velocity);
        } else if (State == States.Eating) {

        } else if (State == States.Attacking) {

        } else if (State == States.Fleeing) {
            transform.position += FleeSpeed * transform.position.normalized;
        }

        if (transform.position.magnitude > DespawnRange) {
            Destroy(gameObject);
        }

        var angle = Mathf.Clamp(VisualContactAngle-Vector3.Angle(Camera.main.transform.rotation * Vector3.forward, transform.position - Camera.main.transform.position), 0, VisualContactAngle);
        Spook(GameController.WaggleScore * (1+Mathf.InverseLerp(0, VisualContactAngle, angle)));
        Fear = Mathf.Max(Fear - FearDecay, 0);
    }

    void findTarget() {
        if (IsAggressive) {
            Target = GameController.Instance.Player;
        } else { // Birds are 3 times more likely to choose their favorite crop, and less likely to choose damaged crops.
            float[] weights = new float[GameController.Instance.LiveCrops.transform.childCount];
            Crop[] values = new Crop[GameController.Instance.LiveCrops.transform.childCount];
            for (int i = 0; i < weights.Length; i++) {
                Crop crop = GameController.Instance.LiveCrops.transform.GetChild(i).GetComponent<Crop>();
                float w = crop.PlantType == FavoritePlant ? 3 : 1;
                w /= crop.HP / crop.PlantType.MaxHP;
                values[i] = crop;
            }
            WeightedRandom<Crop> crops = new WeightedRandom<Crop>(weights, values);
            Target = crops.Choose().gameObject;
        }
    }

    public void Spook(float f) {
        Fear += f;
        if (Fear >= Braveness) {
            ChangeState(States.Fleeing);
        }
    }

}
