using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public interface IReadOnlyInventoryGrid:IReadOnlyInventory
    {

     event Action<Vector2Int> sizeChange;
     Vector2Int Size { get; }

    IReadOnlyInventorySlot[,] GetSlots();

    }

