using UnityEngine;
namespace Client {
    struct Player {
        public Transform Transform;
        public Vector3 direction;
        public CharacterController controller;
        public Rigidbody rigidbody;
        public float MoveSpeed;
        public float RotateSpeed;
    }
}