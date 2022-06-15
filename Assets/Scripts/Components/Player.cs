using UnityEngine;
namespace Client {
    struct Player {
        public Transform Transform;
        public Vector3 direction;
        public Rigidbody rigidbody;
        public Animator animator;
        public float MoveSpeed;
        public float RotateSpeed;
        public Transform ResHolderTransform;
    }
}