using System;
using Item.Weapon;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private Camera _mainCamera;

    public static Action<Vector2> shootInput;
    public static Action attackInput;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Vector2 mousePosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            //RangeWeaponInterface weaponToShoot = GetComponentInChildren<RangeWeaponInterface>();
            shootInput?.Invoke(mousePosition);
        }
        else if (Input.GetMouseButton(1))
        {
            attackInput?.Invoke();
        }
    }
}
