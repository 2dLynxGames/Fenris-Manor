using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenuAttribute(fileName = "CrocData", menuName = "Scriptables/Enemies/Wolf", order=-997)]
public class CrocData : EnemyData
{
    public float jumpStrength;
    public float wakeDistance;
    public float projectileSpeed;
    public GameObject fireball;
}
