using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayInventoy : MonoBehaviour
{
    public Inventory inventory;
    public Transform inventorySlotTemplate;
    public Transform inventoryPannel;
    Dictionary<int, GameObject> itemDisplayed = new Dictionary<int, GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        CreateDisplay();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateDisplay();
    }

    public void CreateDisplay()
    {
        foreach (Item.Item item in inventory.ItemList)
        {         
            RectTransform obj = Instantiate(inventorySlotTemplate, inventoryPannel).GetComponent<RectTransform>();
            obj.gameObject.SetActive(true);
            Image image = obj.Find("Image").GetComponent<Image>();

            image.sprite = item.Sprite;

            itemDisplayed.Add(item.GetInstanceID(), obj.gameObject);
        }
    }

    public void UpdateDisplay()
    {
        foreach (Item.Item item in inventory.ItemList)
        {
            if (!itemDisplayed.ContainsKey(item.GetInstanceID()))
            {
                RectTransform obj = Instantiate(inventorySlotTemplate, inventoryPannel).GetComponent<RectTransform>();
                obj.gameObject.SetActive(true);
                Image image = obj.Find("Image").GetComponent<Image>();

                image.sprite = item.Sprite;

                itemDisplayed.Add(item.GetInstanceID(), obj.gameObject);
            }
        }
    }
}
