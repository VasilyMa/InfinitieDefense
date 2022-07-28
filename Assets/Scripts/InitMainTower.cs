using System.Collections.Generic;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client
{
    sealed class InitMainTower : IEcsInitSystem
    {
        readonly EcsSharedInject<GameState> _state;
        readonly EcsWorldInject _world = default;
        readonly EcsPoolInject<MainTowerTag> _mainTowerPool = default;
        readonly EcsPoolInject<ViewComponent> _viewPool = default;
        readonly EcsPoolInject<RadiusComponent> _radiusPool = default;
        readonly EcsPoolInject<HealthComponent> _healthPool = default;
        readonly EcsPoolInject<TowerTag> _towerPool = default;
        readonly EcsPoolInject<DefenderComponent> _defenderPool = default;
        readonly EcsPoolInject<CanvasUpgradeComponent> _upgradeCanvasPool = default;
        readonly EcsPoolInject<CircleComponent> _circlePool = default;
        readonly EcsPoolInject<CreateNextTowerEvent> _createNextTowerPool = default;
        readonly EcsPoolInject<DestroyEffects> _destroyEffectsPool = default;
        private float Angle = 0;

        public void Init (EcsSystems systems)
        {
            var mainTowerEntity = _world.Value.NewEntity();
            _state.Value.EntityMainTower = mainTowerEntity;
            string towerID = _state.Value.DefenseTowers[0];
            //_state.Value.CurrentTowerID = towerID;
            
            ref var mainTowerComp = ref _mainTowerPool.Value.Add(mainTowerEntity);
            ref var upgradeComponent = ref _upgradeCanvasPool.Value.Add(mainTowerEntity);
            ref var towerComp = ref _towerPool.Value.Add(mainTowerEntity);
            ref var destroyEffectsComponent = ref _destroyEffectsPool.Value.Add(mainTowerEntity);
            towerComp.Level = 1;
            mainTowerComp.DefendersPositions = new Vector3[10];

            for (int d = 0; d < mainTowerComp.DefendersPositions.Length;d++)
            {
                var x = Mathf.Cos(Angle * Mathf.Deg2Rad) * 5;
                var z = Mathf.Sin(Angle * Mathf.Deg2Rad) * 5;
                
                Angle += 360 / 10;

                var defenderEntity = _world.Value.NewEntity();
                ref var defenderComp = ref _defenderPool.Value.Add(defenderEntity);
                defenderComp.Position = new Vector3(x, 0, z);
                _state.Value.DefendersEntity[d] = defenderEntity;
            }

            ref var radiusComp = ref _radiusPool.Value.Add(mainTowerEntity);
            radiusComp.Radius = _state.Value.TowerStorage.GetRadiusByID(towerID);

            ref var healthComponent = ref _healthPool.Value.Add(mainTowerEntity);
            healthComponent.MaxValue = _state.Value.TowerStorage.GetHealthByID(towerID);
            healthComponent.CurrentValue = healthComponent.MaxValue;

            var mainTower = GameObject.Instantiate(_state.Value.TowerStorage.GetTowerPrefabByID(towerID), Vector3.zero, Quaternion.identity);
            var upgradePoint = GameObject.Instantiate(_state.Value.InterfaceStorage.UpgradePointPrefab, new Vector3(0, 0.1f, -2.5f), Quaternion.identity); // to do del magic number
            var upgradeInfo = upgradePoint.transform.GetChild(0).gameObject.GetComponent<UpgradeCanvasMB>();
            var resourcesTimer = upgradePoint.transform.GetChild(3).GetComponent<TimerResourcesMB>();


            upgradeComponent.timerResources = resourcesTimer;
            upgradeComponent.point = upgradePoint.gameObject;
            upgradeComponent.upgrade = upgradeInfo;
            upgradeComponent.Index = 0;
            upgradeInfo.Init(systems.GetWorld(), systems.GetShared<GameState>());
            upgradeInfo.UpdateUpgradePoint(0, _state.Value.DefenseTowerStorage.GetUpgradeByID("1tower"));
            upgradeInfo.SetMaxAmount(_state.Value.DefenseTowerStorage.GetUpgradeByID("1tower"));

            resourcesTimer.Init(systems.GetWorld(), systems.GetShared<GameState>());
            resourcesTimer.ResourcesDrop(0);
            

            var upgradePointMB = upgradePoint.GetComponent<UpgradePointMB>();
            upgradePointMB.TowerIndex = 0;

            if (_state.Value.Saves.TutorialStage <= 11)
                upgradeComponent.point.SetActive(false);

            ref var viewComponent = ref _viewPool.Value.Add(mainTowerEntity);
            viewComponent.GameObject = mainTower;
            viewComponent.ModelMeshFilter = viewComponent.GameObject.transform.GetChild(2).GetComponent<MeshFilter>();
            viewComponent.Transform = mainTower.transform;
            Angle = 0;

            viewComponent.Healthbar = mainTower.GetComponent<HealthbarMB>();
            viewComponent.Healthbar.SetMaxHealth(healthComponent.MaxValue);
            viewComponent.Healthbar.SetHealth(healthComponent.MaxValue);
            viewComponent.Healthbar.Init(systems.GetWorld(), systems.GetShared<GameState>());
            viewComponent.UpgradeParticleSystem = upgradePoint.transform.GetChild(1).GetComponent<ParticleSystem>();

            viewComponent.EcsInfoMB = mainTower.GetComponent<EcsInfoMB>();
            viewComponent.EcsInfoMB.Init(_world);
            viewComponent.EcsInfoMB.SetEntity(mainTowerEntity);

            ref var targetWeightComponent = ref _world.Value.GetPool<TargetWeightComponent>().Add(mainTowerEntity);

            //viewComponent.Level = mainTower.GetComponent<LevelMB>();
            //viewComponent.Level.UpdateLevel(_state.Value.TowerStorage.GetLevelByID(towerID));
            //viewComponent.Level.Init(systems.GetWorld(), systems.GetShared<GameState>());

            viewComponent.LevelPopup = mainTower.transform.GetChild(0).transform.GetChild(2).transform.gameObject;
            viewComponent.LevelPopup.GetComponent<LevelPopupMB>().Init(systems.GetWorld(), systems.GetShared<GameState>());
            viewComponent.LevelPopup.SetActive(false);

            /*viewComponent.ResourcesTimer = mainTower.transform.GetChild(0).transform.GetChild(3).transform.gameObject;
            viewComponent.ResourcesTimer.GetComponent<TimerResourcesMB>().ResourcesDrop(0);
            viewComponent.ResourcesTimer.GetComponent<TimerResourcesMB>().Init(systems.GetWorld(), systems.GetShared<GameState>());
            viewComponent.ResourcesTimer.SetActive(true);*/

            viewComponent.DamagePopups = new List<GameObject>();
            for (int y = 0; y < viewComponent.Transform.GetChild(1).transform.childCount; y++)
            {
                var popup = viewComponent.Transform.GetChild(1).transform.GetChild(y).gameObject;
                viewComponent.DamagePopups.Add(popup);
                viewComponent.DamagePopups[y].SetActive(false);
            }
            targetWeightComponent.Value = 0;

            destroyEffectsComponent.DestroyEffectsMB = viewComponent.GameObject.GetComponentInChildren<DestroyEffectsMB>();
            destroyEffectsComponent.DestroyExplosion = destroyEffectsComponent.DestroyEffectsMB.GetDestroyExplosion();
            destroyEffectsComponent.DestroyFire = destroyEffectsComponent.DestroyEffectsMB.GetDestroyFire();

            destroyEffectsComponent.DestroyExplosion.Stop();
            destroyEffectsComponent.DestroyFire.Stop();

            int circleRadiusLevel = 0;
            int angleOffset = 90;
            int towerCount = 0;
            for (int i = 0; i < _state.Value.TowersEntity.Length;i++)
            {
                if(i == 0) _state.Value.TowersEntity[i] = mainTowerEntity;
                else
                {
                    if(towerCount == _state.Value.TowersInRow)
                    {
                        circleRadiusLevel++;
                        towerCount = 0;
                        Angle = circleRadiusLevel * angleOffset;
                    }
                    int defenseTowerEntity = _world.Value.NewEntity();
                    _state.Value.TowersEntity[i] = defenseTowerEntity;
                    ref var viewComp = ref _viewPool.Value.Add(defenseTowerEntity);
                    _healthPool.Value.Add(defenseTowerEntity);
                    _radiusPool.Value.Add(defenseTowerEntity);
                    ref var upgradeTowerComponent = ref _upgradeCanvasPool.Value.Add(defenseTowerEntity);
                    ref var tComp = ref _towerPool.Value.Add(defenseTowerEntity);

                    var radius = _state.Value.TowerStorage.GetRadiusByID((circleRadiusLevel + 1).ToString() + "tower");

                    var z = Mathf.Cos(Angle * Mathf.Deg2Rad) * radius;//radiusComp.Radius;//переделать постановку
                    var x = Mathf.Sin(Angle * Mathf.Deg2Rad) * radius;//radiusComp.Radius;
                    tComp.Position = new Vector3(x, 0, z);
                    tComp.CircleRadiusLevel = circleRadiusLevel;
                    

                    upgradePoint = GameObject.Instantiate(_state.Value.InterfaceStorage.DefenceTowerUpgradePointPrefab, new Vector3(x, 0.1f, z - 2.5f), Quaternion.identity); // to do del magic number
                    tComp.UpgradePointGO = upgradePoint;
                    if (circleRadiusLevel > towerComp.Level - 1)
                    {
                        upgradePoint.SetActive(false);
                    }

                    upgradePointMB = upgradePoint.GetComponent<UpgradePointMB>();
                    upgradeInfo = upgradePoint.transform.GetComponentInChildren<UpgradeCanvasMB>();
                    var resourcesTimerTower = upgradePoint.transform.GetComponentInChildren<TimerResourcesMB>();
                    upgradeTowerComponent.timerResources = resourcesTimerTower;
                    upgradeTowerComponent.point = upgradePoint.gameObject;
                    upgradeTowerComponent.upgrade = upgradeInfo;

                    resourcesTimerTower.Init(systems.GetWorld(), systems.GetShared<GameState>());
                    resourcesTimerTower.ResourcesDrop(0);
                    

                    upgradeInfo.Init(systems.GetWorld(), systems.GetShared<GameState>());
                    upgradeInfo.UpdateUpgradePoint(0, _state.Value.DefenseTowerStorage.GetUpgradeByID(towerID));
                    upgradeInfo.SetMaxAmount(_state.Value.DefenseTowerStorage.GetUpgradeByID(towerID));
                    upgradePointMB.TowerIndex = i;
                    upgradeTowerComponent.Index = upgradePointMB.TowerIndex;
                    viewComp.UpgradeParticleSystem = upgradePoint.transform.GetChild(1).GetComponent<ParticleSystem>();
                    
                    Angle += 360 / _state.Value.TowersInRow;
                    towerCount++;

                    if(_state.Value.DefenseTowers[i] != "empty")
                    {
                        ref var createTowerComp = ref _createNextTowerPool.Value.Add(_state.Value.TowersEntity[i]);
                        createTowerComp.TowerIndex = i;
                        createTowerComp.Change = false;
                    }
                }
            }
        }
    }
}