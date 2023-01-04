using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    private static readonly int BossDeath = Animator.StringToHash("Boss_Death");

    [SerializeField] private AudioClip clip;
    [SerializeField] private GameObject dialogue;

    [SerializeField] private GameObject mainDialogue;

    [SerializeField] private GameObject activeCamera;
    [SerializeField] private GameObject inventory;

    [SerializeField] private GameObject player;

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
        Transform dialogueToSay = Instantiate(dialogueBeforeDeath, transform).transform.GetChild(1);
        dialogueToSay.GetComponent<DialogueAboveEnemy>().mainCamera = transform.GetComponent<Player.PlayerController>().mainCamera;
        dialogueToSay.GetComponent<DialogueAboveEnemy>().inputPhrase.Add("I feel kinda funny...");        
        
        yield return new WaitForSeconds(3f);

        Die();
    }

    private void Die()
    {
        SoundManager.PlayCharacterSound(clip);

        if (gameObject.layer == LayerMask.NameToLayer("Player") || gameObject.layer == LayerMask.NameToLayer("Dead"))
        {
            Destroy(mainDialogue);
            SoundManager.instance.MusicSource.Stop();
            Destroy(gameObject.GetComponent<Player.PlayerController>());
            Destroy(gameObject.GetComponent<PlayerAttackManager>());
            Destroy(gameObject.GetComponent<PlayerInventoryManager>());
            if (SceneManager.GetActiveScene().name != "Level4")
            {
                gameOverScreen.GameOver();                
            }
            else
            {
                activeCamera.SetActive(false);
                inventory.SetActive(false);
            }
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

            if (activeCamera != null)
            {
                Destroy(player.GetComponent<Player.PlayerController>());
                Destroy(player.GetComponent<PlayerAttackManager>());
                Destroy(player.GetComponent<PlayerInventoryManager>());
                
                animator.Play(BossDeath);

                activeCamera.SetActive(false);
                inventory.SetActive(false);
            }

            if (dialogue != null)
                Destroy(dialogue);

        }

        hand.SetActive(false);
        gameObject.layer = LayerMask.NameToLayer("Dead");

        isAlive = false;
    }
}
