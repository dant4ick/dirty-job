using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Item
{
    public class ItemInterface : MonoBehaviour
    {
        [SerializeField] private Item _item;
        [SerializeField] private GameObject _visualCue;
        public Item Item { get { return _item; } }

        public void OnTriggerStay2D(Collider2D playerCollided)
        {
            var player = playerCollided.GetComponent<Player.PlayerController>();
            if (player)
            {
                _visualCue.SetActive(true);

                if (Input.GetKeyDown(KeyCode.E))
                {
                    player.inventory.AddItem(_item);
                    Destroy(gameObject);
                }
            }
        }

        public void OnTriggerExit2D(Collider2D playerCollided)
        {
            _visualCue.SetActive(false);
        }
    }
}
