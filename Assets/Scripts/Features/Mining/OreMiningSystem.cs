using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using Unity;
namespace Client {
    sealed class OreMiningSystem : IEcsRunSystem {
        readonly EcsWorldInject _world = default;
        readonly EcsFilterInject<Inc<OreEventComponent, OreComponent>> _filter = default;
        readonly EcsFilterInject<Inc<Player>> _filterPlayer = default;
        readonly EcsPoolInject<OreMoveEvent> _movePool = default;
        public void Run (EcsSystems systems) {
            foreach (var entity in _filter.Value)
            {
                ref var oreComp = ref _filter.Pools.Inc2.Get(entity);
                ref var moveComp = ref _movePool.Value.Add(_world.Value.NewEntity());
                oreComp.amount--;
                var stone = (GameObject)GameObject.Instantiate(Resources.Load("Stone"), new Vector3(oreComp.prefab.transform.position.x, oreComp.prefab.transform.position.y + Random.Range(0.5f, 1.2f), oreComp.prefab.transform.position.z), Quaternion.identity);
                
                moveComp.stone = stone;
                moveComp.TargetPosition = new Vector3(oreComp.prefab.transform.position.x + Random.Range(-4, 4), oreComp.prefab.transform.position.y, oreComp.prefab.transform.position.z + Random.Range(-4, 4));
                moveComp.Speed = 10f;
                moveComp.outTime = 0.5f;

                if (oreComp.amount <= 0) 
                { 
                    oreComp.prefab.GetComponent<SphereCollider>().enabled = false;
                    oreComp.prefab.gameObject.SetActive(false); 
                    _filter.Pools.Inc2.Del(entity);
                    foreach (var entityPlayer in _filterPlayer.Value)
                    {
                        ref var player = ref _filterPlayer.Pools.Inc1.Get(entityPlayer);
                        player.animator.SetBool("isMining", false);
                        player.animator.SetBool("isIdle", true);
                    }
                }
                _filter.Pools.Inc1.Del(entity);
            }
        }
    }
}