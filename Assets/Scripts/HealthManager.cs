using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    private bool _alive = true;

    public void TakeDamage()
    {
        Die();
    }

    public void TakeStun(float seconds)
    {
        Debug.Log("STUN");
    }

    public void TakeBleed()
    {
        Debug.Log("BLEED");
    }

    void Die()
    {
        _alive = false;
        Debug.Log(this);
    }
}
