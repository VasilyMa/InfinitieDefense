using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client
{
    sealed class PickUpNewWeaponEventSystem : IEcsRunSystem
    {
        // readonly EcsWorldInject _world = default;

        readonly EcsSharedInject<GameState> _state = default;

        readonly EcsFilterInject<Inc<PickUpNewWeaponEvent>> _pickUpNewWeaponEventFilter = default;

        readonly EcsPoolInject<PickUpNewWeaponEvent> _pickUpNewWeaponEventPool = default;
        readonly EcsPoolInject<ViewComponent> _viewPool = default;
        readonly EcsPoolInject<PlayerWeapon> _playerWeaponPool = default;
        readonly EcsPoolInject<Targetable> _targetablePool = default;
        readonly EcsPoolInject<ActivateContextToolEvent> _activateContextToolEventPool = default;

        public void Run(EcsSystems systems)
        {
            foreach (var eventEntity in _pickUpNewWeaponEventFilter.Value)
            {
                ref var pickUpNewWeaponEvent = ref _pickUpNewWeaponEventPool.Value.Get(eventEntity);

                var playerEntity = _state.Value.EntityPlayer;
                ref var viewComponent = ref _viewPool.Value.Get(playerEntity);
                ref var playerWeaponComponent = ref _playerWeaponPool.Value.Get(playerEntity);
                ref var targetableComponent = ref _targetablePool.Value.Get(playerEntity);
                ref var activateContextToolEventComponent = ref _activateContextToolEventPool.Value.Add(playerEntity);
                targetableComponent.EntitysInMeleeZone.Clear();

                // to do ay clear AllEnemyInDamageZone if weapon was changed

                switch (pickUpNewWeaponEvent.WeaponType)
                {
                    case DropableItem.ItemType.Sword:
                        {
                            playerWeaponComponent.MeleeAttackMB.gameObject.SetActive(true);
                            viewComponent.Animator.SetBool("Melee", true);
                        }
                        break;
                    case DropableItem.ItemType.Bow:
                        {
                            playerWeaponComponent.RangeAttackMB.gameObject.SetActive(true);
                            viewComponent.Animator.SetBool("Range", true);
                        }
                        break;
                    default:
                        {
                            // to do ay some shit
                        }
                        break;
                }

                activateContextToolEventComponent.ActiveTool = ContextToolComponent.Tool.empty;

                _pickUpNewWeaponEventPool.Value.Del(eventEntity);
            }
        }
    }
}