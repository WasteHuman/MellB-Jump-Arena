using Core;
using Core.Gameplay;
using DG.Tweening;
using System;
using TMPro;
using UI.Other;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI.Views
{
    public class WinPopupView : Window
    {
        [SerializeField] private RectTransform _popupRect;
        [SerializeField] private ActionButton _playAgainButton;

        [Space(5), Header("Labels Setup")]
        [SerializeField] private TextMeshProUGUI _bestScoreLabel;
        [SerializeField] private TextMeshProUGUI _scoreLabel;
        [SerializeField] private TextMeshProUGUI _coinsCollectedLabel;

        [Space(5), Header("Animations Setup")]
        [SerializeField] private float _toggleAnimationDuration = 0.5f;

        private Tween _openTween;
        private Tween _closeTween;

        private void Awake()
        {
            _playAgainButton.OnButtonClick += HandlePlayAgainButtonClick;
        }

        private void OnDestroy()
        {
            _playAgainButton.OnButtonClick -= HandlePlayAgainButtonClick;

            _openTween?.Kill();
            _closeTween?.Kill();
        }

        public void UpdateUI(float score, float bestScore, float coinsCollected)
        {
            _coinsCollectedLabel.text = $"Coins Collected:{coinsCollected}";
            _bestScoreLabel.text = $"Best Score: {bestScore}";
            _scoreLabel.text = $"Score: {score}";
        }

        public override void Open(Action onComplete = null)
        {
            base.Open();
            _popupRect.localScale = Vector2.zero;

            _openTween?.Kill();

            _openTween = _popupRect
                .DOScale(Vector2.one, _toggleAnimationDuration)
                .SetEase(Ease.InOutSine);
        }

        public override void Close(Action onComplete = null)
        {
            _closeTween?.Kill();

            _closeTween = _popupRect
                .DOScale(Vector2.zero, _toggleAnimationDuration)
                .SetEase(Ease.OutSine)
                .OnComplete(() =>
                {
                    base.Close();
                    onComplete?.Invoke();
                });
        }

        private void HandlePlayAgainButtonClick()
        {
            EconomyController.Instance.AddCoins(EconomyController.Instance.GetCollectedCoins());
            EconomyController.Instance.ResetCollectedCoins();
            Close(() => SceneManager.LoadSceneAsync(SceneNames.GAME));
        }
    }
}