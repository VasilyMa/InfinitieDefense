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

            var PlayerGo = GameObject.Instantiate(_state.Value.PlayerStorage.GetPlayerByID("1level"), new Vector3(0,2,-10), Quaternion.identity);

            player.Transform = PlayerGo.transform;
            player.playerMB = PlayerGo.GetComponent<PlayerMB>();
            player.rigidbody = PlayerGo.GetComponent<Rigidbody>();
            player.MoveSpeed = 10f;
            player.RotateSpeed = 1f;
            player.damage = _state.Value.PlayerStorage.GetDamageByID("1level");
            player.ResHolderTransform = PlayerGo.transform.GetChild(2).transform;
            player.animator = PlayerGo.GetComponent<Animator>();
            player.playerMB.Init(systems.GetWorld(), systems.GetShared<GameState>());
            var colliderChecker = PlayerGo.GetComponent<ColliderChecker>();
            colliderChecker.Init(systems.GetWorld(), systems.GetShared<GameState>());
            _cooldownMining.Value.Add(_state.Value.EntityPlayer);
            ref var cooldown = ref _cooldownMining.Value.Get(_state.Value.EntityPlayer);
            cooldown.maxValue = 1f;
            cooldown.currentValue = cooldown.maxValue;
            _reloadPool.Value.Add(_state.Value.EntityPlayer);

            //udalit' eto haxyu posle bilda
            player.AttackMonoBehaviour = PlayerGo.GetComponent<AttackMonoBehaviour>();
            player.AttackMonoBehaviour.Init(_world);
            player.AttackMonoBehaviour.SetEntity(playerEntity);
            player.AttackMonoBehaviour.SetDamageValue(50);
        }
    }
}