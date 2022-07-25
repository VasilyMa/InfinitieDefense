using Leopotam.EcsLite;
using UnityEngine;
using Leopotam.EcsLite.Di;

namespace Client {
    sealed class CreateNewPlayer : IEcsRunSystem {
        readonly EcsWorldInject _world = default;
        readonly EcsSharedInject<GameState> _state = default;
        readonly EcsFilterInject<Inc<CreateNewPlayerEvent>> _filter = default;
        readonly EcsPoolInject<ViewComponent> _viewPool = default;
        readonly EcsPoolInject<Player> _playerPool = default;
        readonly EcsFilterInject<Inc<UpgradePlayerPointComponent>> _filterPoint = default;
        readonly EcsPoolInject<LevelUpEvent> _levelUpPool = default;
        public void Run (EcsSystems systems) {
            foreach(var entity in _filter.Value)
            {
                ref var viewComp = ref _viewPool.Value.Get(entity);
                ref var playerComp = ref _playerPool.Value.Get(entity);

                _state.Value.CurrentPlayerID = _state.Value.PlayerStorage.GetNextIdByID(_state.Value.CurrentPlayerID);
                //Debug.Log("dfdfdfdfd " + _state.Value.CurrentPlayerID);
                playerComp.damage = _state.Value.PlayerStorage.GetDamageByID(_state.Value.CurrentPlayerID);
                playerComp.health = _state.Value.PlayerStorage.GetHealthByID(_state.Value.CurrentPlayerID);
                viewComp.UpgradeParticleSystem.Play();
                //viewComp.SkinnedMeshRenderer.sharedMesh = _state.Value.PlayerStorage.GetMeshByID(_state.Value.CurrentPlayerID);
                foreach (var item in _filterPoint.Value)
                {
                    _filterPoint.Pools.Inc1.Get(item).Point.GetComponent<PlayerUpgradePointMB>().
                        UpdateLevelInfo(_state.Value.PlayerStorage.
                        GetUpgradeByID(_state.Value.CurrentPlayerID), _state.Value.PlayerExperience);
                }
                //viewComp.Level.UpdateLevel(_state.Value.PlayerStorage.GetLevelByID(_state.Value.CurrentPlayerID));

                viewComp.LevelPopup = viewComp.GameObject.transform.GetChild(1).transform.GetChild(3).transform.gameObject;
                viewComp.LevelPopup.GetComponent<LevelPopupMB>().UpdateLevel(_state.Value.PlayerStorage.GetLevelByID(_state.Value.CurrentPlayerID));
                viewComp.LevelPopup.GetComponent<LevelPopupMB>().Init(systems.GetWorld(), systems.GetShared<GameState>());
                ref var levelPop = ref _levelUpPool.Value.Add(_world.Value.NewEntity());
                levelPop.LevelPopUp = viewComp.LevelPopup;
                levelPop.LevelPopUp.transform.position = new Vector3(viewComp.GameObject.transform.position.x, viewComp.GameObject.transform.position.y + 2f, viewComp.GameObject.transform.position.z);
                levelPop.LevelPopUp.GetComponent<LevelPopupMB>().UpdateLevel(_state.Value.PlayerStorage.GetLevelByID(_state.Value.CurrentPlayerID));
                levelPop.Text = levelPop.LevelPopUp.GetComponent<LevelPopupMB>().GetText();
                levelPop.target = new Vector3(viewComp.GameObject.transform.position.x, viewComp.GameObject.transform.position.y + 10f, viewComp.GameObject.transform.position.z);
                levelPop.TimeOut = 2f;
                levelPop.LevelPopUp.SetActive(true);

                viewComp.SkinnedMeshRenderer.sharedMesh = _state.Value.PlayerStorage.GetMeshByID(_state.Value.CurrentPlayerID);

                _filter.Pools.Inc1.Del(entity);
            }
        }
    }
}