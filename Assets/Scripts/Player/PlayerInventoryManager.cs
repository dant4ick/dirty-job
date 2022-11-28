using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventoryManager : MonoBehaviour
{
    private static PlayerInventoryManager _instance;

    public static PlayerInventoryManager Instance
    {
        get { return _instance; }
    }

    public Inventory savedInventory;
    public Equipment savedEquipment;

    public Inventory inventory;
    public Equipment equipment;

    private GameObject _rangeWeapon;
    private GameObject _meleeWeapon;
    [Header("Inventory canvas")]
    [SerializeField] private GameObject inventoryCanvas;

    [Header("Weapon positions")]
    [SerializeField] private Transform attackPoint;
    [SerializeField] private Transform grabPoint;

    [Header("Player sprites")]
    [SerializeField] private GameObject hand;
    [SerializeField] private Sprite noWeaponSprite;
    [SerializeField] private Sprite withRangeWeaponSprite;

    private void Start()
    {
        _instance = this;
        CloseInvenotory();
        equipment.OnWeaponListChanged += Equipment_OnWeaponListChanged;
    }

    private void OnDestroy()
    {
        equipment.OnWeaponListChanged -= Equipment_OnWeaponListChanged;
    }    

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (inventoryCanvas.activeSelf)
            {
                CloseInvenotory();                
            }
            else
            {
                ShowInventory();
            }
        }        
    }

    private void CloseInvenotory()
    {
        inventoryCanvas.SetActive(false);
    }

    private void ShowInventory()
    {
        inventoryCanvas.SetActive(true);
    }

    private void Equipment_OnWeaponListChanged(object sender, System.EventArgs e)
    {
        UpdateEquipment();
    }

    public void UpdateEquipment()
    {
        if (_meleeWeapon != null)
        {
            PlayerAttackManager.attackInput -= _meleeWeapon.GetComponent<Item.Weapon.MeleeWeaponInterface>().Attack;
            Destroy(_meleeWeapon);
        }

        if (_rangeWeapon != null)
        {
            PlayerAttackManager.shootInput -= _rangeWeapon.GetComponent<Item.Weapon.RangeWeaponInterface>().Shoot;
            Destroy(_rangeWeapon);
        }

        if (hand.activeSelf)
        {
            hand.SetActive(false);
        }

        if (equipment.GetRangeWeapon() == null && equipment.GetMeleeWeapon() == null)
        {
            transform.GetComponent<SpriteRenderer>().sprite = noWeaponSprite;
            return;
        }

        if (equipment.GetRangeWeapon() != null)
        {
            transform.GetComponent<SpriteRenderer>().sprite = withRangeWeaponSprite;

            hand.SetActive(true);

            Transform rangeWeapon = Instantiate(equipment.GetRangeWeapon().PreFab, grabPoint.transform).GetComponent<Transform>();
            rangeWeapon.GetComponent<PolygonCollider2D>().enabled = false;
            Destroy(rangeWeapon.GetComponent<Rigidbody2D>());
            Destroy(rangeWeapon.GetComponentInChildren<CircleCollider2D>(true));
            rangeWeapon.position = grabPoint.position;
            PlayerAttackManager.shootInput += rangeWeapon.GetComponent<Item.Weapon.RangeWeaponInterface>().Shoot;

            _rangeWeapon = rangeWeapon.gameObject;
        }

        if (equipment.GetMeleeWeapon() != null)
        {
            Transform meleeWeapon = Instantiate(equipment.GetMeleeWeapon().PreFab, attackPoint)
                .GetComponent<Transform>();
            meleeWeapon.GetComponent<PolygonCollider2D>().enabled = false;
            Destroy(meleeWeapon.GetComponent<Rigidbody2D>());

            meleeWeapon.GetComponent<SpriteRenderer>().sprite = null;

            meleeWeapon.GetComponent<Item.Weapon.MeleeWeaponInterface>().attackPoint = attackPoint;

            PlayerAttackManager.attackInput += meleeWeapon.GetComponent<Item.Weapon.MeleeWeaponInterface>().Attack;

            _meleeWeapon = meleeWeapon.gameObject;
        }
    }

    public void DropWeapon(Item.Item item, int positionInInventory, Transform objectInInventory)
    {
        Instantiate(item.PreFab, transform.position, transform.rotation);
        inventory.SetItemToCell(null, positionInInventory);

        Destroy(objectInInventory.gameObject);
    }

    private void OnApplicationQuit()
    {
        inventory.Clear();
        equipment.Clear();

        savedInventory.Clear();
        equipment.Clear();
    }
}
