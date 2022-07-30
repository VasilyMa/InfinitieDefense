using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Client {
    sealed class SavesSystem : IEcsRunSystem {
        readonly EcsSharedInject<GameState> _state = default;
        readonly EcsFilterInject<Inc<SavesEvent>> _filter = default;
        public void Run (EcsSystems systems) {
            foreach(var entity in _filter.Value)
            {
                _state.Value.Saves.SaveCoin(_state.Value.CoinCount);
                //_state.Value.Saves.SaveRock(_state.Value.RockCount);
                //_state.Value.Saves.SaveTowerID(_state.Value.DefenseTowers);
                //_state.Value.Saves.SaveUpgrades(_state.Value.TowersUpgrade);
                _state.Value.Saves.SavePlayerID(_state.Value.CurrentPlayerID);
                _state.Value.Saves.SavePlayerUpgrade(_state.Value.PlayerExperience);
                _state.Value.Saves.SaveCurrentWave(_state.Value.GetCurrentWave());
                
                _filter.Pools.Inc1.Del(entity);
            }
        }
    }
}