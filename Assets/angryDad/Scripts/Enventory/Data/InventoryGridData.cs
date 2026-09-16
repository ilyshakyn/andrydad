using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class InventoryGridData 
{
     public string ownerId;
     public List<InventorySlotData> slots;
     public Vector2Int size; 
}
