using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


    public struct RemoveItemsFromInventoryGridResult
    {
    public readonly string inventoryOwnerId;
    public readonly int itemsToAddAmount;
    public readonly bool Success;


   

    public RemoveItemsFromInventoryGridResult(string inventoryOwnerId, int itemsToAddAmount, bool Success)
    {
        this.inventoryOwnerId = inventoryOwnerId;
        this.itemsToAddAmount = itemsToAddAmount;
        this.Success = Success;
    }
}

