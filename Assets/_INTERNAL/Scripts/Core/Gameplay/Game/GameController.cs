using Core.Gameplay.Player;
using System.Collections.Generic;
using UI.Views;
using UnityEngine;
using Screen = UI.Other.Screen;

namespace Core.Gameplay.Game
{
    public class GameController : MonoBehaviour
    {
        [Header("Game Setup")]
        [SerializeField] private CameraFollower _camera;
        [SerializeField] private PlayerController _player;
        [SerializeField] private PlayerStatisticsController _playerStatisticsController;

        [Space(5), Header("Views")]
        [SerializeField] private WinPopupView _winPopupView;
        [SerializeField] private List<Screen> _activeScreens = new();

        private bool _isPlayerLose = false;

        private float _lowerBound;

        private void Awake()
        {
            _camera.OnHighestYChanged += HandleChangedHighestY;
            _player.OnPlayerDied += HandlePlayerDie;

            _lowerBound = _camera.CurrentLowerBound;
        }

        private void OnDestroy()
        {
            _camera.OnHighestYChanged -= HandleChangedHighestY;
            _player.OnPlayerDied -= HandlePlayerDie;
        }

        private void Update()
        {
            CheckPlayerLose();
        }

        public void PauseGame() => Time.timeScale = 0f;

        public void UnpauseGame() => Time.timeScale = 1f;

        private void CheckPlayerLose()
        {
            if (_isPlayerLose)
                return;

            if (_player.transform.position.y < _lowerBound)
                PlayerLose();
        }

        private void PlayerLose()
        {
            foreach (var screen in _activeScreens)
                screen.Close();

            _winPopupView
                .UpdateUI(
                _playerStatisticsController.Score,
                _playerStatisticsController.BestScore,
                EconomyController.Instance.GetCollectedCoins());
            _winPopupView.Open();
            _isPlayerLose = true;
        }

        private void HandleChangedHighestY(float value) => _lowerBound = value;
        private void HandlePlayerDie() => PlayerLose();
    }
}