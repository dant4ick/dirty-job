using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Item
{
    public class ItemInterface : MonoBehaviour
    {
        private bool _canPickUp;

        [SerializeField] private Item item;
        [SerializeField] private GameObject visualCuePreFab;
        [SerializeField] private Canvas firstTimePickUpCanvas;
        private GameObject _visualCue;
        private Quaternion _visualCueRotation;
        public Item Item { get { return item; } }

        protected virtual void Start()
        {
            float heightOfObject = transform.GetComponent<Collider2D>().bounds.size.y;
            float widthOfObject = transform.GetComponent<Collider2D>().bounds.size.x;

            _visualCue = Instantiate(visualCuePreFab, transform, false);

            _visualCue.SetActive(false);

            _visualCue.transform.position = new Vector2(transform.GetComponent<Collider2D>().bounds.center.x, transform.position.y + heightOfObject + 0.15f);
            _visualCueRotation = _visualCue.transform.rotation;
        }

        private void Update()
        {
            if (!_canPickUp)
                return;
            if (PlayerInventoryManager.Instance.inventory.IsFullCheck())
                return;

            if (Input.GetKeyDown(KeyCode.E))
            {
                Destroy(gameObject);

                if (firstTimePickUpCanvas != null && !PlayerInventoryManager.Instance.shownItems.AlreadySeenCheck(Item))
                {
                    GameObject canvasPopUp = Instantiate(firstTimePickUpCanvas).gameObject;
                    canvasPopUp.GetComponent<FirstTimePickUp>().SetScene(item.SpriteInInventory, item.Name, item.Description, this);

                    PlayerInventoryManager.Instance.shownItems.AddItem(Item);
                    return;
                }

                ContinuePickUp();
            }
        }

        public void ContinuePickUp()
        {
            if (item.Name == "Grenade")
            {
                Weapon weaponToKillYourself = (Weapon)item;
                Transform player = PlayerInventoryManager.Instance.transform;
                Instantiate(weaponToKillYourself.ParticleSystem, player.position, player.rotation);
                SoundManager.PlayWeaponSound(weaponToKillYourself.SoundOnAttack);
                player.GetComponent<HealthManager>().TakeDamage();                
            }
            else
            {
                PlayerInventoryManager.Instance.inventory.AddItem(item);                
            }
        }

        private void OnTriggerStay2D(Collider2D playerCollided)
        {
            if (playerCollided.TryGetComponent<Player.PlayerController>(out var player))
            {
                float heightOfObject = transform.GetComponent<Collider2D>().bounds.size.y;
                float widthOfObject = transform.GetComponent<Collider2D>().bounds.size.x;
                _visualCue.transform.rotation = _visualCueRotation;
                _visualCue.transform.position = new Vector2(transform.position.x + widthOfObject / 2, transform.position.y + heightOfObject + 0.15f);
                _visualCue.SetActive(true);
                _canPickUp = true;
                
            }
        }

        public void OnTriggerExit2D(Collider2D playerCollided)
        {
            _visualCue.SetActive(false);
            _canPickUp = false;
        }
    }
}
