using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Equipment", menuName = "ScriptableObjects/Inventory System/Equipment")]
public class Equipment : ScriptableObject
{
    [SerializeField] private List<EquipmentCell> _weaponList = new List<EquipmentCell>();
    //public List<EquipmentCell> WeaponList { get { return _weaponList; } }

    public event EventHandler OnWeaponListChanged;

    public void SetCells()
    {
        if (_weaponList.Count >= 2)
            return;

        int cellInListCount = _weaponList.Count;

        for (int cellCount = 0; cellCount < 2 - cellInListCount; cellCount++)
            _weaponList.Add(new EquipmentCell());
    }

    public RangeWeapon GetRangeWeapon()
    {
        return (RangeWeapon)_weaponList[0].GetWeapon();
    }
    public void SetRangeWeapon(RangeWeapon rangeWeapon)
    {
        _weaponList[0].SetWeapon(rangeWeapon);
        OnWeaponListChanged?.Invoke(this, EventArgs.Empty);
    }

    public MeleeWeapon GetMeleeWeapon()
    {
        return (MeleeWeapon)_weaponList[1].GetWeapon();
    }
    public void SetMeleeWeapon(MeleeWeapon meleeWeapon)
    {
        _weaponList[1].SetWeapon(meleeWeapon);
        OnWeaponListChanged?.Invoke(this, EventArgs.Empty);
    }

    public Weapon GetWeaponFromCell(int position)
    {
        return _weaponList[position].GetWeapon();
    }
    public void SetWeaponToCell(Weapon weapon, int position)
    {
        _weaponList[position].SetWeapon(weapon);
        OnWeaponListChanged?.Invoke(this, EventArgs.Empty);
    }

    public void Clear()
    {
        _weaponList.Clear();
    }
}

[System.Serializable]
public class EquipmentCell
{
    [SerializeField] private Weapon _weapon;

    public Weapon GetWeapon()
    {
        return _weapon;
    }

    public void SetWeapon(Weapon item)
    {
        _weapon = item;
    }
}