using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenuAttribute(fileName = "WolfData", menuName = "Scriptables/Enemies/Wolf", order=-998)]
public class WolfData : EnemyData
{
    public float wakeDistance;
    public float turnAroundDistance;
    public float jumpStrength;
    public float moveSpeedMultiplier = 1.2f;
}
