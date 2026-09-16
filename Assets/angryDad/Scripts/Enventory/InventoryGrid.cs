using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;


public class InventoryGrid : IReadOnlyInventoryGrid
{

    public event Action<Vector2Int> sizeChange;
    public event Action<string, int> itemsAdded;
    public event Action<string, int> itemsRemoved;

    public Vector2Int Size
    {
        get => data.size;
        set
        {
            if (data.size != value)
            {
                data.size = value;
                sizeChange?.Invoke(value);
            }
        }
    }

    public string OwnerId => data.ownerId;



    private InventoryGridData data;
    private readonly Dictionary<Vector2Int, InventorySlot> slotsMap =  new();
    public InventoryGrid(InventoryGridData data)
    {
        this.data = data;
        var size = data.size;
        for (var i = 0; i < size.x; i++)
        {
            for (var j = 0; i < size.y; j++)
            {
                var index = i * size.y + j;
                var slotData = data.slots[index];
                var slot = new InventorySlot(slotData);
                var postion = new Vector2Int(i, j);
                slotsMap[postion] = slot;
            }
        }

    }
    public int GetAmount(string itemId)
    {
        throw new NotImplementedException();
    }

    public IReadOnlyInventorySlot[,] GetSlots()
    {
        var array = new IReadOnlyInventorySlot[Size.x, Size.y];
        for (var i = 0; i < Size.x; i++)
        {
            for (var j = 0; i < Size.y; j++)
            {
               
                var postion = new Vector2Int(i, j);
                array[i,j] = slotsMap[postion]; 
            }
        }

        return array;

    }

    public bool Has(string itemId, int amount)
    {
        throw new NotImplementedException();
    }



    public AddItemsToInventoryGridResult AddItems(string itemId, int amount)
    {
        var remaimingAmount = amount;
        var itemsAddedToSlotsWithSameItemsAmount = AddToSlotsWithSameItems(itemId,remaimingAmount,out remaimingAmount);

        if(remaimingAmount <=0)
        {

            return new AddItemsToInventoryGridResult(OwnerId, amount, itemsAddedToSlotsWithSameItemsAmount);
        }

        return new AddItemsToInventoryGridResult("s", amount, 14);//////////////////////////////////
    }

    private int AddToSlotsWithSameItems(string itemId, int remaimingAmount1, out int remaimingAmount2)////////////////////
    {///////////
        throw new NotImplementedException();//////////
    }//////////////

    public AddItemsToInventoryGridResult AddItems(Vector2Int slotCoords, string itemId, int amount = 1)
    {
        var slot = slotsMap[slotCoords];
        var newValue = slot.Amount + amount;
        var itemsAddedAmount = 0;


        if (slot.IsEmpty)
        {
            slot.ItemId = itemId;
        }

        var itemSlotCapacity = GetItemSlotCapacity(itemId);

        if (newValue > itemSlotCapacity)
        {
            var remainingItems  = newValue - itemSlotCapacity;
            var itemsToAddAmount = itemSlotCapacity - slot.Amount;

            itemsAddedAmount += itemsToAddAmount;
            slot.Amount = itemSlotCapacity;

            var result = AddItems(itemId, remainingItems);
            itemsAddedAmount += result.ItemsNotAddedAmount;
        }
        else
        {
            itemsAddedAmount = amount;
            slot.Amount = newValue;
        }
        return new AddItemsToInventoryGridResult("s", amount, 14);//////////////////////////////////
    }
   


    public RemoveItemsFromInventoryGridResult RemoveItems(string itemId,int amount = 1)
    {
        return new RemoveItemsFromInventoryGridResult(OwnerId, amount, true);/////////////////////////////////////////////
    }

    public RemoveItemsFromInventoryGridResult RenoveItems(Vector2Int slotCoords, string itemId, int amount = 1)
    {
        var slot = slotsMap[slotCoords];

        if (slot.IsEmpty || slot.ItemId != itemId || slot.Amount< amount)
        {
            return new RemoveItemsFromInventoryGridResult(OwnerId,amount, false);
        }

        slot.Amount -= amount;

        if (slot.Amount ==0)
        {
            slot.ItemId = null;
        }

        return new RemoveItemsFromInventoryGridResult(OwnerId, amount, true);

    }

    private int GetItemSlotCapacity(string itemId)
    {
        return 99;
    }
}

