using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FirstTimePickUp : MonoBehaviour
{
    [SerializeField] private Image itemImageHolder;
    [SerializeField] private GameObject itemNameHolder;
    [SerializeField] private GameObject itemDescriptionHolder;

    private Item.ItemInterface _item; 

    public void SetScene(Sprite sprite, string name, string description, Item.ItemInterface item)
    {
        Time.timeScale = 0;

        itemImageHolder.sprite = sprite;
        itemNameHolder.GetComponent<TextMeshProUGUI>().text = name;
        itemDescriptionHolder.GetComponent<TextMeshProUGUI>().text = description;
        _item = item;
    }

    void Update()
    {
        if (Input.anyKeyDown)
        {
            Time.timeScale = 1;
            _item.ContinuePickUp();
            Destroy(gameObject);
        }
    }
}
