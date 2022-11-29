using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    public bool isAlive = true;
    [SerializeField] private GameObject hand;

    [Header("Friction materials")]
    [SerializeField] private PhysicsMaterial2D maxFriction;

    [SerializeField] private GameOverScreen gameOverScreen;

    public void TakeDamage()
    {
        Die();
    }

    public void TakeStun(float seconds)
    {
        Debug.Log("STUN");
    }
    public void TakeBleed(GameObject particleSystem)
    {
        Instantiate(particleSystem, transform.position, new Quaternion());
    }

    public void TakeBleed(GameObject particleSystem, Vector2 position, Quaternion rotation)
    {
        Instantiate(particleSystem, position, rotation);
    }

    void Die()
    {
        if (gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Destroy(gameObject.GetComponent<Player.PlayerController>());
            Destroy(gameObject.GetComponent<PlayerAttackManager>());
            Destroy(gameObject.GetComponent<PlayerInventoryManager>());
            gameOverScreen.GameOver();
        }
        else
        {
            Instantiate(gameObject.GetComponent<EnemyAttackManager>().rangeWeapon.Item.PreFab.gameObject, transform.position, new Quaternion());            

            Destroy(gameObject.GetComponent<Pathfinding.Seeker>());
            Destroy(gameObject.GetComponent<EnemyAI>());
            Destroy(gameObject.GetComponent<EnemyAttackManager>());
            Destroy(gameObject.GetComponent<AlarmManager>());

            for (int child = 0; child < transform.childCount; child++)
                Destroy(transform.GetChild(0).gameObject);
        }

        gameObject.GetComponent<Rigidbody2D>().sharedMaterial = maxFriction;

        hand.SetActive(false);
        gameObject.layer = LayerMask.NameToLayer("Dead");
        isAlive = false;
    }
}
