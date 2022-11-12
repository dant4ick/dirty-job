using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Inventory", menuName = "ScriptableObjects/Inventory System/Inventory")]
public class Inventory : ScriptableObject
{
    [SerializeField] private List<Item> _itemList = new List<Item>();

    public List<Item> ItemList { get { return _itemList; } }

    public void AddItem(Item item)
    {
        if (_itemList.Count < 5)
        {
            _itemList.Add(item);
        }
    }
}