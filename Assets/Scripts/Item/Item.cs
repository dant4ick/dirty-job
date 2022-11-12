using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : ScriptableObject
{
    [SerializeField] private Sprite _sprite;
    [SerializeField] private string _name;
    //public GameObject prefab;

    public Sprite Sprite { get { return _sprite; } set { _sprite = value; } }
    public string Name { get { return _name; } set { _name = value; } }
}
