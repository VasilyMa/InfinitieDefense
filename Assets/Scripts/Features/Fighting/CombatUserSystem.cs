using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
namespace Client {
    sealed class CombatUserSystem : IEcsRunSystem {
        readonly EcsSharedInject<GameState> _state;
        readonly EcsFilterInject<Inc<CombatEventComponent, FightingComponent, ViewComponent>> _filter = default;
        readonly EcsPoolInject<Player> _player = default;
        readonly EcsPoolInject<DroppedGoldEvent> _goldPool = default;
        readonly EcsWorldInject _world = default;
        public void Run (EcsSystems systems) {
            foreach (var entity in _filter.Value)
            {
                ref var fightComp = ref _filter.Pools.Inc2.Get(entity);
                ref var player = ref _player.Value.Get(_state.Value.EntityPlayer);
                ref var viewComp = ref _filter.Pools.Inc3.Get(entity);
                fightComp.HealthPoints -= player.damage;
                Debug.Log(fightComp.HealthPoints);
                if (fightComp.HealthPoints <= 0)
                {
                    ref var goldComp = ref _goldPool.Value.Add(_world.Value.NewEntity());
                    goldComp.Position = viewComp.Transform.position;
                    GameObject.Destroy(viewComp.GameObject);
                    _filter.Pools.Inc2.Del(entity);
                    _filter.Pools.Inc3.Del(entity);
                }
                _filter.Pools.Inc1.Del(entity);
            }
        }
    }
}