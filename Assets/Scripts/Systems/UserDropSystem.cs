using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
namespace Client {
    sealed class UserDropSystem : IEcsRunSystem {
        readonly EcsSharedInject<GameState> _state = default;
        readonly EcsWorldInject _world = default;
        readonly EcsFilterInject<Inc<DropByDie>> _filter = default;
        readonly EcsPoolInject<ViewComponent> _viewPool = default;
        readonly EcsPoolInject<DropByDieComponent> _dropPool = default;
        readonly EcsPoolInject<Player> _playerPool = default;
        public void Run (EcsSystems systems) {
            foreach (var entity in _filter.Value)
            {
                ref var _dropComp = ref _dropPool.Value.Add(_world.Value.NewEntity());
                ref var _viewComp = ref _viewPool.Value.Get(_state.Value.EntityPlayer);
                ref var _playerComp = ref _playerPool.Value.Get(_state.Value.EntityPlayer);
                _dropComp.StartPosition = _viewComp.Transform.position;
                _dropComp.TargetPosition = _viewComp.Transform.position;
                _dropComp.Transform = new System.Collections.Generic.List<Transform>();
                for (int i = 0; i < _playerComp.ResHolderTransform.childCount; i++)
                {
                    _dropComp.Transform.Add(_playerComp.ResHolderTransform.GetChild(i));
                    //_state.Value.CoinTransformList.Remove(_playerComp.ResHolderTransform.GetChild(i));
                    //_state.Value.StoneTransformList.Remove(_playerComp.ResHolderTransform.GetChild(i));
                }
                _state.Value.CoinTransformList.Clear();
                _state.Value.StoneTransformList.Clear();
            }
        }
    }
}