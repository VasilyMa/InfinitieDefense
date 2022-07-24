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

        public void Run(EcsSystems systems)
        {
            foreach (var eventEntity in _pickUpNewWeaponEventFilter.Value)
            {
                ref var pickUpNewWeaponEvent = ref _pickUpNewWeaponEventPool.Value.Get(eventEntity);

                var playerEntity = _state.Value.EntityPlayer;
                ref var viewComponent = ref _viewPool.Value.Get(playerEntity);
                ref var playerWeaponComponent = ref _playerWeaponPool.Value.Get(playerEntity);

                switch (pickUpNewWeaponEvent.WeaponType)
                {
                    case DropableItem.ItemType.Sword:
                        {
                            playerWeaponComponent.MeleeAttackMB.gameObject.SetActive(true);
                            viewComponent.Animator.SetBool("Melee", true);

                            playerWeaponComponent.RangeAttackMB.gameObject.SetActive(false);
                            viewComponent.Animator.SetBool("Range", false);
                        }
                        break;
                    case DropableItem.ItemType.Bow:
                        {
                            playerWeaponComponent.MeleeAttackMB.gameObject.SetActive(false);
                            viewComponent.Animator.SetBool("Melee", false);

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

                _pickUpNewWeaponEventPool.Value.Del(eventEntity);
            }
        }
    }
}