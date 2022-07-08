using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Leopotam.EcsLite.Unity.Ugui;
using UnityEngine;
using UnityEngine.Scripting;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
namespace Client {
    sealed class UserInputSystem : EcsUguiCallbackSystem
    {
        readonly EcsFilterInject<Inc<Player>> _filter = default;
        readonly EcsSharedInject<GameState> _state = default;
        readonly EcsPoolInject<InterfaceComponent> _interface = default;

        #region Old
        //Vector2 startPosition = Vector2.zero;
        //Vector2 lastPosition = Vector2.zero;
        //private GameObject _joystick;
        //private GameObject _handlerJoystick;
        //private RectTransform _rectTransform;
        //private float _maxAllowedSize = 10.0f;
        //private float _speed;
        //private Vector2 _direction;
        //private Vector3 moveVector;
        //[Preserve]
        //[EcsUguiDragStartEvent(Idents.Ui.TouchListener, Idents.Worlds.Events)]
        //void OnDownTouchListner(in EcsUguiDragStartEvent evt)
        //{
        //    ref var intComp = ref _interface.Value.Get(_state.Value.EntityInterface);
        //    _joystick = intComp.joystick.transform.GetChild(0).gameObject;
        //    _handlerJoystick = _joystick.transform.GetChild(0).gameObject;
        //    _joystick.GetComponent<Image>().enabled = true;
        //    _handlerJoystick.GetComponent<Image>().enabled = true;
        //    startPosition = evt.Position;
        //    _joystick.transform.position = startPosition;
        //    _rectTransform = _handlerJoystick.GetComponent<RectTransform>();
        //    moveVector = Vector3.zero;
        //    Debug.Log($"Down, {evt.Sender}, {_joystick.gameObject}, {_handlerJoystick}");
        //}
        //[Preserve]
        //[EcsUguiDragMoveEvent(Idents.Ui.TouchListener, Idents.Worlds.Events)]
        //void OnDragTouchListner(in EcsUguiDragMoveEvent evt)
        //{

        //    lastPosition = evt.Position - startPosition;
        //    _rectTransform.localPosition = lastPosition;
        //    float size = lastPosition.magnitude;
        //    if (size > _maxAllowedSize)
        //    {
        //        _speed = 1.0f;
        //        lastPosition = lastPosition / size * _maxAllowedSize;
        //    }
        //    else
        //        _speed = size / _maxAllowedSize;
        //    _direction = lastPosition * 0.1f;

        //    moveVector.x = Horizontal();
        //    moveVector.z = Vertical();
        //    foreach (var entity in _filter.Value)
        //    {
        //        ref var _player = ref _filter.Pools.Inc1.Get(entity);

        //        if (_speed > 0.0f)
        //        {
        //            if (Vector3.Angle(Vector3.forward, moveVector) > 1f || Vector3.Angle(Vector3.forward, moveVector) == 0)
        //            {
        //                Vector3 direct = Vector3.RotateTowards(_player.Transform.forward, moveVector, _player.RotateSpeed, 0.0f);
        //                _player.Transform.rotation = Quaternion.LookRotation(direct);
        //            }
        //        }
        //        _player.rigidbody.velocity = new Vector3(moveVector.x * _player.MoveSpeed, _player.rigidbody.velocity.y, moveVector.z * _player.MoveSpeed);
        //        _player.animator.SetBool("isIdle", false);
        //        _player.animator.SetBool("isRun", true);
        //    }
        //    Debug.Log($"Move, {evt.Sender}");
        //}
        //[Preserve]
        //[EcsUguiDragEndEvent(Idents.Ui.TouchListener, Idents.Worlds.Events)]
        //void OnUpTouchListner(in EcsUguiDragEndEvent evt)
        //{
        //    _rectTransform.localPosition = Vector2.zero;
        //    _direction = Vector2.zero;
        //    moveVector = Vector3.zero;
        //    foreach (var entity in _filter.Value)
        //    {
        //        ref var _player = ref _filter.Pools.Inc1.Get(entity);
        //        _player.animator.SetBool("isRun", false);
        //        if (!_player.animator.GetBool("isMining"))
        //            _player.animator.SetBool("isIdle", true);
        //        _player.rigidbody.velocity = Vector3.zero;
        //    }
        //    _joystick.GetComponent<Image>().enabled = false;
        //    _handlerJoystick.GetComponent<Image>().enabled = false;

        //    Debug.Log($"Up, {evt.Sender}, {moveVector}");
        //}
        #endregion

        [Preserve]
        [EcsUguiClickEvent(Idents.Ui.Restart, Idents.Worlds.Events)]
        void OnClickRestart(in EcsUguiClickEvent evt)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            Debug.Log("Restart!", evt.Sender);
        }
        [Preserve]
        [EcsUguiClickEvent(Idents.Ui.NextLevel, Idents.Worlds.Events)]
        void OnClickNextLevel(in EcsUguiClickEvent evt)
        {
            Time.timeScale = 1;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            Debug.Log("Restart!", evt.Sender);
            int index = 0;
            if (SceneManager.GetActiveScene().buildIndex + 1 > SceneManager.sceneCountInBuildSettings - 1)
            {
                index = 1;
            }
            else
            {
                index = SceneManager.GetActiveScene().buildIndex + 1;
            }
            _state.Value.Saves.SaveSceneNumber(index);
            _state.Value.Saves.SaveLevel(_state.Value.Saves.LVL + 1);
            SceneManager.LoadScene(index);
            Debug.Log("NextLevel");
        }
        [Preserve]
        [EcsUguiClickEvent(Idents.Ui.Retry, Idents.Worlds.Events)]
        void OnClickRetry(in EcsUguiClickEvent evt)
        {
            Time.timeScale = 1;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            Debug.Log("Retry!");
        }
        [Preserve]
        [EcsUguiClickEvent(Idents.Ui.Play, Idents.Worlds.Events)]
        void OnClickPlay(in EcsUguiClickEvent evt)
        {
            ref var interfaceComponent = ref _interface.Value.Get(_state.Value.EntityInterface);
            _state.Value.SetNextWave();
            interfaceComponent.waveCounter.GetComponent<WaveCounterMB>().UpdateWave();
            interfaceComponent.progressbar.SetActive(true);
            //interfaceComponent.gamePanel.SetActive(false);
            Debug.Log("Play!");
        }
    }
}
