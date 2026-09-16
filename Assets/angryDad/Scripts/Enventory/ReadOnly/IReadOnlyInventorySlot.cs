using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


    public  interface IReadOnlyInventorySlot
    {
      event Action<string> itemIdChange;
      event Action<int> itemAmountChange;
      string ItemId { get; }
      int Amount { get; }
      public bool IsEmpty { get;  }

    }

