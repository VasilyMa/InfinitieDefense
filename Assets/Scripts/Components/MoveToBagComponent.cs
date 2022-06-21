using UnityEngine;
namespace Client {
    struct MoveToBagComponent {
        public Transform Transform;
        public Vector3 StartPosition;
        public Vector3 TargetPosition;
        public bool Coin;
        public float _t;
    }
}