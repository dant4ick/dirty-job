using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayInventoy : MonoBehaviour
{
    public Inventory inventory;
    public Transform inventorySlotTemplate;
    public Transform itemImageInInventory;

    // Start is called before the first frame update
    void Start()
    {
        inventory.SetCells();
        UpdateDisplay();
        inventory.OnItemListChanged += Inventory_OnItemListChanged;
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
                image.sprite = inventory.GetItemFromCell(item).Sprite;
            }
        }
    }
}
