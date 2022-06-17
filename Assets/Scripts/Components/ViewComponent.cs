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
        public Outline Outline;
    }
}