using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crop : MonoBehaviour
{

    public int GrowTime;
    public int GrowStage;

    public void OnCycleEnd() {
        updateModel();
    }

    void updateModel() {
        // todo: allow separate models for growth stages
        float s = GrowStage / GrowTime;
        gameObject.transform.localScale = new Vector3(s,s,s);
    }

}
