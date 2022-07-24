using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Client
{
    struct ViewComponent
    {
        public GameObject GameObject;
        public Rigidbody Rigidbody;
        public Animator Animator;
        public Transform Transform;
        public int BaseLayer;
        public HealthbarMB Healthbar;
        public LevelMB Level;
        public GameObject LevelPopup;
        public List<GameObject> DamagePopups;
        public Transform PointerTransform;
        public Transform WeaponHolder;
        public float MoveSpeed;
        public float RotateSpeed;
        public bool CanMining;
        public bool isFight;
        public bool isMining;
        public Outline Outline;
        public MeshFilter MeshFilter;
        public MeshFilter ModelMeshFilter;
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
        public ParticleSystem WayTrack;
    }
}