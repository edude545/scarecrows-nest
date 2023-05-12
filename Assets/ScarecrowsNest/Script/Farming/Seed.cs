using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

[RequireComponent(typeof(Throwable))]
public class Seed : MonoBehaviour
{

    public Plant PlantType;

    private Transform seedBag;
    private Vector3 originalPosition;
    private Rigidbody rb;

    private void Awake() {
        seedBag = transform.parent;
        originalPosition = transform.localPosition;
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
    }

    private void Update() {
        if (transform.position.y < -10) {
            ResetSeed();
        }
    }

    public void ResetSeed() {
        transform.parent = seedBag.transform;
        transform.localPosition = originalPosition;
        rb.useGravity = false;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    public void OnDetach() {
        rb.useGravity = true;
    }

}
