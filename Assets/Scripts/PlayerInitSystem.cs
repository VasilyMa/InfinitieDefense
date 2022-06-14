using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client {
    sealed class PlayerInitSystem : IEcsInitSystem {
        readonly EcsPoolInject<Player> _playerPool = default;
        readonly EcsPoolInject<CooldownComponent> _cooldownMining = default;
        readonly EcsPoolInject<RealoadComponent> _reloadPool = default;
        readonly EcsWorldInject _world = default;
        readonly EcsSharedInject<GameState> _state = default;
        public void Init (EcsSystems systems) 
        {
            
            var playerEntity = _playerPool.Value.GetWorld().NewEntity();
            _state.Value.EntityPlayer = playerEntity;
            ref var player = ref _playerPool.Value.Add (playerEntity);

            var PlayerGo = GameObject.Instantiate(_state.Value.PlayerStorage.GetPlayerByID("1level"), Vector3.zero, Quaternion.identity);

            player.Transform = PlayerGo.transform;
            player.controller = PlayerGo.GetComponent<CharacterController>();
            player.rigidbody = PlayerGo.GetComponent<Rigidbody>();
            player.MoveSpeed = 5f;
            player.RotateSpeed = 1f;
            player.ResHolderTransform = PlayerGo.transform.GetChild(0).transform;
            player.animator = PlayerGo.GetComponent<Animator>();
            var colliderChecker = PlayerGo.GetComponent<ColliderChecker>();
            colliderChecker.Init(systems.GetWorld(), systems.GetShared<GameState>());
            _cooldownMining.Value.Add(_state.Value.EntityPlayer);
            ref var cooldown = ref _cooldownMining.Value.Get(_state.Value.EntityPlayer);
            cooldown.maxValue = 1f;
            cooldown.currentValue = cooldown.maxValue;
            _reloadPool.Value.Add(_state.Value.EntityPlayer);
        }
    }
}