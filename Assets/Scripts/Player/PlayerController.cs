using Item.Weapon;
using UnityEngine;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        private static PlayerController _instance;
        public static PlayerController Instance { get { return _instance; } }

        public Inventory inventory;
        public Equipment equipment;

        private CircleCollider2D _feetCollider;
        private BoxCollider2D _headCollider;
        private Rigidbody2D _rigidbody;

        private const float MoveSpeed = 6f;
        private float _horizontalMovement;

        private static int _groundLayer;

        private GameObject _rangeWeapon;
        private GameObject _meleeWeapon;

        [SerializeField] private Transform _attackPoint;

        [SerializeField] private GameObject _handPrefab;
        [SerializeField] private Sprite _noWeaponSprite;
        [SerializeField] private Sprite _withRangeWeaponSprite;
        private GameObject _hand = null;

        private void Start()
        {
            _instance = this;

            _rigidbody = gameObject.GetComponent<Rigidbody2D>();
            _feetCollider = gameObject.GetComponent<CircleCollider2D>();
            _headCollider = gameObject.GetComponent<BoxCollider2D>();
            
            _groundLayer = LayerMask.GetMask("Ground");

            //UpdateEquipment();

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

        private void Equipment_OnWeaponListChanged(object sender, System.EventArgs e)
        {
            UpdateEquipment();
        }

        public void UpdateEquipment()
        {
            if (_meleeWeapon != null)
            {
                PlayerAttack.attackInput -= _meleeWeapon.GetComponent<MeleeWeaponInterface>().Attack;
                Destroy(_meleeWeapon);
            }
            if (_rangeWeapon != null)
            {
                PlayerAttack.shootInput -= _rangeWeapon.GetComponent<RangeWeaponInterface>().Shoot;
                Destroy(_rangeWeapon);
            }
            if (_hand != null)
            {
                Destroy(_hand);
            }

            if (equipment.GetRangeWeapon() == null && equipment.GetMeleeWeapon() == null)
            {
                transform.GetComponent<SpriteRenderer>().sprite = _noWeaponSprite;
                return;
            }

            if (equipment.GetRangeWeapon() != null)
            {
                transform.GetComponent<SpriteRenderer>().sprite = _withRangeWeaponSprite;

                Transform newHand = Instantiate(_handPrefab, transform).GetComponent<Transform>();
                newHand.GetComponent<Pivot>().FixedUpdate();
                Vector3 handPoint = newHand.GetChild(0).transform.position;
                _hand = newHand.gameObject;

                Transform rangeWeapon = Instantiate(equipment.GetRangeWeapon().PreFab, newHand).GetComponent<Transform>();
                rangeWeapon.GetComponent<BoxCollider2D>().enabled = false;
                Destroy(rangeWeapon.GetComponent<Rigidbody2D>());
                rangeWeapon.position = handPoint;
                PlayerAttack.shootInput += rangeWeapon.GetComponent<RangeWeaponInterface>().Shoot;

                _rangeWeapon = rangeWeapon.gameObject;
            }

            if (equipment.GetMeleeWeapon() != null)
            {
                Transform meleeWeapon = Instantiate(equipment.GetMeleeWeapon().PreFab, _attackPoint).GetComponent<Transform>();
                meleeWeapon.GetComponent<BoxCollider2D>().enabled = false;
                Destroy(meleeWeapon.GetComponent<Rigidbody2D>());

                meleeWeapon.GetComponent<SpriteRenderer>().sprite = null;

                meleeWeapon.GetComponent<MeleeWeaponInterface>().attackPoint = _attackPoint;

                PlayerAttack.attackInput += meleeWeapon.GetComponent<MeleeWeaponInterface>().Attack;

                _meleeWeapon = meleeWeapon.gameObject;
            }            
        }

        public void DropWeapon(Item.Item item, int positionInInventory, Transform objectInInventory)
        {
            Instantiate(item.PreFab, transform.position, transform.rotation);
            inventory.SetItemToCell(null, positionInInventory);

            Destroy(objectInInventory.gameObject);
        }

        private void OnApplicationQuit()
        {
            inventory.Clear();
            equipment.Clear();
        }
    }
}