using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Inventory", menuName = "ScriptableObjects/Inventory System/Already Seen Items")]
public class AlreadyShownItems : ScriptableObject
{
    [SerializeField] private List<Item.Item> _itemList = new List<Item.Item>();   

    public void AddItem(Item.Item item)
    {
        _itemList.Add(item);
    }

    public void Clear()
    {
        _itemList.Clear();
    }

    public bool AlreadySeenCheck(Item.Item pickedUpItem)
    {
        foreach (Item.Item item in _itemList)
        {
            if (item == pickedUpItem)
                return true;
        }
        return false;
    }
}
