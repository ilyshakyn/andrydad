 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class InventorySlot : IReadOnlyInventorySlot
{
    public string ItemId 
    {
        get => data.itemId;
        set
        {
            if (data.itemId != value)
            {
                data.itemId = value;
                itemIdChange?.Invoke(value);
            }
        }
    }

    public int Amount
    {
        get => data.amount;
        set
        {
            if (data.amount != value)
            {
                data.amount = value;
                itemAmountChange?.Invoke(value);
            }
        }
    }

    public bool IsEmpty => Amount == 0 && string.IsNullOrEmpty(ItemId);

    public event Action<string> itemIdChange;
    public event Action<int> itemAmountChange;

    InventorySlotData data;
    public InventorySlot(InventorySlotData data)
    {
        this.data = data;
    }

  
} 

