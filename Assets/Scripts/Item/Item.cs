using UnityEngine;

namespace Item
{
    public class Item : ScriptableObject
    {
        [SerializeField] private Sprite _sprite;
        [SerializeField] private string _name;
        [SerializeField] private GameObject _prefab;
        //public GameObject prefab;

        public Sprite Sprite { get { return _sprite; } set { _sprite = value; } }
        public string Name { get { return _name; } set { _name = value; } }
        public GameObject PreFab { get { return _prefab; } set { _prefab = value; } }
    }
}
