using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crow : MonoBehaviour
{
    public float FlySpeed = 0.01f;
    public float Braveness = 1f;
    public float FearDecay = 0.002f;
    public float CorrectionWeight = 0.5f;
    public Animator animator;

    public GameObject Target;

    public float Fear = 0f;
    public bool Fleeing = false;

    public float DespawnRange = 200f;
    public float FleeSpeed = 20f;

    public Vector3 Velocity;

    private void Start()
    {
        Target = GameController.Player;
    }

    private void Update()
    {
        Velocity = (Target.transform.position - transform.position).normalized * FlySpeed * (Fleeing ? -FleeSpeed : 1);
        transform.position += Velocity;

        if (transform.position.magnitude > DespawnRange)
        {
            Destroy(gameObject);
        }

        if (Velocity.magnitude > 0.01f)
        {
            transform.rotation = Quaternion.LookRotation(Velocity);
        }

        Spook(GameController.WaggleScore);

        Fear = Mathf.Max(Fear - FearDecay, 0);
    }

    public void Spook(float f)
    {
        Fear += f;
        if (Fear >= Braveness)
        {
            Fleeing = true;
        }
    }

}
