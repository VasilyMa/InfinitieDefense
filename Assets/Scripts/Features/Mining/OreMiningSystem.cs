using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using Unity;
namespace Client {
    sealed class OreMiningSystem : IEcsRunSystem {
        readonly EcsFilterInject<Inc<OreEventComponent, OreComponent>> _filter = default;
        public void Run (EcsSystems systems) {
            foreach (var entity in _filter.Value)
            {
                ref var oreComp = ref _filter.Pools.Inc2.Get(entity);
                oreComp.amount--;
                var stone = (GameObject)GameObject.Instantiate(Resources.Load("Stone"), oreComp.prefab.transform.position, Quaternion.identity);
                stone.transform.position = Vector3.Lerp(stone.transform.position, new Vector3(
                    stone.transform.position.x + Random.Range(-10.5f, 15.0f),
                    stone.transform.position.y,
                    stone.transform.position.z + Random.Range(-10.5f, 15.0f)),
                    Time.deltaTime * 10f);
                if (oreComp.amount <= 0) { oreComp.prefab.gameObject.SetActive(false); _filter.Pools.Inc2.Del(entity); }               
                _filter.Pools.Inc1.Del(entity);
            }
        }
    }
}