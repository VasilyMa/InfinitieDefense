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
        public LevelMB Level;
        public Transform Transform;
        public Transform PointerTransform;
        public Transform WeaponHolder;
        public float MoveSpeed;
        public float RotateSpeed;
        public bool CanMining;
        public bool isFight;
        public bool isMining;
        public Outline Outline;
        public MeshFilter MeshFilter;
        public LineRenderer LineRenderer;

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
        public ParticleSystem Regeneration;
    }
}