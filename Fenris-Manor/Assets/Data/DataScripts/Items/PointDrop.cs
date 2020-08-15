using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenuAttribute(fileName = "PointGem", menuName = "Scriptables/Items/Gems", order=-899)]
public class PointDrop : DroppableItem
{
    public int points;
}
