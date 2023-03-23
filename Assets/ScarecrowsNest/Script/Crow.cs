using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crow : MonoBehaviour
{

    public float FlySpeed = 1f;
    public Animator animator;

    GameObject target;

    Vector3 velocity;

    private void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        Vector3 acceleration = (target.transform.position - transform.position).normalized * FlySpeed;
        
    }

}
