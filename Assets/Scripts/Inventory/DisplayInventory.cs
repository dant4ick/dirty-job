using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayInventory : MonoBehaviour
{
    [SerializeField] private Inventory savedInventory;

    public Inventory inventory;
    public Transform inventorySlotTemplate;
    public Transform itemImageInInventory;

    // Start is called before the first frame update
    void Start()
    {
        inventory.SetCells();

        if (savedInventory.GetLength() != 0)        
            GetSavedInventory();        
        else
            savedInventory.SetCells();

        UpdateDisplay();
        inventory.OnItemListChanged += Inventory_OnItemListChanged;
    }

    private void OnDestroy()
    {
        inventory.OnItemListChanged -= Inventory_OnItemListChanged;
        //SaveInventory();
        inventory.Clear();
    }

    private void SaveInventory()
    {
        for (int cell = 0; cell < inventory.GetLength(); cell++)
        {
            savedInventory.SetItemToCell(inventory.GetItemFromCell(cell), cell);
        }
    }
    private void GetSavedInventory()
    {
        for (int cell = 0; cell < savedInventory.GetLength(); cell++)
        {
            inventory.SetItemToCell(savedInventory.GetItemFromCell(cell), cell);
        }
    }

    private void Inventory_OnItemListChanged(object sender, System.EventArgs e)
    {
        UpdateDisplay();
    }

    public void UpdateDisplay()
    {        
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        for (int item = 0; item < inventory.GetLength(); item++)
        {         
            RectTransform obj = Instantiate(inventorySlotTemplate, transform).GetComponent<RectTransform>();
            obj.gameObject.SetActive(true);

            if (!(inventory.GetItemFromCell(item) is null))
            {
                RectTransform objImage = Instantiate(itemImageInInventory, obj).GetComponent<RectTransform>();
                objImage.position = obj.position;

                Image image = objImage.GetComponent<Image>();
                image.sprite = inventory.GetItemFromCell(item).SpriteInInventory;
            }
        }
    }
}
