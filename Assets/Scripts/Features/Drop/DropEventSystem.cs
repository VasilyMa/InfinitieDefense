using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client
{
    sealed class DropEventSystem : IEcsRunSystem
    {
        readonly EcsWorldInject _world = default;

        readonly EcsSharedInject<GameState> _state = default;

        readonly EcsFilterInject<Inc<DropEvent>> _dropEventFilter = default;

        readonly EcsPoolInject<DropEvent> _dropEventPool = default;

        public void Run (EcsSystems systems)
        {
            foreach (var eventEntity in _dropEventFilter.Value)
            {
                ref var dropEvent = ref _dropEventPool.Value.Get(eventEntity);

                var dropedItem = GameObject.Instantiate(_state.Value.DropableItemStorage.DroppedWeaponPrefab[((int)dropEvent.Item)], dropEvent.Point, Quaternion.identity);

                DropInfoMB itemDropInfoMB;

                if (!dropedItem.GetComponent<DropInfoMB>())
                {
                    itemDropInfoMB = dropedItem.AddComponent<DropInfoMB>();
                }
                else
                {
                    itemDropInfoMB = dropedItem.GetComponent<DropInfoMB>();
                }

                itemDropInfoMB.SetItem(dropEvent.Item, _world);

                _dropEventPool.Value.Del(eventEntity);
            }
        }
    }
}