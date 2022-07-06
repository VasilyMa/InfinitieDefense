using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using UnityEngine.UI;

namespace Client {
    sealed class UserMoveSystem : IEcsRunSystem {
        readonly EcsSharedInject<GameState> _state = default;
        readonly EcsFilterInject<Inc<Player>> _playerFilter = default;
        readonly EcsPoolInject<InterfaceComponent> _interfacePool = default;

        public void Run (EcsSystems systems) {
            foreach (var entity in _playerFilter.Value)
            {
                ref var interfacePool = ref _interfacePool.Value.Get(_state.Value.EntityInterface);
                var _joystick = interfacePool._joystick;
                ref var player = ref _playerFilter.Pools.Inc1.Get(entity);
                player.rigidbody.velocity = new Vector3(_joystick.Horizontal * player.MoveSpeed, player.rigidbody.velocity.y, _joystick.Vertical * player.MoveSpeed) + new Vector3(0, player.rigidbody.velocity.y, 0);
                if (_joystick.Horizontal != 0 || _joystick.Vertical != 0)
                {
                    player.Transform.rotation = Quaternion.LookRotation(player.rigidbody.velocity);
                    player.animator.SetBool("isIdle", false);
                    player.animator.SetBool("isMining", false);
                    player.animator.SetBool("isRun", true);
                }
                else if (player.animator.GetBool("isMining"))
                {
                    player.animator.SetBool("isRun", false);
                    player.animator.SetBool("isIdle", false);
                }
                else if (!player.animator.GetBool("isMining"))
                {
                    player.animator.SetBool("isRun", false);
                    player.animator.SetBool("isIdle", true);
                }
                Debug.Log("Drag!");
            }
            
        }
    }
}