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
        private EcsPool<CountdownWaveComponent> _countdownPool = default;
        private EcsPool<TutorialComponent> _tutorialnPool = default;
        public void Init(EcsWorld world, GameState state)
        {
            _world = world;
            _state = state;
            _countdownPool = _world.GetPool<CountdownWaveComponent>();
            _tutorialnPool = _world.GetPool<TutorialComponent>();
        }
        private void Update()
        {
            _timerObject.SetActive(_timerOn);
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
                    _state.SetNextWave();
                    _time = 0;
                    _timerOn = false;
                    _state.Saves.TutorialStage = 12;
                    _state.Saves.SaveTutorial(12);
                    if (_state.Saves.TutorialStage == 12)
                    {
                        var tutorFilter = _world.Filter<TutorialComponent>().End();
                        foreach (var entity in tutorFilter)
                        {
                            _tutorialnPool.Get(entity).TutorialStage = 12;
                            if (_tutorialnPool.Has(entity))
                            {
                                _tutorialnPool.Get(entity).TextHolder.SetActive(false);
                                _countdownPool.Del(entity);
                            }
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
    }
}
