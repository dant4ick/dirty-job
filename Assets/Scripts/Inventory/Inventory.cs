using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Inventory", menuName = "ScriptableObjects/Inventory System/Inventory")]
public class Inventory : ScriptableObject
{
    [SerializeField] private List<Item.Item> _itemList = new List<Item.Item>();

    public List<Item.Item> ItemList { get { return _itemList; } }

    public void AddItem(Item.Item item)
    {
        if (_itemList.Count < 5)
        {
            _itemList.Add(item);
        }
    }
}