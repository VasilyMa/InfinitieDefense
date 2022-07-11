using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using System.Collections.Generic;

namespace Client
{
    sealed class CreateNextTowerSystem : IEcsRunSystem
    {
        readonly EcsSharedInject<GameState> _state = default;
        readonly EcsWorldInject _world = default;
        readonly EcsFilterInject<Inc<CreateNextTowerEvent>> _filter = default;
        readonly EcsPoolInject<ViewComponent> _viewPool = default;
        readonly EcsPoolInject<RadiusComponent> _radiusPool = default;
        readonly EcsPoolInject<TowerTag> _towerPool = default;
        readonly EcsPoolInject<Targetable> _targetablePool = default;
        readonly EcsPoolInject<DamageComponent> _damagePool = default;
        readonly EcsPoolInject<Cooldown> _cooldownPool = default;
        readonly EcsPoolInject<TargetWeightComponent> _targetWeightPool = default;
        readonly EcsPoolInject<HealthComponent> _healthWeightPool = default;
        readonly EcsPoolInject<CreateDefenderEvent> _defenderPool = default;
        readonly EcsPoolInject<DeadTag> _deadPool = default;
        readonly EcsFilterInject<Inc<CanvasUpgradeComponent, UpgradeCanvasEvent>> _canvasFilter = default;
        readonly EcsFilterInject<Inc<CircleComponent>> _circleFilter = default;
        readonly EcsPoolInject<CanvasUpgradeComponent> _upgradePool = default;

        private string Model;

        public void Run(EcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                ref var filterComp = ref _filter.Pools.Inc1.Get(entity);

                int towerIndex = filterComp.TowerIndex;
                ref var radiusComp = ref _radiusPool.Value.Get(entity);
                ref var viewComp = ref _viewPool.Value.Get(entity);
                if (viewComp.GameObject != null) GameObject.Destroy(viewComp.GameObject);

                if (!_targetWeightPool.Value.Has(entity))
                {
                    _targetWeightPool.Value.Add(entity);
                }

                ref var targetWeightComponent = ref _targetWeightPool.Value.Get(entity);

                viewComp.UpgradeParticleSystem.Play();
                
                if (towerIndex == 0)
                {
                    _state.Value.DefenseTowers[towerIndex] = _state.Value.TowerStorage.GetNextIDByID(_state.Value.DefenseTowers[towerIndex]);
                    viewComp.GameObject = GameObject.Instantiate(_state.Value.TowerStorage.GetTowerPrefabByID(_state.Value.DefenseTowers[towerIndex]), Vector3.zero, Quaternion.identity);
                    //radiusComp.RadiusTransform = GameObject.Instantiate(_state.Value.InterfaceStorage.RadiusPrefab, viewComp.GameObject.transform).GetComponent<Transform>();
                    radiusComp.Radius = _state.Value.TowerStorage.GetRadiusByID(_state.Value.DefenseTowers[towerIndex]);
                    foreach (var entityCircle in _circleFilter.Value)
                    {
                        _circleFilter.Pools.Inc1.Get(entityCircle).maxDistance = radiusComp.Radius;
                    }
                    _defenderPool.Value.Add(_world.Value.NewEntity());

                    viewComp.EcsInfoMB = viewComp.GameObject.GetComponent<EcsInfoMB>();
                    viewComp.EcsInfoMB.Init(_world);
                    viewComp.EcsInfoMB.SetEntity(entity);
                    viewComp.Healthbar = viewComp.GameObject.GetComponent<HealthbarMB>();
                    viewComp.Healthbar.SetMaxHealth(_state.Value.TowerStorage.GetHealthByID(_state.Value.DefenseTowers[towerIndex]));
                    viewComp.Healthbar.SetHealth(_state.Value.TowerStorage.GetHealthByID(_state.Value.DefenseTowers[towerIndex]));
                    viewComp.Healthbar.Init(systems.GetWorld(), systems.GetShared<GameState>());

                    targetWeightComponent.Value = 0;
                }
                else
                {
                    ref var towerComp = ref _towerPool.Value.Get(entity);
                    if (!_targetablePool.Value.Has(entity))
                    {
                        _targetablePool.Value.Add(entity);
                    }

                    if (!_damagePool.Value.Has(entity))
                    {
                        _damagePool.Value.Add(entity);
                    }

                    if (!_cooldownPool.Value.Has(entity))
                    {
                        _cooldownPool.Value.Add(entity);
                    }

                    ref var targetableComponent = ref _targetablePool.Value.Get(entity);
                    ref var damageComponent = ref _damagePool.Value.Get(entity);
                    ref var cooldownComponent = ref _cooldownPool.Value.Get(entity);
                    ref var healthComponent = ref _healthWeightPool.Value.Get(entity);

                    _state.Value.DefenseTowers[towerIndex] = _state.Value.DefenseTowerStorage.GetNextIDByID(_state.Value.DefenseTowers[towerIndex]);

                    viewComp.GameObject = GameObject.Instantiate(_state.Value.DefenseTowerStorage.GetTowerPrefabByID(_state.Value.DefenseTowers[towerIndex]), towerComp.Position, Quaternion.identity);
                    //EEEEEXPERIMEEENTS
                    //
                    //if (!viewComp.GameObject)
                    //{
                    //    viewComp.GameObject = GameObject.Instantiate(_state.Value.DefenseTowerStorage.GetTowerPrefabByID(_state.Value.DefenseTowers[towerIndex]), towerComp.Position, Quaternion.identity);
                    //    viewComp.ModelMeshFilter = viewComp.GameObject.transform.GetChild(1).GetComponent<MeshFilter>();
                    //}
                    //else
                    //{
                    //    viewComp.ModelMeshFilter.mesh = _state.Value.DefenseTowerStorage.GetTowerMeshByID(_state.Value.DefenseTowers[towerIndex]);
                    //}
                    //
                    //End of the Experiment

                    radiusComp.Radius = _state.Value.DefenseTowerStorage.GetRadiusByID(_state.Value.DefenseTowers[towerIndex]);
                    //radiusComp.RadiusTransform = GameObject.Instantiate(_state.Value.InterfaceStorage.RadiusPrefab, viewComp.GameObject.transform).GetComponent<Transform>();
                    viewComp.Healthbar = viewComp.GameObject.GetComponent<HealthbarMB>();
                    viewComp.Healthbar.SetMaxHealth(_state.Value.DefenseTowerStorage.GetHealthByID(_state.Value.DefenseTowers[towerIndex]));
                    viewComp.Healthbar.SetHealth(_state.Value.DefenseTowerStorage.GetHealthByID(_state.Value.DefenseTowers[towerIndex]));
                    viewComp.Healthbar.Init(systems.GetWorld(), systems.GetShared<GameState>());

                    damageComponent.Value = _state.Value.DefenseTowerStorage.GetDamageByID(_state.Value.DefenseTowers[towerIndex]);

                    viewComp.EcsInfoMB = viewComp.GameObject.GetComponent<EcsInfoMB>();
                    viewComp.EcsInfoMB.Init(_world);
                    viewComp.EcsInfoMB.SetEntity(entity);

                    viewComp.TowerAttackMB = viewComp.GameObject.GetComponentInChildren<TowerAttackMB>();
                    viewComp.DamageZone = viewComp.TowerAttackMB.GetComponent<SphereCollider>();
                    viewComp.DamageZone.radius = radiusComp.Radius - 1;

                    Transform[] allChildren = viewComp.GameObject.GetComponentsInChildren<Transform>();
                    foreach (var child in allChildren)
                    {
                        if (child.CompareTag("Weapon"))
                        {
                            viewComp.TowerWeapon = child.gameObject;

                            Transform[] allWeaponChildren = viewComp.TowerWeapon.GetComponentsInChildren<Transform>();

                            foreach (var weaponChild in allWeaponChildren)
                            {
                                if (weaponChild.CompareTag("Fire Point"))
                                {
                                    viewComp.TowerFirePoint = weaponChild.gameObject;
                                }
                            }
                        }

                        if (child.CompareTag("DetectedZone"))
                        {
                            viewComp.DetectedZone = child.GetComponent<SphereCollider>();
                            viewComp.DetectedZone.radius = radiusComp.Radius;
                        }
                    }

                    targetableComponent.TargetEntity = -1;
                    targetableComponent.TargetObject = null;
                    targetableComponent.AllEntityInDetectedZone = new List<int>();
                    targetableComponent.AllEntityInDamageZone = new List<int>();

                    targetWeightComponent.Value = 10;

                    cooldownComponent.MaxValue = _state.Value.DefenseTowerStorage.GetCooldownByID(_state.Value.DefenseTowers[towerIndex]);
                    cooldownComponent.CurrentValue = 0;

                    healthComponent.MaxValue = _state.Value.DefenseTowerStorage.GetHealthByID(_state.Value.DefenseTowers[towerIndex]);
                    healthComponent.CurrentValue = _state.Value.DefenseTowerStorage.GetHealthByID(_state.Value.DefenseTowers[towerIndex]);

                    if (_deadPool.Value.Has(entity)) _deadPool.Value.Del(entity);

                    towerComp.TowerID = _state.Value.DefenseTowers[towerIndex];
                }
                

                //radiusComp.Radius = _state.Value.TowerStorage.GetRadiusByID(_state.Value.CurrentTowerID);
                //radiusComp.RadiusTransform.localScale = new Vector3(radiusComp.Radius * 2, radiusComp.Radius * 2, 1);

                // —оздаем отображение дл€ зоны обнаружени€
                Mesh mesh = new Mesh();
                viewComp.MeshFilter = viewComp.GameObject.GetComponent<MeshFilter>();
                viewComp.MeshFilter.mesh = mesh;

                viewComp.LineRenderer = viewComp.GameObject.GetComponent<LineRenderer>();

                float fov = 360f;
                Vector3 origin = Vector3.zero;
                int triangelesCount = 45;
                float angle = 0f;
                float angleIncrease = fov / triangelesCount;
                float viewDistence = radiusComp.Radius;

                Vector3[] vertices = new Vector3[triangelesCount + 1 + 1];
                //Vector2[] uv = new Vector2[vertices.Length];
                int[] trianglesVertices = new int[triangelesCount * 3];
                Vector3[] circleVerticesv = new Vector3[triangelesCount];
                if (viewComp.LineRenderer) viewComp.LineRenderer.positionCount = triangelesCount;

                vertices[0] = origin;

                int vertexIndex = 1;
                int triangleIndex = 0;
                int circleIndex = 0;
                for (int i = 0; i <= triangelesCount; i++)
                {
                    float angleRad = angle * (Mathf.PI / 180f);
                    Vector3 VectorFromAngle = new Vector3(Mathf.Cos(angleRad), 0, Mathf.Sin(angleRad));

                    Vector3 vertex = origin + VectorFromAngle * viewDistence;
                    vertices[vertexIndex] = vertex;

                    if (i > 0 && i <= circleVerticesv.Length)
                    {
                        circleVerticesv[circleIndex] = vertices[vertexIndex];
                        circleIndex++;
                    }

                    if (i > 0)
                    {
                        trianglesVertices[triangleIndex + 0] = 0;
                        trianglesVertices[triangleIndex + 1] = vertexIndex - 1;
                        trianglesVertices[triangleIndex + 2] = vertexIndex;

                        triangleIndex += 3;
                    }

                    vertexIndex++;
                    angle -= angleIncrease;
                }

                mesh.vertices = vertices;
                //mesh.uv = uv;
                mesh.triangles = trianglesVertices;

                if (viewComp.LineRenderer)
                {
                    viewComp.LineRenderer.SetPositions(circleVerticesv);
                    viewComp.LineRenderer.loop = true;
                }
                // «акончили создание зоны обнаружени€

                _filter.Pools.Inc1.Del(entity);

            }
        }
    }
}