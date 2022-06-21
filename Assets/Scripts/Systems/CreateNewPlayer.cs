using Leopotam.EcsLite;
using UnityEngine;
using Leopotam.EcsLite.Di;

namespace Client {
    sealed class CreateNewPlayer : IEcsRunSystem {
        readonly EcsSharedInject<GameState> _state = default;
        readonly EcsFilterInject<Inc<CreateNewPlayerEvent>> _filter = default;
        readonly EcsPoolInject<ViewComponent> _viewPool = default;
        readonly EcsPoolInject<Player> _playerPool = default;

        public void Run (EcsSystems systems) {
            foreach(var entity in _filter.Value)
            {
                ref var viewComp = ref _viewPool.Value.Get(entity);
                ref var playerComp = ref _playerPool.Value.Get(entity);

                _state.Value.CurrentPlayerID = _state.Value.PlayerStorage.GetNextIdByID(_state.Value.CurrentPlayerID);
                //Debug.Log("dfdfdfdfd " + _state.Value.CurrentPlayerID);
                playerComp.damage = _state.Value.PlayerStorage.GetDamageByID(_state.Value.CurrentPlayerID);
                playerComp.health = _state.Value.PlayerStorage.GetHealthByID(_state.Value.CurrentPlayerID);
                viewComp.UpgradeParticleSystem.Play();
                //viewComp.SkinnedMeshRenderer.sharedMesh = _state.Value.PlayerStorage.GetMeshByID(_state.Value.CurrentPlayerID);

                _filter.Pools.Inc1.Del(entity);
            }
        }
    }
}