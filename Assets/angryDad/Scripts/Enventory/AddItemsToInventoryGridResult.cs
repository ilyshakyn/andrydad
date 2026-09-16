using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


    public readonly struct AddItemsToInventoryGridResult
{
      public readonly string inventoryOwnerId;
      public readonly int itemsToAddAmount;
      public readonly int itemsAddedAmount;


     public int ItemsNotAddedAmount => itemsToAddAmount - itemsAddedAmount;

     public AddItemsToInventoryGridResult(string inventoryOwnerId, int itemsToAddAmount, int itemsAddedAmount)
     {
         this.inventoryOwnerId = inventoryOwnerId;
         this.itemsToAddAmount = itemsToAddAmount;
         this.itemsAddedAmount = itemsAddedAmount;
     }
    }

