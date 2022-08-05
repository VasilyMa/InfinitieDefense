using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leopotam.EcsLite;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Client
{
    public class BiomPointsMB : MonoBehaviour
    {
        private EcsWorld _world;
        private GameState _state;
        [SerializeField] private Image _currentBiomImage;
        [SerializeField] private Image _nextBiomImage;
        [SerializeField] private Transform _biomPointHolder;
        [SerializeField] private GameObject _biomPointPrefab;

        public void Init(EcsWorld world, GameState state)
        {
            _world = world;
            _state = state;
        }

        public void InitBiomPoints()
        {
            _currentBiomImage.sprite = _state.CurrentBiom.BiomSprite;
            _nextBiomImage.sprite = _state.CurrentBiom.NextBiomSprite;

            for (int i = 0; i < _state.CurrentBiom.BiomLevels.Count; i++)
            {
                var lvl = _state.CurrentBiom.BiomLevels[i];
                var currentScene = SceneManager.GetActiveScene().buildIndex;
                if (currentScene < lvl)
                {
                    var image = Instantiate(_biomPointPrefab, _biomPointHolder).GetComponent<Image>();
                    image.color = _state.LevelsStorage.AnCompletePointColor;
                }
                else if (currentScene == lvl)
                {
                    var image = Instantiate(_biomPointPrefab, _biomPointHolder).GetComponent<Image>();
                    image.color = _state.LevelsStorage.CyrrentPointColor;
                    image.transform.localScale = new Vector3(1, 1.2f, 1);
                }
                else
                {
                    var image = Instantiate(_biomPointPrefab, _biomPointHolder).GetComponent<Image>();
                    image.color = _state.LevelsStorage.CompletePointColor;
                }
            }
        }
    }
}
