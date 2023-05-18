using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO TODO TODO
[CreateAssetMenu]
public class CropSpawnDictionary : ScriptableObject
{

    public Dictionary<Plant, Tuple<GameObject,float>[]> CropToBirdPrefabs;

}
