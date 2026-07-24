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

        public event Action OnSettingsButtonClick;

        private void Start()
        {
            _settingsButton.OnButtonClick += HandleSettingsButtonClick;
            _storeButton.OnButtonClick += HandleStoreButtonClick;
            _playButton.OnButtonClick += HandlePlayButtonClick;
            _ballsButton.OnButtonClick += HandleBallsButtonClick;
            _achievementsButton.OnButtonClick += HandleAchievementsButtonClick;
        }

        private void OnDestroy()
        {
            _settingsButton.OnButtonClick -= HandleSettingsButtonClick;
            _storeButton.OnButtonClick -= HandleStoreButtonClick;
            _playButton.OnButtonClick -= HandlePlayButtonClick;
            _ballsButton.OnButtonClick -= HandleBallsButtonClick;
            _achievementsButton.OnButtonClick -= HandleAchievementsButtonClick;
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
            Debug.Log("Store Button clicked");
        }

        private void HandleSettingsButtonClick() => OnSettingsButtonClick?.Invoke();
    }
}