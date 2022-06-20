using UnityEngine;
namespace Client {
    struct Player {
        public Transform Transform;
        public PlayerMB playerMB;
        public AttackMonoBehaviour AttackMonoBehaviour;
        public Vector3 direction;
        public Rigidbody rigidbody;
        public Animator animator;
        public float MoveSpeed;
        public float RotateSpeed;
        public float damage;
        public float health;
        public Transform ResHolderTransform;
    }
}