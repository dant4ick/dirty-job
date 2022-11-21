using System;
using Item.Weapon;
using UnityEngine;

public class PlayerAttackManager : MonoBehaviour
{
    [SerializeField] private Camera _mainCamera;

    public static Action shootInput;
    public static Action attackInput;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            shootInput?.Invoke();
        }
        else if (Input.GetMouseButton(1))
        {
            attackInput?.Invoke();
        }
    }
}
