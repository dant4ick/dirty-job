using Item.Weapon;
using UnityEngine;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        public Inventory inventory;
        public Equipment equipment;

        private CircleCollider2D _feetCollider;
        private BoxCollider2D _headCollider;
        private Rigidbody2D _rigidbody;

        private const float MoveSpeed = 6f;
        private float _horizontalMovement;

        private static int _groundLayer;

        private GameObject _rangeWeapon = null;

        [SerializeField] private GameObject _handPrefab;
        [SerializeField] private Sprite _noWeaponSprite;
        [SerializeField] private Sprite _withRangeWeaponSprite;
        private GameObject _hand = null;

        private void Start()
        {
            _rigidbody = gameObject.GetComponent<Rigidbody2D>();
            _feetCollider = gameObject.GetComponent<CircleCollider2D>();
            _headCollider = gameObject.GetComponent<BoxCollider2D>();
            
            _groundLayer = LayerMask.GetMask("Ground");

            UpdateEquipment();

            equipment.OnWeaponListChanged += Equipment_OnWeaponListChanged;
        }

        private void Update()
        {
            _horizontalMovement = Input.GetAxisRaw("Horizontal");
        }

        private void FixedUpdate()
        {
            if (!IsGrounded()) return;
            _rigidbody.velocity = _horizontalMovement != 0f ? new Vector2(_horizontalMovement * MoveSpeed, _rigidbody.velocity.y) : new Vector2(0f, _rigidbody.velocity.y);
        }

        private bool IsGrounded()
        {       
            return Physics2D.BoxCast(_feetCollider.bounds.center, new Vector2(1f, 1f), 0f, Vector2.down, 0.2f, _groundLayer);
        }

        public void OnTriggerEnter2D(Collider2D other)
        {
            var item = other.GetComponent<Item.ItemInterface>();
            if (item)
            {
                inventory.AddItem(item.Item);
                Destroy(other.gameObject);
            }
        }
        private void Equipment_OnWeaponListChanged(object sender, System.EventArgs e)
        {
            UpdateEquipment();
        }

        public void UpdateEquipment()
        {
            if (_rangeWeapon != null)
            {
                PlayerShoot.shootInput -= _rangeWeapon.GetComponent<RangeWeaponInterface>().Shoot;
                Destroy(_rangeWeapon);
            }
            if (_hand != null)
            {
                Destroy(_hand);
            }

            if (equipment.GetRangeWeapon() != null)
            {
                transform.GetComponent<SpriteRenderer>().sprite = _withRangeWeaponSprite;

                Transform newHand = Instantiate(_handPrefab, transform).GetComponent<Transform>();
                Vector3 handPoint = newHand.GetChild(0).transform.position;
                _hand = newHand.gameObject;

                Transform weapon = Instantiate(equipment.GetRangeWeapon().PreFab, newHand).GetComponent<Transform>();
                weapon.position = handPoint;
                PlayerShoot.shootInput += weapon.GetComponent<RangeWeaponInterface>().Shoot;

                _rangeWeapon = weapon.gameObject;
            }
            else
            {
                transform.GetComponent<SpriteRenderer>().sprite = _noWeaponSprite;
            }
        }

        private void OnApplicationQuit()
        {
            inventory.Clear();
            equipment.Clear();
        }
    }
}