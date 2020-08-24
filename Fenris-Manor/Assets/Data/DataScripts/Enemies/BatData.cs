using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenuAttribute(fileName = "BatData", menuName = "Scriptables/Enemies/Bat", order=-996)]
public class BatData : EnemyData
{
    public float wakeDistance;
    public float wakeHeight;
    public float batVerticalSpeed = 1f;
    public float turnDistance = 5f;
}
