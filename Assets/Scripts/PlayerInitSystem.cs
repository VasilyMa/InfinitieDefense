using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client {
    sealed class PlayerInitSystem : IEcsInitSystem {
        readonly EcsPoolInject<Player> _playerPool = default;
        readonly EcsWorldInject _world = default;
        readonly EcsSharedInject<GameState> _state = default;
        public void Init (EcsSystems systems) 
        {
            
            var playerEntity = _playerPool.Value.GetWorld().NewEntity();
            ref var player = ref _playerPool.Value.Add (playerEntity);


            //var PlayerPrefab = Resources.Load("Player");
            var PlayerGo = GameObject.Instantiate(_state.Value.PlayerStorage.GetPlayerByID("1level"), Vector3.zero, Quaternion.identity);

            player.Transform = PlayerGo.transform;
            player.controller = PlayerGo.GetComponent<CharacterController>();
            player.rigidbody = PlayerGo.GetComponent<Rigidbody>();
            player.MoveSpeed = 10f;
            player.RotateSpeed = 10f;
        }
    }
}