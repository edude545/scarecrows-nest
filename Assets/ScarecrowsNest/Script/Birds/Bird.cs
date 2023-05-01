using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Bird : MonoBehaviour {

    public float FlySpeed = 0.01f;
    public float Braveness = 1f;
    public float FearDecay = 0.002f;
    public float CorrectionWeight = 0.5f;
    public Animator animator;

    public GameObject Target; // most birds target crops, aggressive birds target the player
    public bool IsAggressive = false;
    public Plant FavoritePlant;

    public float Fear = 0f;
    public bool Fleeing = false;

    public float DespawnRange = 200f;
    public float FleeSpeed = 20f;

    public Vector3 Velocity;

    private void Start() {
        findTarget();
        beginFlight();
    }

    private void Update() {
        Velocity = (Target.transform.position - transform.position).normalized * FlySpeed * (Fleeing ? -FleeSpeed : 1);
        transform.position += Velocity;

        if (transform.position.magnitude > DespawnRange) {
            Destroy(gameObject);
        }

        if (Velocity.magnitude > 0.01f) {
            transform.rotation = Quaternion.LookRotation(Velocity);
        }

        Spook(GameController.WaggleScore);

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

    void beginFlight() {
        animator.SetBool("airborne", true);
        animator.SetBool("moving", true);
        animator.SetBool("attacking", false);
        animator.SetBool("eating", false);
    }

    public void Spook(float f) {
        Fear += f;
        if (Fear >= Braveness) {
            beginFlight();
            Fleeing = true;
        }
    }

}
