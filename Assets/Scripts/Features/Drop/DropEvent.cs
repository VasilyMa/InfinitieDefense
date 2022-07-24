using UnityEngine;

namespace Client
{
    struct DropEvent
    {
        public DropableItem.ItemType Item;
        public Vector3 Point;
    }
}