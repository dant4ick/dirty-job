using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Item
{
    public class ItemInterface : MonoBehaviour
    {
        [SerializeField] private Item _item;

        public Item Item { get { return _item; } }
    }
}
