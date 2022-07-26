using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using Unity;
namespace Client {
    sealed class OreMiningSystem : IEcsRunSystem {
        readonly EcsWorldInject _world = default;
        readonly EcsSharedInject<GameState> _state = default;
        readonly EcsFilterInject<Inc<OreEventComponent, OreComponent>, Exc<OreMinedTag>> _filter = default;
        readonly EcsFilterInject<Inc<Player>> _filterPlayer = default;
        readonly EcsPoolInject<OreMoveEvent> _movePool = default;
        readonly EcsPoolInject<OreMinedTag> _minedPool = default;
        readonly EcsPoolInject<ViewComponent> _viewPool = default;
        readonly EcsPoolInject<InMiningTag> _miningPool = default;

        readonly EcsPoolInject<VibrationEvent> _vibrationEventPool = default;
        readonly EcsFilterInject<Inc<TutorialComponent>> _tutorPool = default;
        public void Run (EcsSystems systems) {
            foreach (var entity in _filter.Value)
            {
                ref var oreComp = ref _filter.Pools.Inc2.Get(entity);
                ref var oreViewComp = ref _viewPool.Value.Get(entity);
                ref var moveComp = ref _movePool.Value.Add(_world.Value.NewEntity());
                
                var stone = (GameObject)GameObject.Instantiate(Resources.Load("Stone"), new Vector3(oreComp.prefab.transform.position.x, oreComp.prefab.transform.position.y + Random.Range(0.5f, 1.2f), oreComp.prefab.transform.position.z), Quaternion.identity);

                oreComp.OreParts[oreComp.MaxAmount - oreComp.CurrentAmount].SetActive(false);

                oreComp.CurrentAmount--;
                oreViewComp.Animator.SetTrigger("Extract");

                moveComp.stone = stone;
                moveComp.TargetPosition = new Vector3(oreComp.prefab.transform.position.x + Random.Range(-4, 4), oreComp.prefab.transform.position.y, oreComp.prefab.transform.position.z + Random.Range(-4, 4));
                moveComp.Speed = 10f;
                moveComp.outTime = 0.5f;

                _vibrationEventPool.Value.Add(_world.Value.NewEntity()).Vibration = VibrationEvent.VibrationType.LightImpact;

                if (oreComp.CurrentAmount <= 0) 
                { 
                    oreComp.prefab.GetComponent<SphereCollider>().enabled = false;
                    oreComp.prefab.gameObject.SetActive(false);
                    _minedPool.Value.Add(entity);
                    oreComp.respawnTime = 5f;
                    foreach (var entityPlayer in _filterPlayer.Value)
                    {
                        foreach (var item in _tutorPool.Value)
                        {
                            if (_tutorPool.Pools.Inc1.Get(item).TutorialStage == 2)
                            {
                                _tutorPool.Pools.Inc1.Get(item).TutorialStage = 3;
                                _state.Value.Saves.TutorialStage = 3;
                                _state.Value.Saves.SaveTutorial(3);
                            }
                                
                        }
                        _miningPool.Value.Del(entityPlayer);
                    }
                }
                _filter.Pools.Inc1.Del(entity);
            }
        }
    }
}