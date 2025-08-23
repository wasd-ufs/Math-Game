using UnityEngine;

public class InventorySlot
{
  public InventorySlot(int id)
  {
    this._id = id;
  }
  
  private int _id;
  public IItem Item;
}
