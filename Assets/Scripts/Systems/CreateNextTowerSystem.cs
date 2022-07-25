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
        readonly EcsFilterInject<Inc<CreateNextTowerEvent>> _CreateNextTowerFilter = default;
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
        readonly EcsFilterInject<Inc<CircleComponent>> _circleFilter = default;
        readonly EcsPoolInject<NewTowerCircleEvent> _circlePool = default;
        readonly EcsPoolInject<LevelUpEvent> _levelUpPool = default;
        readonly EcsFilterInject<Inc<UpgradeTimerEvent>> _timerPool = default;
        readonly EcsPoolInject<UpgradeComponent> _upgradePool = default;

        private string Model;

        public void Run(EcsSystems systems)
        {
            foreach (var eventEntity in _CreateNextTowerFilter.Value)
            {
                ref var filterComp = ref _CreateNextTowerFilter.Pools.Inc1.Get(eventEntity);

                int towerIndex = filterComp.TowerIndex;
                ref var radiusComp = ref _radiusPool.Value.Get(eventEntity);
                ref var viewComp = ref _viewPool.Value.Get(eventEntity);

                if (!_targetWeightPool.Value.Has(eventEntity))
                {
                    _targetWeightPool.Value.Add(eventEntity);
                }

                ref var targetWeightComponent = ref _targetWeightPool.Value.Get(eventEntity);

                viewComp.UpgradeParticleSystem.Play();
                
                if (towerIndex == 0) // main tower
                {
                    _state.Value.DefenseTowers[towerIndex] = _state.Value.TowerStorage.GetNextIDByID(_state.Value.DefenseTowers[towerIndex]);

                    ref var towerComp = ref _towerPool.Value.Get(eventEntity);
                    towerComp.Level++;

                    if (!viewComp.GameObject)
                    {
                        viewComp.GameObject = GameObject.Instantiate(_state.Value.TowerStorage.GetTowerPrefabByID(_state.Value.DefenseTowers[towerIndex]), Vector3.zero, Quaternion.identity);
                        viewComp.ModelMeshFilter = viewComp.GameObject.transform.GetChild(2).GetComponent<MeshFilter>();
                    }
                    else
                    {
                        viewComp.ModelMeshFilter.mesh = _state.Value.TowerStorage.GetTowerMeshByID(_state.Value.DefenseTowers[towerIndex]);
                    }

                    //radiusComp.RadiusTransform = GameObject.Instantiate(_state.Value.InterfaceStorage.RadiusPrefab, viewComp.GameObject.transform).GetComponent<Transform>();
                    radiusComp.Radius = _state.Value.TowerStorage.GetRadiusByID(_state.Value.DefenseTowers[towerIndex]);
                    foreach (var entityCircle in _circleFilter.Value)
                    {
                        _circleFilter.Pools.Inc1.Get(entityCircle).maxDistance = radiusComp.Radius;
                    }
                    _defenderPool.Value.Add(_world.Value.NewEntity());

                    viewComp.EcsInfoMB = viewComp.GameObject.GetComponent<EcsInfoMB>();
                    viewComp.EcsInfoMB.Init(_world);
                    viewComp.EcsInfoMB.SetEntity(eventEntity);
                    viewComp.Healthbar = viewComp.GameObject.GetComponent<HealthbarMB>();
                    viewComp.Healthbar.SetMaxHealth(_state.Value.TowerStorage.GetHealthByID(_state.Value.DefenseTowers[towerIndex]));
                    viewComp.Healthbar.SetHealth(_state.Value.TowerStorage.GetHealthByID(_state.Value.DefenseTowers[towerIndex]));
                    viewComp.Healthbar.Init(systems.GetWorld(), systems.GetShared<GameState>());
                    //viewComp.Level = viewComp.GameObject.GetComponent<LevelMB>();
                    //viewComp.Level.UpdateLevel(_state.Value.TowerStorage.GetLevelByID(_state.Value.DefenseTowers[towerIndex]));
                    //viewComp.Level.Init(systems.GetWorld(), systems.GetShared<GameState>());

                    viewComp.LevelPopup = viewComp.GameObject.transform.GetChild(0).transform.GetChild(2).transform.gameObject;
                    viewComp.LevelPopup.GetComponent<LevelPopupMB>().UpdateLevel(_state.Value.TowerStorage.GetLevelByID(_state.Value.DefenseTowers[towerIndex]));
                    viewComp.LevelPopup.GetComponent<LevelPopupMB>().Init(systems.GetWorld(), systems.GetShared<GameState>());
                    ref var levelPop = ref _levelUpPool.Value.Add(_world.Value.NewEntity());
                    levelPop.LevelPopUp = viewComp.LevelPopup;
                    levelPop.LevelPopUp.transform.position = new Vector3(viewComp.GameObject.transform.position.x, viewComp.GameObject.transform.position.y + 2f, viewComp.GameObject.transform.position.z);
                    levelPop.LevelPopUp.GetComponent<LevelPopupMB>().UpdateLevel(_state.Value.TowerStorage.GetLevelByID(_state.Value.DefenseTowers[towerIndex]));
                    levelPop.Text = levelPop.LevelPopUp.GetComponent<LevelPopupMB>().GetText();
                    levelPop.target = new Vector3(viewComp.GameObject.transform.position.x, viewComp.GameObject.transform.position.y + 10f, viewComp.GameObject.transform.position.z);
                    levelPop.TimeOut = 2f;
                    levelPop.LevelPopUp.SetActive(true); 
                    
                    
                    /*viewComp.ResourcesTimer = viewComp.GameObject.transform.GetChild(0).transform.GetChild(3).transform.gameObject;
                    viewComp.ResourcesTimer.GetComponent<TimerResourcesMB>().ResourcesDrop(0);
                    viewComp.ResourcesTimer.GetComponent<TimerResourcesMB>().Init(systems.GetWorld(), systems.GetShared<GameState>());
                    viewComp.ResourcesTimer.SetActive(true);
                    */
                    viewComp.DamagePopups = new List<GameObject>();
                    for (int y = 0; y < viewComp.Transform.GetChild(1).transform.childCount; y++)
                    {
                        var popup = viewComp.Transform.GetChild(1).transform.GetChild(y).gameObject;
                        viewComp.DamagePopups.Add(popup);
                        viewComp.DamagePopups[y].SetActive(false);
                    }

                    targetWeightComponent.Value = 0;




                    //todo
                    if (_state.Value.CoinCount > 0)
                    {
                        foreach (var item in _timerPool.Value)
                        {
                            ref var timerComp = ref _timerPool.Pools.Inc1.Get(item);
                            timerComp.TimeToUpgrade = 0f;
                        }
                    }
                    ref var upgradeComp = ref _upgradePool.Value.Get(_state.Value.EntityPlayer);
                    upgradeComp.DelayTime = 0f;
                    upgradeComp.Time = 0f;

                    _circlePool.Value.Add(_world.Value.NewEntity());
                }
                else // defence towers
                {
                    ref var towerComp = ref _towerPool.Value.Get(eventEntity);
                    towerComp.Level++;

                    if (!_targetablePool.Value.Has(eventEntity))
                    {
                        _targetablePool.Value.Add(eventEntity);
                    }

                    if (!_damagePool.Value.Has(eventEntity))
                    {
                        _damagePool.Value.Add(eventEntity);
                    }

                    if (!_cooldownPool.Value.Has(eventEntity))
                    {
                        _cooldownPool.Value.Add(eventEntity);
                    }

                    ref var targetableComponent = ref _targetablePool.Value.Get(eventEntity);
                    ref var damageComponent = ref _damagePool.Value.Get(eventEntity);
                    ref var cooldownComponent = ref _cooldownPool.Value.Get(eventEntity);
                    ref var healthComponent = ref _healthWeightPool.Value.Get(eventEntity);

                    if (filterComp.Change)
                    {
                        _state.Value.DefenseTowers[towerIndex] = _state.Value.DefenseTowerStorage.GetNextIDByID(_state.Value.DefenseTowers[towerIndex]);
                    }
                    
                    if (!viewComp.GameObject)
                    {
                        viewComp.GameObject = GameObject.Instantiate(_state.Value.DefenseTowerStorage.GetTowerPrefabByID(_state.Value.DefenseTowers[towerIndex]), towerComp.Position, Quaternion.identity);
                        viewComp.ModelMeshFilter = viewComp.GameObject.transform.GetChild(1).GetComponent<MeshFilter>();
                        viewComp.Transform = viewComp.GameObject.transform;
                    }
                    else
                    {
                        viewComp.ModelMeshFilter.mesh = _state.Value.DefenseTowerStorage.GetTowerMeshByID(_state.Value.DefenseTowers[towerIndex]);
                    }

                    radiusComp.Radius = _state.Value.DefenseTowerStorage.GetRadiusByID(_state.Value.DefenseTowers[towerIndex]);
                    //radiusComp.RadiusTransform = GameObject.Instantiate(_state.Value.InterfaceStorage.RadiusPrefab, viewComp.GameObject.transform).GetComponent<Transform>();
                    viewComp.Healthbar = viewComp.GameObject.GetComponent<HealthbarMB>();
                    viewComp.Healthbar.SetMaxHealth(_state.Value.DefenseTowerStorage.GetHealthByID(_state.Value.DefenseTowers[towerIndex]));
                    viewComp.Healthbar.SetHealth(_state.Value.DefenseTowerStorage.GetHealthByID(_state.Value.DefenseTowers[towerIndex]));
                    viewComp.Healthbar.Init(systems.GetWorld(), systems.GetShared<GameState>());

                    //viewComp.Level = viewComp.GameObject.GetComponent<LevelMB>();
                    //viewComp.Level.UpdateLevel(_state.Value.DefenseTowerStorage.GetLevelByID(_state.Value.DefenseTowers[towerIndex]));
                    //viewComp.Level.Init(systems.GetWorld(), systems.GetShared<GameState>());

                    viewComp.LevelPopup = viewComp.GameObject.transform.GetChild(0).transform.GetChild(2).transform.gameObject;
                    viewComp.LevelPopup.GetComponent<LevelPopupMB>().UpdateLevel(_state.Value.TowerStorage.GetLevelByID(_state.Value.DefenseTowers[towerIndex]));
                    viewComp.LevelPopup.GetComponent<LevelPopupMB>().Init(systems.GetWorld(), systems.GetShared<GameState>());
                    ref var levelPop = ref _levelUpPool.Value.Add(_world.Value.NewEntity());
                    levelPop.LevelPopUp = viewComp.LevelPopup;
                    levelPop.LevelPopUp.transform.position = new Vector3(viewComp.GameObject.transform.position.x, viewComp.GameObject.transform.position.y + 2f, viewComp.GameObject.transform.position.z);
                    levelPop.LevelPopUp.GetComponent<LevelPopupMB>().UpdateLevel(_state.Value.TowerStorage.GetLevelByID(_state.Value.DefenseTowers[towerIndex]));
                    levelPop.Text = levelPop.LevelPopUp.GetComponent<LevelPopupMB>().GetText();
                    levelPop.target = new Vector3(viewComp.GameObject.transform.position.x, viewComp.GameObject.transform.position.y + 10f, viewComp.GameObject.transform.position.z);
                    levelPop.TimeOut = 2f;
                    levelPop.LevelPopUp.SetActive(true);

                    /*viewComp.ResourcesTimer = viewComp.GameObject.transform.GetChild(0).transform.GetChild(3).transform.gameObject;
                    viewComp.ResourcesTimer.GetComponent<TimerResourcesMB>().ResourcesDrop(0);
                    viewComp.ResourcesTimer.GetComponent<TimerResourcesMB>().Init(systems.GetWorld(), systems.GetShared<GameState>());
                    viewComp.ResourcesTimer.SetActive(true);
                    */

                    viewComp.DamagePopups = new List<GameObject>();
                    for (int y = 0; y < viewComp.GameObject.transform.GetChild(6).transform.childCount; y++)
                    {
                        var popup = viewComp.GameObject.transform.GetChild(6).transform.GetChild(y).gameObject;
                        viewComp.DamagePopups.Add(popup);
                        viewComp.DamagePopups[y].SetActive(false);
                    }

                    damageComponent.Value = _state.Value.DefenseTowerStorage.GetDamageByID(_state.Value.DefenseTowers[towerIndex]);
                    if (_state.Value.RockCount > 0)
                    {
                        foreach (var item in _timerPool.Value)
                        {
                            ref var timerComp = ref _timerPool.Pools.Inc1.Get(item);
                            timerComp.Entity = _state.Value.TowersEntity[towerIndex];
                            timerComp.TimeToUpgrade = 0f;
                        }
                        ref var upgradeComp = ref _upgradePool.Value.Get(_state.Value.EntityPlayer);
                        upgradeComp.DelayTime = 0f;
                        upgradeComp.Time = 0f;
                    }
                    

                    viewComp.EcsInfoMB = viewComp.GameObject.GetComponent<EcsInfoMB>();
                    viewComp.EcsInfoMB.Init(_world);
                    viewComp.EcsInfoMB.SetEntity(eventEntity);

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

                    if (_deadPool.Value.Has(eventEntity)) _deadPool.Value.Del(eventEntity);

                    towerComp.TowerID = _state.Value.DefenseTowers[towerIndex];
                }
                

                //radiusComp.Radius = _state.Value.TowerStorage.GetRadiusByID(_state.Value.CurrentTowerID);
                //radiusComp.RadiusTransform.localScale = new Vector3(radiusComp.Radius * 2, radiusComp.Radius * 2, 1);

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

                _CreateNextTowerFilter.Pools.Inc1.Del(eventEntity);

            }
        }
    }
}