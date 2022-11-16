using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayInventoy : MonoBehaviour
{
    public Inventory inventory;
    public Transform inventorySlotTemplate;
    public Transform inventoryPannel;
    public Transform itemImageInInventory;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 6; i++)
        {
            inventory.AddCell();
        }
        UpdateDisplay();
        inventory.OnItemListChanged += Inventory_OnItemListChanged;
    }

    private void Inventory_OnItemListChanged(object sender, System.EventArgs e)
    {
        UpdateDisplay();
    }

    public void UpdateDisplay()
    {        
        foreach (Transform child in inventoryPannel)
        {
            Destroy(child.gameObject);
        }

        foreach (InventoryCell item in inventory.ItemList)
        {         
            RectTransform obj = Instantiate(inventorySlotTemplate, inventoryPannel).GetComponent<RectTransform>();
            obj.gameObject.SetActive(true);

            if (!(item.GetItem() is null))
            {
                RectTransform objImage = Instantiate(itemImageInInventory, obj).GetComponent<RectTransform>();
                objImage.position = obj.position;

                Image image = objImage.GetComponent<Image>();
                image.sprite = item.GetItem().Sprite;
            }
        }
    }
}
