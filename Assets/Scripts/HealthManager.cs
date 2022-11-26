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

    public void TakeBleed(GameObject particleSystem, Vector2 position, Quaternion rotation)
    {
        Instantiate(particleSystem, position, rotation);
    }

    void Die()
    {
        _alive = false;
        Debug.Log(this);
    }
}
