using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using UnityEngine.UI;

namespace Client {
    sealed class UserMoveSystem : IEcsRunSystem {
        readonly EcsSharedInject<GameState> _state = default;
        readonly EcsFilterInject<Inc<Player>> _playerFilter = default;
        readonly EcsPoolInject<InterfaceComponent> _interfacePool = default;
        private float _angleOffset = 30f;

        public void Run (EcsSystems systems) {
            foreach (var entity in _playerFilter.Value)
            {
                ref var interfacePool = ref _interfacePool.Value.Get(_state.Value.EntityInterface);
                var _joystick = interfacePool._joystick;
                ref var player = ref _playerFilter.Pools.Inc1.Get(entity);

                float angle = Mathf.Atan2(_joystick.gameObject.transform.GetChild(0).transform.GetChild(0).transform.position.y - _joystick.gameObject.transform.GetChild(0).transform.position.y,
                _joystick.gameObject.transform.GetChild(0).transform.GetChild(0).transform.position.x - _joystick.gameObject.transform.GetChild(0).transform.position.x) * Mathf.Rad2Deg - _angleOffset;

                if(angle < 0)
                {
                    angle += 360;
                }
                else
                {
                    angle -= 360;
                }
                float radius = new Vector2(_joystick.Horizontal, _joystick.Vertical).sqrMagnitude;
                var x = Mathf.Cos((angle) * Mathf.Deg2Rad) * radius;
                var z = Mathf.Sin((angle) * Mathf.Deg2Rad) * radius;

                player.rigidbody.velocity = new Vector3(x * player.MoveSpeed, 
                player.rigidbody.velocity.y, 
                z * player.MoveSpeed);

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
                //Debug.Log("Drag!");
            }
            
        }
    }
}