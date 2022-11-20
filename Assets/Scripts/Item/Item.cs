using UnityEngine;

namespace Item
{
    public class Item : ScriptableObject
    {
        [SerializeField] private Sprite _spriteInInventory;
        [SerializeField] private string _name;
        [SerializeField] private GameObject _prefab;
        //public GameObject prefab;

        public Sprite SpriteInInventory { get { return _spriteInInventory; } set { _spriteInInventory = value; } }
        public string Name { get { return _name; } set { _name = value; } }
        public GameObject PreFab { get { return _prefab; } set { _prefab = value; } }
    }
}
