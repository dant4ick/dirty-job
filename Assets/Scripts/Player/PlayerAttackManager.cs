using System;
using Item.Weapon;
using UnityEngine;

public class PlayerAttackManager : MonoBehaviour
{
    [SerializeField] private GameObject _emptySniperRifle;
    [SerializeField] private GameObject _equipmentPanel;

    public static Action<LayerMask> shootInput;
    public static Action attackInput;

    private LayerMask _enemyLayers;

    private void Start()
    {
        _enemyLayers = LayerMask.GetMask("Enemy", "EnemyThroughPlatform", "Ground");
    }

    public void Shoot()
    {
        shootInput?.Invoke(_enemyLayers);
        PlayerInventoryManager playerInventoryManager = GetComponent<PlayerInventoryManager>();

        if (playerInventoryManager.equipment.GetRangeWeapon() == null)       
            return;

        if (playerInventoryManager.equipment.GetRangeWeapon().Name != "Sniper Rifle")
            return;

        playerInventoryManager.equipment.SetRangeWeapon(null);
        Destroy(_equipmentPanel.transform.GetChild(0).GetChild(1).gameObject);

        GameObject emptySniperRifle = Instantiate(_emptySniperRifle, transform.position, new Quaternion());

        emptySniperRifle.GetComponent<Rigidbody2D>().AddForce(new Vector2(2000f * -transform.localScale.x, 3f));
    }

    public void Attack()
    {
        attackInput?.Invoke();
    }
}