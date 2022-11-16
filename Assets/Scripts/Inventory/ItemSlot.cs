using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null)
            return;

        if (transform.childCount < 2)
        {
            Item.Item itemInPrevCell = null;

            for (int child = 0; child < transform.parent.childCount; child++)
            {
                if (eventData.pointerDrag.transform.parent.parent.GetChild(child) == eventData.pointerDrag.transform.parent)
                {
                    itemInPrevCell = transform.GetComponentInParent<DisplayInventoy>().inventory.ItemList[child].GetItem();
                    transform.GetComponentInParent<DisplayInventoy>().inventory.SetItem(null, child);
                    break;
                }
            }
            //eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
            eventData.pointerDrag.GetComponent<RectTransform>().transform.SetParent(transform, false);
            eventData.pointerDrag.GetComponent<RectTransform>().position = GetComponent<RectTransform>().position;

            for (int child = 0; child < transform.parent.childCount; child++)
            {
                if (transform.parent.GetChild(child) == transform)
                {
                    transform.GetComponentInParent<DisplayInventoy>().inventory.SetItem(itemInPrevCell, child);
                    break;
                }
            }
        }
        else
        {
            //swap weapons
        }
    }
}
