using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client
{
    sealed class CameraFollowSystem : IEcsRunSystem
    {
        readonly EcsFilterInject<Inc<CameraComponent>> _cameraFilter = default;

        readonly EcsPoolInject<CameraComponent> _cameraPool = default;
        readonly EcsPoolInject<ViewComponent> _viewPool = default;
        readonly EcsPoolInject<Player> _playerPool = default;

        readonly EcsSharedInject<GameState> _state;
        
        private Vector3 _holderOffset = new Vector3(0, 20, -20);
        private float _holderFollowSpeed = 5f;

        public void Run (EcsSystems systems)
        {
            foreach (var cameraEntity in _cameraFilter.Value)
            {
                ref var cameraComponent = ref _cameraPool.Value.Get(cameraEntity);
                ref var playerComponent = ref _playerPool.Value.Get(_state.Value.EntityPlayer);

                if (!cameraComponent.FollowObject)
                {
                    cameraComponent.FollowObject = playerComponent.Transform.gameObject;
                    cameraComponent.FollowTransform = playerComponent.Transform;
                }

                var actualPosition = Vector3.Lerp(cameraComponent.HolderTransform.position, cameraComponent.FollowTransform.position + _holderOffset, Time.deltaTime * _holderFollowSpeed);
                cameraComponent.HolderTransform.position = actualPosition;
                cameraComponent.CameraTransform.LookAt(cameraComponent.FollowTransform);
            }
        }
    }
}