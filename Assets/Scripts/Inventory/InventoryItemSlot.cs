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

            if (prevCell.GetComponentInParent<DisplayInventoy>() != null)
            { 
                itemInPrevCell = prevCell.GetComponentInParent<DisplayInventoy>().inventory.GetItemFromCell(idOfPrevCell);
                prevCell.GetComponentInParent<DisplayInventoy>().inventory.SetItemToCell(null, idOfPrevCell);
            }
            else
            {
                itemInPrevCell = prevCell.GetComponentInParent<DisplayEquipment>().equipment.GetWeaponFromCell(idOfPrevCell);
                prevCell.GetComponentInParent<DisplayEquipment>().equipment.SetWeaponToCell(null, idOfPrevCell);
            }

            int idOfCurCell = FindIdOfChild(transform);
            transform.GetComponentInParent<DisplayInventoy>().inventory.SetItemToCell(itemInPrevCell, idOfCurCell);

            eventData.pointerDrag.GetComponent<RectTransform>().transform.SetParent(transform, false);
            eventData.pointerDrag.GetComponent<RectTransform>().position = GetComponent<RectTransform>().position;
        }
        else
        {
            Transform prevCell = eventData.pointerDrag.transform.parent;
            int idOfPrevCell = FindIdOfChild(prevCell);
            Item.Item itemInPrevCell;

            Transform curCell = transform;
            int idOfCurCell = FindIdOfChild(curCell);
            Item.Item itemInCurCell = transform.GetComponentInParent<DisplayInventoy>().inventory.GetItemFromCell(idOfCurCell);

            if (prevCell.GetComponentInParent<DisplayInventoy>() != null)
            {
                itemInPrevCell = prevCell.GetComponentInParent<DisplayInventoy>().inventory.GetItemFromCell(idOfPrevCell);
                prevCell.GetComponentInParent<DisplayInventoy>().inventory.SetItemToCell(null, idOfPrevCell);

                prevCell.GetComponentInParent<DisplayInventoy>().inventory.SetItemToCell(itemInCurCell, idOfPrevCell);
            }
            else
            {
                itemInPrevCell = prevCell.GetComponentInParent<DisplayEquipment>().equipment.GetWeaponFromCell(idOfPrevCell);
                prevCell.GetComponentInParent<DisplayEquipment>().equipment.SetWeaponToCell(null, idOfPrevCell);

                prevCell.GetComponentInParent<DisplayEquipment>().equipment.SetWeaponToCell((Weapon)itemInCurCell, idOfPrevCell);
            }

            transform.GetComponentInParent<DisplayInventoy>().inventory.SetItemToCell(itemInPrevCell, idOfCurCell);

            RectTransform childOfCurCell = curCell.GetChild(1).GetComponent<RectTransform>();
            RectTransform childOfPrevCell = eventData.pointerDrag.GetComponent<RectTransform>();

            childOfCurCell.transform.SetParent(childOfPrevCell.parent, false);
            childOfCurCell.position = childOfPrevCell.parent.GetComponent<RectTransform>().position;

            childOfPrevCell.transform.SetParent(transform, false);
            childOfPrevCell.position = GetComponent<RectTransform>().position;
        }
    }

    private int FindIdOfChild(Transform childToFindId)
    {
        for (int child = 0; child < transform.parent.childCount; child++)
        {
            if (childToFindId.parent.GetChild(child) == childToFindId)
            {
                return child;
            }
        }
        return 0;
    }
}
