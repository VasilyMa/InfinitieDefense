using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leopotam.EcsLite;
namespace Client
{
    public class PlayerMB : MonoBehaviour
    {
        private GameState _state;
        private EcsPool<OreEventComponent> _orePool;
        private EcsPool<CombatEventComponent> _combatPool;
        //private EcsPool<FightingComponent> _fightPool;
        private EcsWorld _world;
        private int _entity;
        private GameObject _target;

        public void Init(EcsWorld world, GameState state)
        {
            _state = state;
            _orePool = world.GetPool<OreEventComponent>();
            _combatPool = world.GetPool<CombatEventComponent>();
            _world = world;
        }
        public void InitMiningEvent(int entity, GameObject target)
        {
            _target = target;
            _entity = entity;
        }
        public void InitCombatEvent(int entity, GameObject target)
        {
            _entity = entity;
            _target = target;
        }
        public void MiningEvent()
        {
            if (_target != null)
            {
                ref var oreEvent = ref _orePool.Add(_entity);
            }
        }
        public void DamageEvent()
        {
            if (_target != null)
            {
                ref var combatEvent = ref _combatPool.Add(_entity);
            }    
        }
    }
}
