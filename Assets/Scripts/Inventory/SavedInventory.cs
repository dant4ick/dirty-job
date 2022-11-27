using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Inventory", menuName = "ScriptableObjects/Inventory System/SavedInventory")]
public class SavedInventory : MonoBehaviour
{
    [SerializeField] private List<InventoryCell> _itemList = new List<InventoryCell>();

    private void OnApplicationQuit()
    {
        _itemList.Clear();        
    }
}
