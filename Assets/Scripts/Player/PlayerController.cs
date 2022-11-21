using System;
using Item.Weapon;
using UnityEngine;
using UnityEngine.UIElements;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        private static PlayerController _instance;

        public static PlayerController Instance
        {
            get { return _instance; }
        }

        public Inventory inventory;
        public Equipment equipment;

        private CapsuleCollider2D _playerCollider;
        private Rigidbody2D _rigidbody;

        private const float MoveSpeed = 6f;
        private float _horizontalMovement;

        private static int _groundLayer;

        [Header("Friction materials")] 
        [SerializeField] private PhysicsMaterial2D zeroFriction;

        [SerializeField] private PhysicsMaterial2D maxFriction;

        private GameObject _rangeWeapon;
        private GameObject _meleeWeapon;

        [Header("Attack")] 
        [SerializeField] private Transform attackPoint;
        [SerializeField] private Transform grabPoint;

        [Header("Player sprites")]
        [SerializeField]
        private GameObject hand;

        [SerializeField] private Sprite noWeaponSprite;
        [SerializeField] private Sprite withRangeWeaponSprite;

        private void Start()
        {
            _instance = this;

            _rigidbody = gameObject.GetComponent<Rigidbody2D>();
            _playerCollider = gameObject.GetComponent<CapsuleCollider2D>();

            _groundLayer = LayerMask.GetMask("Ground");

            equipment.OnWeaponListChanged += Equipment_OnWeaponListChanged;
        }

        private void Update()
        {
            _horizontalMovement = Input.GetAxisRaw("Horizontal");
        }

        private void FixedUpdate()
        {
            if (!IsGrounded()) return;

            if (_horizontalMovement == 0f)
            {
                _rigidbody.sharedMaterial = maxFriction;
                return;
            }

            _rigidbody.sharedMaterial = zeroFriction;
            _rigidbody.velocity = new Vector2(_horizontalMovement * MoveSpeed, _rigidbody.velocity.y);
        }

        private bool IsGrounded()
        {
            var bounds = _playerCollider.bounds;
            var radius = bounds.size.x * .4f;
            var origin = (Vector2)bounds.center - new Vector2(0f, (bounds.size.y / 2) - radius);

            var hitPoint = Physics2D.CircleCast(origin, radius, Vector2.down, .2f, _groundLayer);

            Debug.DrawRay(hitPoint.point, hitPoint.normal);

            return hitPoint;
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

            if (hand.activeSelf)
            {
                hand.SetActive(false);
            }

            if (equipment.GetRangeWeapon() == null && equipment.GetMeleeWeapon() == null)
            {
                transform.GetComponent<SpriteRenderer>().sprite = noWeaponSprite;
                return;
            }

            if (equipment.GetRangeWeapon() != null)
            {
                transform.GetComponent<SpriteRenderer>().sprite = withRangeWeaponSprite;
                
                hand.SetActive(true);
                
                Transform rangeWeapon = Instantiate(equipment.GetRangeWeapon().PreFab, grabPoint.transform).GetComponent<Transform>();
                rangeWeapon.GetComponent<PolygonCollider2D>().enabled = false;
                Destroy(rangeWeapon.GetComponent<Rigidbody2D>());
                rangeWeapon.position = grabPoint.position;
                PlayerAttack.shootInput += rangeWeapon.GetComponent<RangeWeaponInterface>().Shoot;

                _rangeWeapon = rangeWeapon.gameObject;
            }

            if (equipment.GetMeleeWeapon() != null)
            {
                Transform meleeWeapon = Instantiate(equipment.GetMeleeWeapon().PreFab, attackPoint)
                    .GetComponent<Transform>();
                meleeWeapon.GetComponent<PolygonCollider2D>().enabled = false;
                Destroy(meleeWeapon.GetComponent<Rigidbody2D>());

                meleeWeapon.GetComponent<SpriteRenderer>().sprite = null;

                meleeWeapon.GetComponent<MeleeWeaponInterface>().attackPoint = attackPoint;

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