using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EquipmentItemSlot : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null)
            return;

        if (transform.childCount < 2)
        {
            Transform prevCell = eventData.pointerDrag.transform.parent;
            int idOfPrevCell = FindIdOfChild(prevCell);
            Item.Item itemInPrevCell = prevCell.GetComponentInParent<DisplayInventoy>().inventory.GetItemFromCell(idOfPrevCell);

            if (FindIdOfChild(transform) == 0)
            {
                if (!(itemInPrevCell is RangeWeapon))
                    return;
            }
            else
            {
                if (!(itemInPrevCell is MeleeWeapon))
                    return;
            }

            prevCell.GetComponentInParent<DisplayInventoy>().inventory.SetItemToCell(null, idOfPrevCell);

            int idOfCurCell = FindIdOfChild(transform);
            transform.GetComponentInParent<DisplayEquipment>().equipment.SetWeaponToCell((Weapon)itemInPrevCell, idOfCurCell);

            eventData.pointerDrag.GetComponent<RectTransform>().transform.SetParent(transform, false);
            eventData.pointerDrag.GetComponent<RectTransform>().position = GetComponent<RectTransform>().position;
        }
        else
        {
            Transform prevCell = eventData.pointerDrag.transform.parent;
            int idOfPrevCell = FindIdOfChild(prevCell);
            Item.Item itemInPrevCell = prevCell.GetComponentInParent<DisplayInventoy>().inventory.GetItemFromCell(idOfPrevCell);

            Transform curCell = transform;
            int idOfCurCell = FindIdOfChild(curCell);
            Item.Item itemInCurCell = transform.GetComponentInParent<DisplayEquipment>().equipment.GetWeaponFromCell(idOfCurCell);

            prevCell.GetComponentInParent<DisplayInventoy>().inventory.SetItemToCell(itemInCurCell, idOfPrevCell);
            transform.GetComponentInParent<DisplayEquipment>().equipment.SetWeaponToCell((Weapon)itemInPrevCell, idOfCurCell);

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