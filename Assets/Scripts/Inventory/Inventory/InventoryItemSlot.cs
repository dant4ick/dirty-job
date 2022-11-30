using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryItemSlot : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null)
            return;

        if (transform.childCount < 2)
        {
            Transform prevCell = eventData.pointerDrag.transform.parent;
            int idOfPrevCell = FindIdOfChild(prevCell);
            Item.Item itemInPrevCell;

            RectTransform childOfPrevCell = eventData.pointerDrag.GetComponent<RectTransform>();

            if (prevCell.GetComponentInParent<DisplayInventory>() != null)
            { 
                itemInPrevCell = prevCell.GetComponentInParent<DisplayInventory>().inventory.GetItemFromCell(idOfPrevCell);
                prevCell.GetComponentInParent<DisplayInventory>().inventory.SetItemToCell(null, idOfPrevCell);
            }
            else
            {
                itemInPrevCell = prevCell.GetComponentInParent<DisplayEquipment>().equipment.GetWeaponFromCell(idOfPrevCell);
                prevCell.GetComponentInParent<DisplayEquipment>().equipment.SetWeaponToCell(null, idOfPrevCell);

                childOfPrevCell.transform.GetChild(0).gameObject.SetActive(true);
            }

            int idOfCurCell = FindIdOfChild(transform);
            transform.GetComponentInParent<DisplayInventory>().inventory.SetItemToCell(itemInPrevCell, idOfCurCell);

            childOfPrevCell.GetComponent<RectTransform>().transform.SetParent(transform, false);
            childOfPrevCell.GetComponent<RectTransform>().position = GetComponent<RectTransform>().position;
        }
        else
        {
            Transform prevCell = eventData.pointerDrag.transform.parent;
            int idOfPrevCell = FindIdOfChild(prevCell);
            Item.Item itemInPrevCell;

            Transform curCell = transform;
            int idOfCurCell = FindIdOfChild(curCell);
            Item.Item itemInCurCell = transform.GetComponentInParent<DisplayInventory>().inventory.GetItemFromCell(idOfCurCell);

            RectTransform childOfCurCell = curCell.GetChild(1).GetComponent<RectTransform>();
            RectTransform childOfPrevCell = eventData.pointerDrag.GetComponent<RectTransform>();

            if (prevCell.GetComponentInParent<DisplayInventory>() != null)
            {
                itemInPrevCell = prevCell.GetComponentInParent<DisplayInventory>().inventory.GetItemFromCell(idOfPrevCell);
                prevCell.GetComponentInParent<DisplayInventory>().inventory.SetItemToCell(null, idOfPrevCell);

                prevCell.GetComponentInParent<DisplayInventory>().inventory.SetItemToCell(itemInCurCell, idOfPrevCell);
            }
            else
            {
                itemInPrevCell = prevCell.GetComponentInParent<DisplayEquipment>().equipment.GetWeaponFromCell(idOfPrevCell);

                if (itemInPrevCell.GetType() != itemInCurCell.GetType())
                    return;

                prevCell.GetComponentInParent<DisplayEquipment>().equipment.SetWeaponToCell(null, idOfPrevCell);

                prevCell.GetComponentInParent<DisplayEquipment>().equipment.SetWeaponToCell((Weapon)itemInCurCell, idOfPrevCell);

                childOfPrevCell.transform.GetChild(0).gameObject.SetActive(true);
                childOfCurCell.transform.GetChild(0).gameObject.SetActive(false);
            }

            transform.GetComponentInParent<DisplayInventory>().inventory.SetItemToCell(itemInPrevCell, idOfCurCell);

            childOfCurCell.transform.SetParent(childOfPrevCell.parent, false);
            childOfCurCell.position = childOfPrevCell.parent.GetComponent<RectTransform>().position;

            childOfPrevCell.transform.SetParent(transform, false);
            childOfPrevCell.position = GetComponent<RectTransform>().position;
        }
    }

    private int FindIdOfChild(Transform childToFindId)
    {
        for (int child = 0; child < childToFindId.parent.childCount; child++)
        {
            if (childToFindId.parent.GetChild(child) == childToFindId)
            {
                return child;
            }
        }
        return 0;
    }
}
