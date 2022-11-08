using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int health = 1;

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Die();
        }
    }

    public void TakeStun(float seconds)
    {
        // Когда бъешь типо битой и тп то чел глушиться
        Debug.Log("STUN");
    }

    public void TakeBleed()
    {
        Debug.Log("BLEED");
    }

    void Die()
    {
        Debug.Log("DED");
    }
}
