using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Class for choosing random values by relative weights.
public class WeightedRandom<T> {

    protected float[] Weights;
    protected T[] Values;
    public int Length;

    protected float totalWeight;

    public WeightedRandom(float[] ws, T[] vs) {
        Values = vs;
        Length = ws.Length;
        Weights = new float[Length];
        totalWeight = 0f;
        for (int i = 0; i < Length; i++) {
            totalWeight += ws[i];
            Weights[i] = totalWeight;
        }
    }

    public T Choose() {
        float r = Random.Range(0, totalWeight);
        for (int i = 0; i < Length - 1; i++) {
            if (r < Weights[i]) {
                return Values[i];
            }
        }
        return Values[Length];
    }

}
