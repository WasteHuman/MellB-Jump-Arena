using Core;
using DG.Tweening;
using System;
using UI.Other;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class ButtonsController : MonoBehaviour
    {
        [SerializeField] private ActionButton _settingsButton;
        [SerializeField] private ActionButton _storeButton;
        [SerializeField] private ActionButton _playButton;
        [SerializeField] private ActionButton _ballsButton;
        [SerializeField] private ActionButton _achievementsButton;
        [SerializeField] private ActionButton _wheelOfLuckButton;

        public event Action OnSettingsButtonClick;

        private void Start()
        {
            _settingsButton.OnButtonClick += HandleSettingsButtonClick;
            _storeButton.OnButtonClick += HandleStoreButtonClick;
            _playButton.OnButtonClick += HandlePlayButtonClick;
            _ballsButton.OnButtonClick += HandleBallsButtonClick;
            _achievementsButton.OnButtonClick += HandleAchievementsButtonClick;
            _wheelOfLuckButton.OnButtonClick += HandleWheelOfLuckButtonClick;
        }

        private void OnDestroy()
        {
            _settingsButton.OnButtonClick -= HandleSettingsButtonClick;
            _storeButton.OnButtonClick -= HandleStoreButtonClick;
            _playButton.OnButtonClick -= HandlePlayButtonClick;
            _ballsButton.OnButtonClick -= HandleBallsButtonClick;
            _achievementsButton.OnButtonClick -= HandleAchievementsButtonClick;
            _wheelOfLuckButton.OnButtonClick -= HandleWheelOfLuckButtonClick;
        }

        private void HandleAchievementsButtonClick()
        {
            Debug.Log("Achievements Button clicked");
        }

        private void HandleBallsButtonClick()
        {
            Debug.Log("Balls Button clicked");
        }

        private void HandlePlayButtonClick()
        {
            SceneManager.LoadSceneAsync(SceneNames.GAME);
            DOTween.KillAll();
            Debug.Log("Play Button clicked");
        }

        private void HandleStoreButtonClick()
        {
            SceneManager.LoadSceneAsync(SceneNames.STORE);
            DOTween.KillAll();
            Debug.Log("Store Button clicked");
        }

        private void HandleWheelOfLuckButtonClick()
        {
            SceneManager.LoadSceneAsync(SceneNames.WHEEL_OF_LUCK);
            DOTween.KillAll();
        }

        private void HandleSettingsButtonClick() => OnSettingsButtonClick?.Invoke();
    }
}