using UnityEngine;

namespace Client
{
    struct ViewComponent
    {
        public GameObject GameObject;
        public Rigidbody Rigidbody;
        public Animator Animator;
        public AttackMonoBehaviour AttackMonoBehaviour;
        public Transform Transform;
        public float MoveSpeed;
        public float RotateSpeed;
        public Outline Outline;
    }
}