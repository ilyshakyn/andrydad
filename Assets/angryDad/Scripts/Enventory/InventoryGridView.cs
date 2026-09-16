using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


    public class InventoryGridView : MonoBehaviour
    {

        public void Setup(IReadOnlyInventoryGrid inventory)
        {
          var slots = inventory.GetSlots();
          var size = inventory.Size;

          for (var i = 0; i < size.x; i++)
          {
            for (var j = 0; i < size.y; j++)
            {
                var slot = slots[i,j];
                Debug.Log($"Slot {i}:{j} Item {slot.ItemId}, amount: {slot.Amount}");
            }
          }
        }
    }

