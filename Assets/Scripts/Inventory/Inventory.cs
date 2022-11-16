using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Inventory", menuName = "ScriptableObjects/Inventory System/Inventory")]
public class Inventory : ScriptableObject
{
    [SerializeField] private List<InventoryCell> _itemList = new List<InventoryCell>();
    public event EventHandler OnItemListChanged;

    public List<InventoryCell> ItemList { get { return _itemList; } }

    public void AddCell()
    {
        if (_itemList.Count < 6)
        {
            _itemList.Add(new InventoryCell());
        }
    }

    public void AddItem(Item.Item item)
    {
        foreach (InventoryCell inventoryCell in _itemList)
        {
            if (inventoryCell.GetItem() is null)
            {
                inventoryCell.SetItem(item);
                OnItemListChanged?.Invoke(this, EventArgs.Empty);
                break;
            }
        }
    }

    public void SetItem(Item.Item item, int position)
    {
        _itemList[position].SetItem(item);
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