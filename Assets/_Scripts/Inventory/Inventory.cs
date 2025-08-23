using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private const int INVENTORY_SIZE = 5;
    private List<InventorySlot>  _inventorySlots = new List<InventorySlot>();

    private void Awake()
    {
        for (int i = 0; i < INVENTORY_SIZE; i++)
        {
            _inventorySlots[i] =  new InventorySlot(i);
        }
    }

    public IItem GetItem(int id)
    {
        return _inventorySlots[id].Item;
    }

    public void RemoveItem(int id)
    {
        _inventorySlots[id].Item = null;
    }

    public void AddItem(IItem item)
    {
        for (int i = 0; i < INVENTORY_SIZE; i++)
        {
            if (_inventorySlots[i].Item == null)
            {
                _inventorySlots[i].Item = item;
                return;
            }
        }
        throw new ArgumentOutOfRangeException("Inventory is full");
    }

    public bool HasItem(IItem item)
    {
        return _inventorySlots.Any(slot => slot.Item == item);
    }
    
}
