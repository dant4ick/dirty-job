using System;
using Item.Weapon;
using UnityEngine;

public class PlayerAttackManager : MonoBehaviour
{
    public static Action<LayerMask> shootInput;
    public static Action attackInput;

    private LayerMask _enemyLayers;

    private void Start()
    {
        _enemyLayers = LayerMask.GetMask("Enemy", "EnemyThroughPlatform");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            shootInput?.Invoke(_enemyLayers);            
        }
        else if (Input.GetMouseButton(1))
        {
            attackInput?.Invoke();
        }
    }
}