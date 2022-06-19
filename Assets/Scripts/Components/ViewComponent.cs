using UnityEngine;

namespace Client
{
    struct ViewComponent
    {
        public GameObject GameObject;
        public Rigidbody Rigidbody;
        public Animator Animator;
        public Transform Transform;
        public Outline Outline;

        public AttackMonoBehaviour AttackMonoBehaviour;
        public EcsInfoMB EcsInfoMB;
    }
}