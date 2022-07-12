using Leopotam.EcsLite;
using UnityEngine;
using Leopotam.EcsLite.Di;
using System.Collections.Generic;
using UnityEngine.AI;

namespace Client {
    sealed class StartCurrentWave : IEcsRunSystem {
        readonly EcsSharedInject<GameState> _state = default;
        readonly EcsFilterInject<Inc<StartWaveEvent>> _filter = default;
        readonly EcsWorldInject _world = default;
        readonly EcsPoolInject<ViewComponent> _viewPool = default;
        readonly EcsPoolInject<EnemyTag> _enemyPool = default;
        readonly EcsPoolInject<UnitTag> _unitPool = default;
        readonly EcsPoolInject<InactiveTag> _inactivePool = default;
        readonly EcsPoolInject<Targetable> _targetablePool = default;
        readonly EcsPoolInject<Movable> _movablePool = default;
        readonly EcsPoolInject<ShipComponent> _shipPool = default;
        readonly EcsPoolInject<HealthComponent> _healthPool = default;
        readonly EcsPoolInject<DamageComponent> _damagePool = default;
        readonly EcsPoolInject<TargetWeightComponent> _targetWeightPool = default;
        private float _angle = 0f;
        private float _shipAngle = 0f;
        private int _encounter = 0;
        private int _enemyCountInEncounter = 0;
        public void Run (EcsSystems systems) {
            foreach(var entity in _filter.Value)
            {
                ref var filterComp = ref _filter.Pools.Inc1.Get(entity);

                for (int wave = 0; wave < _state.Value.WaveStorage.Waves.Count; wave++)
                {
                    int[] enemyInShip = _state.Value.WaveStorage.Waves[wave].MeleeEnemyInShip;
                    int[] rangeEnemyInShip = _state.Value.WaveStorage.Waves[wave].RangeEnemyInShip;
                    int[] encounters = _state.Value.WaveStorage.Waves[wave].Encounters;

                    for (int i = 0; i < enemyInShip.Length; i++)
                    {
                        _angle += Random.Range(30,360 / enemyInShip.Length + 20);

                        var x = Mathf.Cos(_angle * Mathf.Deg2Rad) * 155;
                        var z = Mathf.Sin(_angle * Mathf.Deg2Rad) * 155;
                        var ship = GameObject.Instantiate(_state.Value.EnemyConfig.ShipPrefab, new Vector3(x, 0, z), Quaternion.identity);
                        

                        var shipEntity = _world.Value.NewEntity();

                        _world.Value.GetPool<EnemyTag>().Add(shipEntity);

                        _world.Value.GetPool<ShipTag>().Add(shipEntity);

                        ref var targetableComponent = ref _world.Value.GetPool<Targetable>().Add(shipEntity);

                        ref var _viewMainTowerComponent = ref _viewPool.Value.Get(_state.Value.TowersEntity[0]);

                        targetableComponent.TargetEntity = _state.Value.TowersEntity[0];
                        targetableComponent.TargetObject = _viewMainTowerComponent.GameObject;

                        ref var movableComponent = ref _world.Value.GetPool<Movable>().Add(shipEntity);
                        movableComponent.Speed = 10f;

                        ref var shipComponent = ref _shipPool.Value.Add(shipEntity);
                        shipComponent.Encounter = _encounter;
                        shipComponent.Wave = wave;
                        shipComponent.EnemyUnitsEntitys = new List<int>();

                        ref var viewComponent = ref _viewPool.Value.Add(shipEntity);
                        viewComponent.GameObject = ship.gameObject;
                        viewComponent.Rigidbody = ship.GetComponent<Rigidbody>();
                        viewComponent.EcsInfoMB = ship.GetComponent<EcsInfoMB>();
                        viewComponent.EcsInfoMB.Init(_world);
                        viewComponent.EcsInfoMB.SetEntity(shipEntity);

                        if (!ship.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.transform) Debug.LogError("�� ������� ��� CanvasPointer'a");
                        viewComponent.PointerTransform = ship.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.transform;

                        _inactivePool.Value.Add(shipEntity);

                        string[] enemies = new string[enemyInShip[i] + rangeEnemyInShip[i]];

                        for (int n = 0; n < enemies.Length; n++)
                        {
                            if (n < enemyInShip[i])
                            {
                                enemies[n] = "Melee";
                            }
                            else
                            {
                                enemies[n] = "Range";
                            }
                        }

                        for (int j = 0; j < enemies.Length; j++)
                        {
                            var ex = Mathf.Cos(_shipAngle * Mathf.Deg2Rad) * 2;
                            var ez = Mathf.Sin(_shipAngle * Mathf.Deg2Rad) * 2;
                            _shipAngle += 360 / enemyInShip[i];

                            GameObject enemy = null;

                            var enemyEntity = _world.Value.NewEntity();

                            _enemyPool.Value.Add(enemyEntity);
                            _unitPool.Value.Add(enemyEntity);
                            _inactivePool.Value.Add(enemyEntity);

                            ref var enemyTargetableComponent = ref _targetablePool.Value.Add(enemyEntity);
                            ref var enemyMovableComponent = ref _movablePool.Value.Add(enemyEntity);
                            ref var enemyViewComponent = ref _viewPool.Value.Add(enemyEntity);
                            ref var enemyShipComponent = ref _shipPool.Value.Add(enemyEntity);
                            ref var healthComponent = ref _healthPool.Value.Add(enemyEntity);
                            ref var damageComponent = ref _damagePool.Value.Add(enemyEntity);
                            ref var targetWeightComponent = ref _targetWeightPool.Value.Add(enemyEntity);

                            switch (enemies[j])
                            {
                                case "Melee":
                                    {
                                        enemy = GameObject.Instantiate(_state.Value.EnemyConfig.EnemyPrefab, ship.transform);
                                        enemyViewComponent.GameObject = enemy;
                                        enemyViewComponent.Animator = enemy.GetComponent<Animator>();
                                        enemyViewComponent.Animator.SetBool("Melee", true);

                                        damageComponent.Value = 10f;

                                        healthComponent.MaxValue = 20;
                                        break;
                                    }
                                case "Range":
                                    {
                                        enemy = GameObject.Instantiate(_state.Value.EnemyConfig.RangeEnemyPrefab, ship.transform);
                                        enemyViewComponent.GameObject = enemy;
                                        enemyViewComponent.Animator = enemy.GetComponent<Animator>();
                                        enemyViewComponent.Animator.SetBool("Range", true);

                                        damageComponent.Value = 7f;

                                        healthComponent.MaxValue = 10;
                                        break;
                                    }
                            }

                            enemy.transform.localPosition = new Vector3(ex, 0, ez);

                            enemyTargetableComponent.TargetEntity = _state.Value.TowersEntity[0];
                            enemyTargetableComponent.TargetObject = _viewPool.Value.Get(_state.Value.TowersEntity[0]).GameObject;

                            enemyTargetableComponent.AllEntityInDetectedZone = new List<int>();
                            enemyTargetableComponent.AllEntityInDamageZone = new List<int>();

                            targetWeightComponent.Value = 5;

                            healthComponent.CurrentValue = healthComponent.MaxValue;

                            movableComponent.Speed = 5f;

                            enemyViewComponent.Rigidbody = enemy.GetComponent<Rigidbody>();
                            enemyViewComponent.Transform = enemy.GetComponent<Transform>();
                            enemyViewComponent.Outline = enemy.GetComponent<Outline>();
                            enemyViewComponent.AttackMB = enemy.GetComponent<MeleeAttackMB>();
                            enemyViewComponent.NavMeshAgent = enemy.GetComponent<NavMeshAgent>();
                            enemyViewComponent.EcsInfoMB = enemy.GetComponent<EcsInfoMB>();
                            enemyViewComponent.EcsInfoMB.Init(_world);
                            enemyViewComponent.EcsInfoMB.SetEntity(enemyEntity);
                            enemyViewComponent.EcsInfoMB.SetTarget(enemyTargetableComponent.TargetEntity, enemyTargetableComponent.TargetObject);
                            enemyViewComponent.PointerTransform = enemy.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.transform;
                            enemyViewComponent.Healthbar = enemy.GetComponent<HealthbarMB>();
                            enemyViewComponent.Healthbar.SetMaxHealth(healthComponent.MaxValue);
                            enemyViewComponent.Healthbar.SetHealth(healthComponent.MaxValue);
                            enemyViewComponent.Healthbar.ToggleSwitcher();
                            enemyViewComponent.Healthbar.Init(systems.GetWorld(), systems.GetShared<GameState>());
                            for (int y = 0; y < enemyViewComponent.Transform.GetChild(0).transform.childCount; y++)
                            {
                                enemyViewComponent.DamagePopups.Add(enemy.transform.GetChild(0).transform.GetChild(y).gameObject);
                                enemyViewComponent.DamagePopups[y].SetActive(false);
                            }
                            
                            enemyShipComponent.Encounter = _encounter;

                            shipComponent.EnemyUnitsEntitys.Add(enemyEntity);


                        }
                        _enemyCountInEncounter++;
                        //Debug.Log("Dlinna " + encounters.Length + " " + _encounter + " " + encounters[_encounter] + " " + _enemyCountInEncounter);
                        if (_encounter <= encounters.Length && _enemyCountInEncounter == encounters[_encounter])
                        {
                            _encounter++;
                            _enemyCountInEncounter = 0;
                        }

                        _shipAngle = 0f;
                    }
                    _angle = 0f;
                    _encounter = 0;
                    _enemyCountInEncounter = 0;
                }
                // _state.Value.Saves.CurrentWave++;
                // _state.Value.Saves.SaveCurrentWave(_state.Value.Saves.CurrentWave);
                
                _filter.Pools.Inc1.Del(entity);
            }
        }
    }
}