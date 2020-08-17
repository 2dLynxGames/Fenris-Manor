using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DropTable", menuName = "Scriptables/Items/DropTable", order = 901)]
public class DropTableData : ScriptableObject {
    public List<DroppableItemData> dropTable;
}