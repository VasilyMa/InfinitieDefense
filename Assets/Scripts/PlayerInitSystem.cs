using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client {
    sealed class PlayerInitSystem : IEcsInitSystem {
        readonly EcsPoolInject<Player> _playerPool = default;
        public void Init (EcsSystems systems) 
        {
            var world = systems.GetWorld ();
            var playerEntity = _playerPool.Value.GetWorld().NewEntity();
            ref var player = ref world.GetPool<Player>().Add (playerEntity);


            var PlayerPrefab = Resources.Load("Player");
            var PlayerGo = (GameObject)Object.Instantiate(PlayerPrefab, Vector3.zero, Quaternion.identity);

            player.Transform = PlayerGo.transform;
            player.controller = PlayerGo.GetComponent<CharacterController>();
            player.rigidbody = PlayerGo.GetComponent<Rigidbody>();
            player.MoveSpeed = 10f;
            player.RotateSpeed = 10f;
        }
    }
}