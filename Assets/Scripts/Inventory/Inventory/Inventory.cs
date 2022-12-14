using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Inventory", menuName = "ScriptableObjects/Inventory System/Inventory")]
public class Inventory : ScriptableObject
{
    [SerializeField] private List<InventoryCell> _itemList = new List<InventoryCell>();
    public event EventHandler OnItemListChanged;

    public bool IsFullCheck()
    {
        foreach (InventoryCell cell in _itemList)
        {
            if (cell.GetItem() == null)
                return false;
        }

        return true;
    }

    public int GetLength()
    {
        return _itemList.Count;
    }

    public Item.Item GetItemFromCell(int position)
    {
        return _itemList[position].GetItem();
    }

    public void SetCells()
    {
        if (_itemList.Count >= 6)
            return;

        int cellInListCount = _itemList.Count;

        for (int cellCount = 0; cellCount < 6 - cellInListCount; cellCount++)
            _itemList.Add(new InventoryCell());        
    }

    public bool AddItem(Item.Item item)
    {
        foreach (InventoryCell inventoryCell in _itemList)
        {
            if (inventoryCell.GetItem() is null)
            {
                inventoryCell.SetItem(item);
                OnItemListChanged?.Invoke(this, EventArgs.Empty);
                return true;
            }
        }
        return false;
    }

    public void SetItemToCell(Item.Item item, int position)
    {
        _itemList[position].SetItem(item);
    }

    public void Clear()
    {
        _itemList.Clear();
    }
}

[System.Serializable]
public class InventoryCell
{
    [SerializeField] private Item.Item _item;

    public Item.Item GetItem()
    {
        return _item;
    }

    public void SetItem(Item.Item item)
    {
        _item = item;
    }
}