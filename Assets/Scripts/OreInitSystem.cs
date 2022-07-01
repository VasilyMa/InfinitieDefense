using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using System.Collections.Generic;
namespace Client {
    sealed class OreInitSystem : IEcsInitSystem {
        readonly EcsSharedInject<GameState> _state;
        readonly EcsWorldInject _world = default;
        readonly EcsPoolInject<OreComponent> _orePool = default;
        private string Ore;
        public void Init (EcsSystems systems) {
            List<GameObject[]> Ores = new List<GameObject[]>();
            var allOres = GameObject.FindGameObjectsWithTag(nameof(Ore));

            Ores.Add(allOres);
            foreach (var ore in allOres)
            {
                int amount = Random.Range(2, 7);
                var oresEntity = _world.Value.NewEntity();
                _orePool.Value.Add(oresEntity);
                ref var orePool = ref _orePool.Value.Get(oresEntity);
                orePool.prefab = ore;
                orePool.amount = amount;
            }
        }
    }
}