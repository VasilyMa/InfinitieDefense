using UnityEngine;
namespace Client {
    struct OreMoveEvent {
        public float Speed;
        public GameObject stone;
        public Vector3 StartPosition;
        public Vector3 TargetPosition;
        public float outTime;
    }
}