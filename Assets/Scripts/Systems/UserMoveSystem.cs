using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using UnityEngine.UI;

namespace Client {
    sealed class UserMoveSystem : IEcsRunSystem {
        readonly EcsSharedInject<GameState> _state = default;
        readonly EcsFilterInject<Inc<Player>, Exc<DeadTag>> _playerFilter = default;
        readonly EcsPoolInject<InterfaceComponent> _interfacePool = default;
        readonly EcsPoolInject<ViewComponent> _viewPool = default;
        readonly EcsPoolInject<CameraComponent> _cameraPool = default;

        public void Run (EcsSystems systems) {
            foreach (var entity in _playerFilter.Value)
            {
                ref var interfacePool = ref _interfacePool.Value.Get(_state.Value.EntityInterface);
                var _joystick = interfacePool._joystick;
                ref var player = ref _playerFilter.Pools.Inc1.Get(entity);
                ref var viewComponent = ref _viewPool.Value.Get(entity);
                ref var cameraComponent = ref _cameraPool.Value.Get(_state.Value.EntityCamera);

                Vector3 cameraRight = cameraComponent.CameraTransform.right;
                Vector3 cameraForward = cameraComponent.CameraTransform.forward;

                cameraRight.y = 0;
                cameraForward.y = 0;

                // Vector3 for movement regarding the camera
                Vector3 movementVector = cameraForward.normalized * _joystick.Vertical + cameraRight.normalized * _joystick.Horizontal;
                movementVector = Vector3.ClampMagnitude(movementVector, 1);

                player.rigidbody.velocity = new Vector3(movementVector.x * player.MoveSpeed,
                                                        player.rigidbody.velocity.y,
                                                        movementVector.z * player.MoveSpeed);

                // Vector3 for correct aim moving
                Vector3 relativeVector = viewComponent.Transform.InverseTransformDirection(movementVector);

                player.animator.SetFloat("RunX", relativeVector.x);
                player.animator.SetFloat("RunZ", relativeVector.z);

                if (_joystick.Horizontal != 0 || _joystick.Vertical != 0)
                {
                    player.Transform.rotation = Quaternion.LookRotation(player.rigidbody.velocity);
                    player.animator.SetBool("isIdle", false);
                    player.animator.SetBool("isRun", true);

                    if (viewComponent.WayTrack.particleCount == 0)
                    {
                        viewComponent.WayTrack.transform.parent = null;
                        viewComponent.WayTrack.transform.position = viewComponent.Transform.position;
                        viewComponent.WayTrack.Play();
                    }
                }
                else if (player.animator.GetBool("isMining"))
                {
                    player.animator.SetBool("isRun", false);
                    player.animator.SetBool("isIdle", false);
                    viewComponent.WayTrack.Stop();
                }
                else if (!player.animator.GetBool("isMining"))
                {
                    player.animator.SetBool("isRun", false);
                    player.animator.SetBool("isIdle", true);
                    viewComponent.WayTrack.Stop();
                }
                else
                    viewComponent.WayTrack.Stop();
                //Debug.Log("Drag!");
            }
            
        }
    }
}