using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenuAttribute(fileName = "DroppableItem", menuName = "Scriptables/Items/Generic", order=-900)]
public class DroppableItem : ScriptableObject
{
    public string dropName;
    public int weight;
    public Sprite sprite;
}
