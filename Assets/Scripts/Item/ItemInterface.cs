using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Item
{
    public class ItemInterface : MonoBehaviour
    {
        private bool _canPickUp;

        [SerializeField] private Item _item;
        [SerializeField] private GameObject _visualCuePreFab;
        private GameObject _visualCue;
        private Quaternion _visualCueRotation;
        public Item Item { get { return _item; } }

        protected virtual void Start()
        {
            float heightOfObject = transform.GetComponent<PolygonCollider2D>().bounds.size.y;
            float widthOfObject = transform.GetComponent<PolygonCollider2D>().bounds.size.x;

            _visualCue = Instantiate(_visualCuePreFab, transform, false);

            _visualCue.SetActive(false);

            _visualCue.transform.position = new Vector2(transform.GetComponent<PolygonCollider2D>().bounds.center.x, transform.position.y + heightOfObject + 0.15f);
            _visualCueRotation = _visualCue.transform.rotation;
        }

        private void Update()
        {
            if (_canPickUp)
                if (Input.GetKeyDown(KeyCode.E))
                {
                    if (PlayerInventoryManager.Instance.inventory.AddItem(_item))
                        Destroy(gameObject);
                }
        }

        private void OnTriggerStay2D(Collider2D playerCollided)
        {
            if (playerCollided.TryGetComponent<Player.PlayerController>(out var player))
            {
                float heightOfObject = transform.GetComponent<PolygonCollider2D>().bounds.size.y;
                float widthOfObject = transform.GetComponent<PolygonCollider2D>().bounds.size.x;
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
