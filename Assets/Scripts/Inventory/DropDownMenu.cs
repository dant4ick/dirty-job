using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropDownMenu : MonoBehaviour
{
    [SerializeField] private Inventory _inventory;

    public void InputValue(int value)
    {
        if (value == 0)
        {

        }
        else
        {
            Transform curCell = transform.parent;
            int idOfCurCell = FindIdOfChild(curCell);
            Item.Item itemInCurCell = _inventory.GetItemFromCell(idOfCurCell);

            Player.PlayerController.Instance.DropWeapon(itemInCurCell, idOfCurCell, transform);
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
