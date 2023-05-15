using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Class for choosing random values by relative weights.
public class WeightedRandom<T> {

    public float[] Thresholds;
    public T[] Values;
    public int Length;

    protected float totalWeight;
    public WeightedRandom(float[] weights, T[] values) {
        Values = values;
        Length = weights.Length;
        Thresholds = new float[Length];
        totalWeight = 0f;
        for (int i = 0; i < Length; i++) {
            totalWeight += weights[i];
            Thresholds[i] = totalWeight;
        }
    }

    public T Choose() {
        float r = Random.Range(0, totalWeight);
        for (int i = 0; i < Length - 1; i++) {
            if (r < Thresholds[i]) {
                return Values[i];
            }
        }
        return Values[Length - 1];
    }

}
