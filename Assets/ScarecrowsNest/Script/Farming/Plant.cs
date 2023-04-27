using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Plant : ScriptableObject
{

    public string Name;
    public int RequiredSeeds;
    public int GrowthTime;
    public int MinYield;
    public int MaxYield;

}
