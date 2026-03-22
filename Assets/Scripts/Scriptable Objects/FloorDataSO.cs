using System;
using UnityEngine;

[CreateAssetMenu(fileName = "FloorDataSO", menuName = "Scriptable Objects/FloorDataSO")]
public class FloorDataSO : ScriptableObject
{
    public FloorData[] floorDatas;
}
