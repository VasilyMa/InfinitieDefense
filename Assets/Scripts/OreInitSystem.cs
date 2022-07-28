using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using System.Collections.Generic;

namespace Client
{
    sealed class OreInitSystem : IEcsInitSystem
    {
        readonly EcsSharedInject<GameState> _state;

        readonly EcsWorldInject _world = default;

        readonly EcsPoolInject<OreComponent> _orePool = default;
        readonly EcsPoolInject<ViewComponent> _viewPool = default;

        private int _amount = 4;

        private string Ore;
        public void Init (EcsSystems systems)
        {
            List<GameObject[]> Ores = new List<GameObject[]>();
            var allOres = GameObject.FindGameObjectsWithTag(nameof(Ore));

            Ores.Add(allOres);

            foreach (var ore in allOres)
            {
                var oresEntity = _world.Value.NewEntity();
                ref var orePool = ref _orePool.Value.Add(oresEntity);
                ref var viewComponent = ref _viewPool.Value.Add(oresEntity);

                viewComponent.GameObject = ore;
                viewComponent.Animator = ore.GetComponent<Animator>();
                viewComponent.EcsInfoMB = ore.GetComponent<EcsInfoMB>();
                viewComponent.EcsInfoMB.Init(_world);
                viewComponent.EcsInfoMB.SetEntity(oresEntity);

                orePool.prefab = ore;
                orePool.MaxAmount = _amount;
                orePool.CurrentAmount = _amount;
                orePool.IsEnable = false;
                orePool.respawnTime = 10;
                //orePool.cursor = orePool.prefab.transform.GetChild(1).gameObject;
                //orePool.cursor.SetActive(false);
                GameObject oreModel = ore.transform.GetChild(0).gameObject;
                orePool.OreParts = new GameObject[oreModel.transform.childCount];
                for (int i = 0; i < oreModel.transform.childCount; i++)
                {
                    orePool.OreParts[i] = oreModel.transform.GetChild(i).gameObject;
                }
            }
        }
    }
}