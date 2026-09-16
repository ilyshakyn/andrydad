using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

    public interface IReadOnlyInventory
    {

     event Action<string, int> itemsAdded;
     event Action<string, int> itemsRemoved;

      string OwnerId { get; }

     int GetAmount(string itemId);
     bool Has(string itemId, int amount);
    }

