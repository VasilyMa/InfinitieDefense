using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leopotam.EcsLite;
using UnityEngine.UI;

namespace Client
{
    public class CountdownWaveMB : MonoBehaviour
    {
        private EcsWorld _world;
        private GameState _state;
        [SerializeField] private GameObject _timerObject;
        [SerializeField] private float _time;
        [SerializeField] private Text _timer;
        [SerializeField] private bool _timerOn = false;
        [SerializeField] private Text _textAmount;
        private EcsPool<CountdownWaveComponent> _countdownPool = default;
        private EcsPool<TutorialComponent> _tutorialnPool = default;
        private EcsPool<InterfaceComponent> _interfacePool = default;

        private EcsPool<WinEvent> _winPool = default;
        public void Init(EcsWorld world, GameState state)
        {
            _world = world;
            _state = state;
            _countdownPool = _world.GetPool<CountdownWaveComponent>();
            _tutorialnPool = _world.GetPool<TutorialComponent>();
            _interfacePool = _world.GetPool<InterfaceComponent>();
            _winPool = _world.GetPool<WinEvent>();
        }
        private void Update()
        {
            if (_time == 0)
                _timerObject.SetActive(false);
            else
                _timerObject.SetActive(true);
        }
        public void Countdown()
        {
            if (_timerOn)
            {
                if (_time > 0)
                {
                    _time -= Time.deltaTime;
                    UpdateTimer(_time);
                }
                else
                {
                    //to do start wave
                    if (_state.isWave)
                    {
                        _state.SetNextWave();
                        _interfacePool.Get(_state.EntityInterface)._waveCounter.GetComponent<CounterMB>().ChangeCount(_state.GetCurrentWave());
                    }
                    _time = 0;
                    _timerOn = false;
                    if (!_state.isWave)
                    {
                        _interfacePool.Get(_state.EntityInterface)._waveCounter.GetComponent<CounterMB>().ChangeCount(_state.GetCurrentWave());
                        _winPool.Add(_world.NewEntity());
                    }
                    if (_state.Saves.TutorialStage == 2)
                    {
                        var tutorFilter = _world.Filter<TutorialComponent>().End();
                        foreach (var entity in tutorFilter)
                        {
                            _tutorialnPool.Get(entity).TextHolder.GetComponentInChildren<Text>().text = "Kill all enemies";
                        }
                    }
                    var filter = _world.Filter<CountdownWaveComponent>().End();
                    foreach (var entity in filter)
                    {
                        _countdownPool.Del(entity);
                    }
                }
            }
        }
        void UpdateTimer(float currentTime)
        {
            currentTime += 1;
            float seconds = Mathf.FloorToInt(currentTime % 60);
            _timer.text = string.Format("{0:00}", seconds);
        }
        #region Get/Set
        public void SetTimer(float timer)
        {
            _time = timer;
        }
        /// <summary>
        ///  Выключает или включает таймер,
        ///  если значение равно 0
        /// </summary>
        public void SwitcherTurn(bool value) 
        {
            _timerOn = value;
        }
        #endregion Get/Set
        public void SetText(string value)
        {
            _textAmount.text = value;
        }
    }
}
