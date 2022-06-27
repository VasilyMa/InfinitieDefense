using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leopotam.EcsLite;
namespace Client
{
    public class PlayerMB : MonoBehaviour
    {
        private GameState _state;
        private EcsPool<OreEventComponent> _oreEventPool;
        private EcsPool<Player> _playerPool;
        private EcsPool<InterfaceComponent> _interfacePool;
        private EcsWorld _world;
        private int _entity;
        private GameObject _target;

        public void Init(EcsWorld world, GameState state)
        {
            _state = state;
            _oreEventPool = world.GetPool<OreEventComponent>();
            _playerPool = world.GetPool<Player>();
            _interfacePool = world.GetPool<InterfaceComponent>();
            _world = world;
        }
        public void InitMiningEvent(int entity, GameObject target)
        {
            _target = target;
            _entity = entity;
        }
        public void MiningEvent()
        {
            if (_target != null && _target.activeSelf)
            {
                ref var oreEvent = ref _oreEventPool.Add(_entity);
            }
        }
        private void FixedUpdate()
        {
            ref var player = ref _playerPool.Get(_state.EntityPlayer);
            ref var interfaceComp = ref _interfacePool.Get(_state.EntityInterface);
            var _joystick = interfaceComp._joystick;
            player.rigidbody.velocity = new Vector3(_joystick.Horizontal * player.MoveSpeed, player.rigidbody.velocity.y, _joystick.Vertical * player.MoveSpeed);
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
        }
    }
}
