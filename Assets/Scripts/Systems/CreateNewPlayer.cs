using Leopotam.EcsLite;
using UnityEngine;
using Leopotam.EcsLite.Di;

namespace Client
{
    sealed class CreateNewPlayer : IEcsRunSystem
    {
        readonly EcsWorldInject _world = default;

        readonly EcsSharedInject<GameState> _state = default;

        readonly EcsFilterInject<Inc<CreateNewPlayerEvent>> _filter = default;
        readonly EcsFilterInject<Inc<UpgradePlayerPointComponent>> _filterPoint = default;

        readonly EcsPoolInject<CanvasUpgradeComponent> _upgradePoint = default;

        readonly EcsPoolInject<ViewComponent> _viewPool = default;
        readonly EcsPoolInject<Player> _playerPool = default;
        readonly EcsPoolInject<LevelUpEvent> _levelUpPool = default;
        readonly EcsPoolInject<HealthComponent> _healthPool = default;
        readonly EcsPoolInject<DamageComponent> _damagePool = default;
        readonly EcsFilterInject<Inc<UpgradeTimerEvent>> _timerPool = default;
        readonly EcsFilterInject<Inc<TutorialComponent>> _tutorPool = default;
        readonly EcsPoolInject<UpgradeComponent> _upgradePool = default;
        readonly EcsPoolInject<SavesEvent> _savePool = default;
        public void Run (EcsSystems systems) {
            foreach(var entity in _filter.Value)
            {
                ref var upgradePointComp = ref _upgradePoint.Value.Get(_state.Value.TowersEntity[0]);
                foreach (var item in _tutorPool.Value)
                {
                    ref var tutorComp = ref _tutorPool.Pools.Inc1.Get(item);
                    GameObject.Destroy(tutorComp.TutorialCursor);
                    if (tutorComp.TutorialStage <= 7)
                    {
                        tutorComp.TutorialStage = 8;
                        _state.Value.Saves.TutorialStage = 8;
                        _state.Value.Saves.SaveTutorial(8);
                        upgradePointComp.point.SetActive(true);
                    }
                }
                /*if (_state.Value.Saves.TutorialStage < 12)
                {
                    foreach (var item in _timerPool.Value)
                    {
                        _timerPool.Pools.Inc1.Get(item).TimeToUpgrade = 0;
                        _timerPool.Pools.Inc1.Del(item);
                    }
                    _upgradePool.Value.Del(_state.Value.EntityPlayer);
                    _filter.Pools.Inc1.Del(entity);
                    return;
                }*/

                ref var viewComp = ref _viewPool.Value.Get(entity);
                ref var playerComp = ref _playerPool.Value.Get(entity);
                ref var damageComponent = ref _damagePool.Value.Get(entity);
                ref var healthComponent = ref _healthPool.Value.Get(entity);
                
                _state.Value.CurrentPlayerID = _state.Value.PlayerStorage.GetNextIdByID(_state.Value.CurrentPlayerID);
                //Debug.Log("dfdfdfdfd " + _state.Value.CurrentPlayerID);
                damageComponent.Value = _state.Value.PlayerStorage.GetDamageByID(_state.Value.CurrentPlayerID);

                healthComponent.MaxValue = _state.Value.PlayerStorage.GetHealthByID(_state.Value.CurrentPlayerID);
                healthComponent.CurrentValue = healthComponent.MaxValue;
                viewComp.Healthbar.SetMaxHealth(healthComponent.MaxValue);
                viewComp.Healthbar.SetHealth(healthComponent.CurrentValue);

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
                levelPop.LevelPopUp.GetComponent<LevelPopupMB>().UpdateInfoDamageHealth(_state.Value.PlayerStorage.GetDamageByID(_state.Value.CurrentPlayerID), _state.Value.PlayerStorage.GetHealthByID(_state.Value.CurrentPlayerID));
                levelPop.Text = levelPop.LevelPopUp.GetComponent<LevelPopupMB>().GetText();
                levelPop.target = new Vector3(viewComp.GameObject.transform.position.x, viewComp.GameObject.transform.position.y + 10f, viewComp.GameObject.transform.position.z);
                levelPop.TimeOut = 2f;
                levelPop.LevelPopUp.SetActive(true);

                

                viewComp.SkinnedMeshRenderer.sharedMesh = _state.Value.PlayerStorage.GetMeshByID(_state.Value.CurrentPlayerID);
                foreach (var item in _timerPool.Value)
                {
                    _timerPool.Pools.Inc1.Get(item).TimeToUpgrade = 0;
                    _timerPool.Pools.Inc1.Del(item);
                }
                _savePool.Value.Add(_world.Value.NewEntity());
                _upgradePool.Value.Del(_state.Value.EntityPlayer);
                _filter.Pools.Inc1.Del(entity);
            }
        }
    }
}