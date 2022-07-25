using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client {
    sealed class TutorialSystem : IEcsRunSystem {
        readonly EcsSharedInject<GameState> _state = default;
        readonly EcsFilterInject<Inc<TutorialComponent>> _filter = default;
        readonly EcsPoolInject<TutorialComponent> _tutorialPool = default;
        public void Run(EcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                ref var tutorialComp = ref _tutorialPool.Value.Get(entity);
                if (tutorialComp.TutorialStage == 0)
                {
                    Time.timeScale = 0;
                    tutorialComp.HandObject.SetActive(true);
                    tutorialComp.HoleObject.SetActive(true);
                    tutorialComp.Animator.SetBool("isSwipe", true);
                }
            }
        }
    }
}