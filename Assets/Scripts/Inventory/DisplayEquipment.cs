using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayEquipment : MonoBehaviour
{
    public Equipment equipment;
    public Transform equipmentSlotTemplate;
    public Transform itemImageInEquipment;

    // Start is called before the first frame update
    void Start()
    {
        equipment.SetCells();
        UpdateDisplay();
    }

    private void Equipment_OnWeaponListChanged(object sender, System.EventArgs e)
    {
        UpdateDisplay();
    }

    void UpdateDisplay()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        RectTransform slot1 = Instantiate(equipmentSlotTemplate, transform).GetComponent<RectTransform>();
        slot1.gameObject.SetActive(true);

        if (!(equipment.GetRangeWeapon() is null))
        {
            RectTransform objImage = Instantiate(itemImageInEquipment, slot1).GetComponent<RectTransform>();
            objImage.position = slot1.position;

            Image image = objImage.GetComponent<Image>();
            image.sprite = equipment.GetRangeWeapon().SpriteInInventory;
        }

        RectTransform slot2 = Instantiate(equipmentSlotTemplate, transform).GetComponent<RectTransform>();
        slot2.gameObject.SetActive(true);

        if (!(equipment.GetMeleeWeapon() is null))
        {
            RectTransform objImage = Instantiate(itemImageInEquipment, slot2).GetComponent<RectTransform>();
            objImage.position = slot2.position;

            Image image = objImage.GetComponent<Image>();
            image.sprite = equipment.GetMeleeWeapon().SpriteInInventory;
        }
    }
}
