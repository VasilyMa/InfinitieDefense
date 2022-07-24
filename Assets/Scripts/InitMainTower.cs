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
        private float Angle = 0;

        public void Init (EcsSystems systems)
        {
            var mainTowerEntity = _world.Value.NewEntity();
            string towerID = _state.Value.DefenseTowers[0];
            //_state.Value.CurrentTowerID = towerID;
            
            ref var mainTowerComp = ref _mainTowerPool.Value.Add(mainTowerEntity);
            ref var upgradeComponent = ref _upgradeCanvasPool.Value.Add(mainTowerEntity);
            ref var towerComp = ref _towerPool.Value.Add(mainTowerEntity);
            towerComp.Level = 1;
            mainTowerComp.DefendersPositions = new Vector3[10];

            for (int d = 0; d < mainTowerComp.DefendersPositions.Length;d++)
            {
                var x = Mathf.Cos(Angle * Mathf.Deg2Rad) * 7;
                var z = Mathf.Sin(Angle * Mathf.Deg2Rad) * 7;
                
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
            var upgradePoint = GameObject.Instantiate(_state.Value.InterfaceStorage.UpgradePointPrefab, new Vector3(0, 0.1f, 0), Quaternion.identity);
            var upgradeInfo = upgradePoint.transform.GetChild(0).gameObject.GetComponent<UpgradeCanvasMB>();
            upgradeComponent.point = upgradePoint.gameObject;
            upgradeComponent.upgrade = upgradeInfo;
            upgradeComponent.Index = 0;
            upgradeInfo.Init(systems.GetWorld(), systems.GetShared<GameState>());
            upgradeInfo.UpdateUpgradePoint(0, _state.Value.DefenseTowerStorage.GetUpgradeByID("1tower"), _state.Value.DefenseTowerStorage.GetImageByID("1tower"));
            upgradeInfo.SetMaxAmount(_state.Value.DefenseTowerStorage.GetUpgradeByID("1tower"));

            var upgradePointMB = upgradePoint.GetComponent<UpgradePointMB>();
            upgradePointMB.TowerIndex = 0;

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



            ref var targetWeightComponent = ref _world.Value.GetPool<TargetWeightComponent>().Add(mainTowerEntity);

            //viewComponent.Level = mainTower.GetComponent<LevelMB>();
            //viewComponent.Level.UpdateLevel(_state.Value.TowerStorage.GetLevelByID(towerID));
            //viewComponent.Level.Init(systems.GetWorld(), systems.GetShared<GameState>());

            viewComponent.LevelPopup = mainTower.transform.GetChild(0).transform.GetChild(2).transform.gameObject;
            viewComponent.LevelPopup.GetComponent<LevelPopupMB>().Init(systems.GetWorld(), systems.GetShared<GameState>());
            viewComponent.LevelPopup.SetActive(false);

            viewComponent.ResourcesTimer = mainTower.transform.GetChild(0).transform.GetChild(3).transform.gameObject;
            viewComponent.ResourcesTimer.GetComponent<TimerResourcesMB>().ResourcesDrop(0);
            viewComponent.ResourcesTimer.GetComponent<TimerResourcesMB>().Init(systems.GetWorld(), systems.GetShared<GameState>());
            viewComponent.ResourcesTimer.SetActive(true);

            viewComponent.DamagePopups = new List<GameObject>();
            for (int y = 0; y < viewComponent.Transform.GetChild(1).transform.childCount; y++)
            {
                var popup = viewComponent.Transform.GetChild(1).transform.GetChild(y).gameObject;
                viewComponent.DamagePopups.Add(popup);
                viewComponent.DamagePopups[y].SetActive(false);
            }
            targetWeightComponent.Value = 0;

            int circleRadiusLevel = 0;
            int angleOffset = 90;
            int towerCount = 0;
            for (int i = 0; i < _state.Value.TowersEntity.Length;i++)
            {
                if(i == 0) _state.Value.TowersEntity[i] = mainTowerEntity;
                else
                {
                    if(towerCount == 6)
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

                    var x = Mathf.Cos(Angle * Mathf.Deg2Rad) * radius;//radiusComp.Radius;//переделать постановку
                    var z = Mathf.Sin(Angle * Mathf.Deg2Rad) * radius;//radiusComp.Radius;
                    tComp.Position = new Vector3(x, 0, z);
                    tComp.CircleRadiusLevel = circleRadiusLevel;
                    

                    upgradePoint = GameObject.Instantiate(_state.Value.InterfaceStorage.UpgradePointPrefab, new Vector3(x, 0.1f, z), Quaternion.identity);
                    tComp.UpgradePointGO = upgradePoint;
                    if (circleRadiusLevel > towerComp.Level - 1)
                    {
                        upgradePoint.SetActive(false);
                    }

                    upgradePointMB = upgradePoint.GetComponent<UpgradePointMB>();
                    upgradeInfo = upgradePoint.transform.GetChild(0).gameObject.GetComponent<UpgradeCanvasMB>();
                    upgradeTowerComponent.point = upgradePoint.gameObject;
                    upgradeTowerComponent.upgrade = upgradeInfo;

                    upgradeInfo.Init(systems.GetWorld(), systems.GetShared<GameState>());
                    upgradeInfo.UpdateUpgradePoint(0, _state.Value.DefenseTowerStorage.GetUpgradeByID(towerID), _state.Value.DefenseTowerStorage.GetImageByID(towerID));
                    upgradeInfo.SetMaxAmount(_state.Value.DefenseTowerStorage.GetUpgradeByID(towerID));
                    upgradePointMB.TowerIndex = i;
                    upgradeTowerComponent.Index = upgradePointMB.TowerIndex;
                    viewComp.UpgradeParticleSystem = upgradePoint.transform.GetChild(1).GetComponent<ParticleSystem>();
                    
                    
                    Angle += 360 / 6;
                    towerCount++;

                    if(_state.Value.DefenseTowers[i] != "empty")
                    {
                        ref var createTowerComp = ref _createNextTowerPool.Value.Add(_state.Value.TowersEntity[i]);
                        createTowerComp.TowerIndex = i;
                        createTowerComp.Change = false;
                    }
                }
            }


            Mesh mesh = new Mesh();
            viewComponent.MeshFilter = viewComponent.GameObject.GetComponent<MeshFilter>();
            viewComponent.MeshFilter.mesh = mesh;

            viewComponent.LineRenderer = viewComponent.GameObject.GetComponent<LineRenderer>();
            viewComponent.LineRenderer.loop = true;

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
            viewComponent.LineRenderer.positionCount = triangelesCount;

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
            ref var circleComp = ref _circlePool.Value.Add(_world.Value.NewEntity());
            circleComp.maxDistance = radiusComp.Radius;
            mesh.vertices = vertices;
            //mesh.uv = uv;
            mesh.triangles = trianglesVertices;

            viewComponent.LineRenderer.SetPositions(circleVerticesv);
        }
    }
}