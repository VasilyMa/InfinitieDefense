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
        readonly EcsPoolInject<MainTowerTag> _towerPool = default;
        readonly EcsPoolInject<ViewComponent> _viewPool = default;
        readonly EcsPoolInject<RadiusComponent> _radiusPool = default;
        readonly EcsPoolInject<HealthComponent> _healthPool = default;
        readonly EcsPoolInject<TowerTag> _tPool = default;
        readonly EcsPoolInject<DefenderComponent> _defenderPool = default;
        readonly EcsPoolInject<CanvasUpgradeComponent> _upgradeCanvasPool = default;
        readonly EcsPoolInject<CircleComponent> _circlePool = default;
        private float Angle = 0;

        public void Init (EcsSystems systems)
        {
            var entity = _world.Value.NewEntity();
            string towerID = _state.Value.DefenseTowers[0];
            //_state.Value.CurrentTowerID = towerID;
            
            ref var towerComp = ref _towerPool.Value.Add(entity);
            ref var upgradeComponent = ref _upgradeCanvasPool.Value.Add(entity);
            
            towerComp.DefendersPositions = new Vector3[10];

            for (int d = 0; d < towerComp.DefendersPositions.Length;d++)
            {
                var x = Mathf.Cos(Angle * Mathf.Deg2Rad) * 7;
                var z = Mathf.Sin(Angle * Mathf.Deg2Rad) * 7;
                
                Angle += 360 / 10;

                var ent = _world.Value.NewEntity();
                ref var defComp = ref _defenderPool.Value.Add(ent);
                defComp.Position = new Vector3(x, 0, z);
                _state.Value.DefendersEntity[d] = ent;
            }

            ref var radiusComp = ref _radiusPool.Value.Add(entity);
            radiusComp.Radius = _state.Value.TowerStorage.GetRadiusByID(towerID);

            ref var healthComponent = ref _healthPool.Value.Add(entity);
            healthComponent.MaxValue = _state.Value.TowerStorage.GetHealthByID(towerID);
            healthComponent.CurrentValue = healthComponent.MaxValue;

            GameObject upgradePoint = null;
            UpgradePointMB upgradePointMB = null;
            UpgradeCanvasMB upgradeInfo = null;

            var mainTower = GameObject.Instantiate(_state.Value.TowerStorage.GetTowerPrefabByID(towerID), Vector3.zero, Quaternion.identity);
            upgradePoint = GameObject.Instantiate(_state.Value.InterfaceStorage.UpgradePointPrefab, new Vector3(0, 0.1f, 0), Quaternion.identity);
            upgradeInfo = upgradePoint.transform.GetChild(0).gameObject.GetComponent<UpgradeCanvasMB>();
            upgradeComponent.point = upgradePoint.gameObject;
            upgradeComponent.upgrade = upgradeInfo;
            upgradeComponent.Index = 0;
            upgradeInfo.Init(systems.GetWorld(), systems.GetShared<GameState>());
            upgradeInfo.UpdateUpgradePoint(0, _state.Value.DefenseTowerStorage.GetUpgradeByID("empty"));

            upgradePointMB = upgradePoint.GetComponent<UpgradePointMB>();
            upgradePointMB.TowerIndex = 0;

            ref var viewComponent = ref _viewPool.Value.Add(entity);
            viewComponent.GameObject = mainTower;
            viewComponent.Transform = mainTower.transform;
            Angle = 0;

            viewComponent.Healthbar = mainTower.GetComponent<HealthbarMB>();
            viewComponent.Healthbar.SetMaxHealth(healthComponent.MaxValue);
            viewComponent.Healthbar.SetHealth(healthComponent.MaxValue);
            viewComponent.Healthbar.Init(systems.GetWorld(), systems.GetShared<GameState>());
            viewComponent.UpgradeParticleSystem = upgradePoint.transform.GetChild(1).GetComponent<ParticleSystem>();

            ref var targetWeightComponent = ref _world.Value.GetPool<TargetWeightComponent>().Add(entity);

            targetWeightComponent.Value = 0;

            int circle = 0;
            int towerCount = 0;
            for (int i = 0; i < _state.Value.TowersEntity.Length;i++)
            {
                if(i == 0) _state.Value.TowersEntity[i] = entity;
                else
                {
                    if(towerCount == 6)
                    {
                        circle++;
                        towerCount = 0;
                        Angle = circle * 45;
                    }
                    int towerEntity = _world.Value.NewEntity();
                    _state.Value.TowersEntity[i] = towerEntity;
                    ref var viewComp = ref _viewPool.Value.Add(towerEntity);
                    _healthPool.Value.Add(towerEntity);
                    _radiusPool.Value.Add(towerEntity);
                    ref var upgradeTowerComponent = ref _upgradeCanvasPool.Value.Add(towerEntity);
                    ref var tComp = ref _tPool.Value.Add(towerEntity);

                    var radius = _state.Value.TowerStorage.GetRadiusByID((circle + 1).ToString() + "tower");

                    var x = Mathf.Cos(Angle * Mathf.Deg2Rad) * radius;//radiusComp.Radius;//переделать постановку
                    var z = Mathf.Sin(Angle * Mathf.Deg2Rad) * radius;//radiusComp.Radius;
                    tComp.Position = new Vector3(x, 0, z);
                    tComp.Circle = circle;
                    

                    upgradePoint = GameObject.Instantiate(_state.Value.InterfaceStorage.UpgradePointPrefab, new Vector3(x, 0.1f, z), Quaternion.identity);
                    tComp.UpgradePointGO = upgradePoint;
                    if(circle > _state.Value.Saves.Circle)
                    {
                        upgradePoint.SetActive(false);
                    }
                    upgradePointMB = upgradePoint.GetComponent<UpgradePointMB>();
                    upgradeInfo = upgradePoint.transform.GetChild(0).gameObject.GetComponent<UpgradeCanvasMB>();
                    upgradeTowerComponent.point = upgradePoint.gameObject;
                    upgradeTowerComponent.upgrade = upgradeInfo;

                    upgradeInfo.Init(systems.GetWorld(), systems.GetShared<GameState>());
                    upgradeInfo.UpdateUpgradePoint(0, _state.Value.DefenseTowerStorage.GetUpgradeByID(towerID));
                    upgradePointMB.TowerIndex = i;
                    upgradeTowerComponent.Index = upgradePointMB.TowerIndex;
                    viewComp.UpgradeParticleSystem = upgradePoint.transform.GetChild(1).GetComponent<ParticleSystem>();

                    Angle += 360 / 6;
                    towerCount++;
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