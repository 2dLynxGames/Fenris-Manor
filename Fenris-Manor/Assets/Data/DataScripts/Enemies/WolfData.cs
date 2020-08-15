using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenuAttribute()]
public class WolfData : EnemyData
{
    public float wakeDistance;
    public float turnAroundDistance;
    public float jumpStrength;
    public float moveSpeedMultiplier = 1.2f;
}
