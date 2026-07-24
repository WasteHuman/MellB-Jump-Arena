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
        [SerializeField] private BonusSystemController _bonusSystemController;

        [Space(5), Header("Views")]
        [SerializeField] private WinPopupView _winPopupView;
        [SerializeField] private PauseWindowView _pauseWindowView;
        [SerializeField] private List<Screen> _activeScreens = new();

        private bool _isPlayerLose = false;

        private float _lowerBound;

        private void Awake()
        {
            _camera.OnHighestYChanged += HandleChangedHighestY;
            _player.OnPlayerDied += HandlePlayerDie;

            _pauseWindowView.OnPauseClosed += UnpauseGame;
            _pauseWindowView.OnPauseOpened += PauseGame;

            _lowerBound = _camera.CurrentLowerBound;
        }

        private void OnDestroy()
        {
            _camera.OnHighestYChanged -= HandleChangedHighestY;
            _player.OnPlayerDied -= HandlePlayerDie;

            _pauseWindowView.OnPauseClosed -= UnpauseGame;
            _pauseWindowView.OnPauseOpened -= PauseGame;
        }

        private void Update()
        {
            CheckPlayerLose();
        }

        private void PauseGame()
        {
            _player.FreezePlayer();
            _bonusSystemController.Pause();
        }

        private void UnpauseGame()
        {
            _player.UnfreezePlayer();
            _bonusSystemController.Unpause();
        }

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

            _playerStatisticsController.SaveBestScore();
        }

        private void HandleChangedHighestY(float value) => _lowerBound = value;
        private void HandlePlayerDie() => PlayerLose();
    }
}