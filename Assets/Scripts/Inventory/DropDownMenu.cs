using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropDownMenu : MonoBehaviour
{
    [SerializeField] private Inventory inventory;
    [SerializeField] private List<Item.Item> itemsInMedKit;

    public void InputValue(int value)
    {
        if (value == 0)
        {
            Transform curCell = transform.parent;
            int idOfCurCell = FindIdOfChild(curCell);
            Item.Item itemInCurCell = inventory.GetItemFromCell(idOfCurCell);

            if (itemInCurCell is Weapon)
            {
                Weapon weaponToKillYourself = (Weapon)itemInCurCell;
                if (weaponToKillYourself.ParticleSystem != null)
                {                   
                    Transform player = PlayerInventoryManager.Instance.transform;
                    Instantiate(weaponToKillYourself.ParticleSystem, player.position, player.rotation);
                    player.GetComponent<HealthManager>().TakeDamage();
                }
            }
            else if (itemInCurCell.Name == "Syringe")
            {
                Player.PlayerController.Instance.GetComponent<HealthManager>().TakeSubstance();

                PlayerInventoryManager.Instance.DestroyItem(idOfCurCell, transform);
            }
            else if (itemInCurCell.Name == "Medkit")
            {
                PlayerInventoryManager.Instance.DestroyItem(idOfCurCell, transform);

                foreach (Item.Item item in itemsInMedKit)
                {
                    Instantiate(item.PreFab, Player.PlayerController.Instance.transform.position, Player.PlayerController.Instance.transform.rotation);   
                }                
            }
        }
        else
        {
            Transform curCell = transform.parent;
            int idOfCurCell = FindIdOfChild(curCell);
            Item.Item itemInCurCell = inventory.GetItemFromCell(idOfCurCell);

            PlayerInventoryManager.Instance.DropItem(itemInCurCell, idOfCurCell, transform);
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
