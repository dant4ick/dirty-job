using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    public bool isAlive = true;
    [SerializeField] private GameObject hand;
    [SerializeField] private bool dropOnDeath = true;

    [Header("Friction materials")]
    [SerializeField] private PhysicsMaterial2D maxFriction;

    [SerializeField] private GameOverScreen gameOverScreen;
    [SerializeField] private GameObject dialogueBeforeDeath;

    [SerializeField] private Animator animator;
    private static readonly int EnemyDeath = Animator.StringToHash("Enemy_Death");

    [SerializeField] private AudioClip clip;
    [SerializeField] private GameObject dialogue;

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

    public void TakeSubstance()
    {
        gameObject.layer = LayerMask.NameToLayer("Dead");
        StartCoroutine(DieSlowly());
    }

    private IEnumerator DieSlowly()
    {
        Transform dialogueToSay = Instantiate(dialogueBeforeDeath, transform).transform.GetChild(0);
        dialogueToSay.GetComponent<DialogueAboveEnemy>().mainCamera = transform.GetComponent<Player.PlayerController>().mainCamera;
        dialogueToSay.GetComponent<DialogueAboveEnemy>().inputPhrase.Add("I'm feel kinda funny");        
        
        yield return new WaitForSeconds(3f);

        Die();
    }

    private void Die()
    {
        SoundManager.PlayCaracterSound(clip);

        if (gameObject.layer == LayerMask.NameToLayer("Player") || gameObject.layer == LayerMask.NameToLayer("Dead"))
        {
            Destroy(gameObject.GetComponent<Player.PlayerController>());
            Destroy(gameObject.GetComponent<PlayerAttackManager>());
            Destroy(gameObject.GetComponent<PlayerInventoryManager>());
            gameOverScreen.GameOver();
        }
        else
        {
            animator.Play(EnemyDeath);

            if (dropOnDeath)
                Instantiate(gameObject.GetComponent<EnemyAttackManager>().rangeWeapon.Item.PreFab.gameObject, transform.position, new Quaternion());            

            Destroy(gameObject.GetComponent<Pathfinding.Seeker>());
            Destroy(gameObject.GetComponent<EnemyAI>());
            Destroy(gameObject.GetComponent<EnemyAttackManager>());
            //Destroy(gameObject.GetComponent<AlarmManager>());

            Destroy(gameObject.GetComponent<BossAI>());

            for (int child = 0; child < transform.childCount; child++)
                Destroy(transform.GetChild(0).gameObject);

            if (gameOverScreen != null)
                gameOverScreen.GameOver();

            if (dialogue != null)
                Destroy(dialogue);

        }


        hand.SetActive(false);
        gameObject.layer = LayerMask.NameToLayer("Dead");


        isAlive = false;
    }
}
