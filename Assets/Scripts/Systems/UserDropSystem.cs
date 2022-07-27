using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
namespace Client {
    sealed class UserDropSystem : IEcsRunSystem {
        readonly EcsSharedInject<GameState> _state = default;
        readonly EcsWorldInject _world = default;
        readonly EcsFilterInject<Inc<DropByDie, DeadTag>> _filter = default;
        readonly EcsPoolInject<ViewComponent> _viewPool = default;
        readonly EcsPoolInject<DropByDieComponent> _dropPool = default;
        readonly EcsPoolInject<Player> _playerPool = default;
        readonly EcsPoolInject<InterfaceComponent> _interfacePool = default;
        public void Run (EcsSystems systems) {
            foreach (var entity in _filter.Value)
            {
                ref var _viewComp = ref _viewPool.Value.Get(_state.Value.EntityPlayer);
                ref var _playerComp = ref _playerPool.Value.Get(_state.Value.EntityPlayer);
                ref var _interfaceComp = ref _interfacePool.Value.Get(_state.Value.EntityInterface);
                //for (int i = 0; i < _state.Value.CoinTransformList.Count; i++)
                //{
                //    _state.Value.CoinTransformList[i].gameObject.tag = "Coin";
                    
                //}
                //_state.Value.CoinTransformList.Clear();
                //_state.Value.CoinCount = 0;
                //_interfaceComp.resourcePanel.GetComponent<ResourcesPanelMB>().UpdateGold();
                for (int i = 0; i < _state.Value.StoneTransformList.Count; i++)
                {
                    _state.Value.StoneTransformList[i].gameObject.tag = "Stone";
                    
                }
                _state.Value.StoneTransformList.Clear();
                _state.Value.RockCount = 0;
                _interfaceComp.resourcePanel.GetComponent<ResourcesPanelMB>().UpdateStone();
                for (int i = 0; i < _playerComp.ResHolderTransform.childCount; i++)
                {
                    ref var _dropComp = ref _dropPool.Value.Add(_world.Value.NewEntity());
                    _dropComp.StartPosition = _viewComp.Transform.position;
                    _dropComp.TargetPosition = new Vector3(_viewComp.GameObject.transform.position.x + Random.Range(-2, 2), _viewComp.GameObject.transform.position.y, _viewComp.GameObject.transform.position.z + Random.Range(-2, 2)); ;
                    _dropComp.Speed = 7f;
                    _dropComp.Object = _playerComp.ResHolderTransform.GetChild(i).gameObject;
                    if (_dropComp.Object.CompareTag("Coin"))
                        _dropComp.Coin = true;
                }
                _filter.Pools.Inc1.Del(entity);
            }
        }
    }
}