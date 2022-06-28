using UnityEngine;
using UnityEngine.AI;

namespace Client
{
    struct ViewComponent
    {
        public GameObject GameObject;
        public Rigidbody Rigidbody;
        public Animator Animator;
        public HealthbarMB Healthbar;
        public Transform Transform;
        public Transform PointerTransform;
        public float MoveSpeed;
        public float RotateSpeed;
        public Outline Outline;
        public MeshFilter MeshFilter;

        public MeleeAttackMB AttackMB;
        public TowerAttackMB TowerAttackMB;
        public EcsInfoMB EcsInfoMB;
        public NavMeshAgent NavMeshAgent;
        public SkinnedMeshRenderer SkinnedMeshRenderer;


        public GameObject TowerWeapon;
        public GameObject TowerFirePoint;
        public SphereCollider DetectedZone;
        public SphereCollider DamageZone;
        public ParticleSystem UpgradeParticleSystem;
        public ParticleSystem HitParticleSystem;
        public ParticleSystem DropItemParticleSystem;
    }
}