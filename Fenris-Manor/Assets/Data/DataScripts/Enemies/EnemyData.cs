using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenuAttribute(fileName = "EnemyData", menuName = "Scriptables/Enemies/Generic", order=-1000)]
public class EnemyData : ScriptableObject
{
    public float moveSpeed;
    public GameObject death;
    public int health;
    public float gravityModifier = 1f;
    public int damageToDo;
}
