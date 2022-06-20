using UnityEngine;

namespace Client
{
    struct ViewComponent
    {
        public GameObject GameObject;
        public Rigidbody Rigidbody;
        public Animator Animator;
        public HealthbarMB Healthbar;
        public Transform Transform;
        public float MoveSpeed;
        public float RotateSpeed;
        public Outline Outline;

        public AttackMB AttackMB;
        public PlayerAttackMB PlayerAttackMB;
        public EcsInfoMB EcsInfoMB;
        public SkinnedMeshRenderer SkinnedMeshRenderer;
    }
}