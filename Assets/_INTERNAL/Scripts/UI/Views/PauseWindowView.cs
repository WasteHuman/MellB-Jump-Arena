using Core;
using DG.Tweening;
using System;
using UI.Other;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI.Views
{
    public class PauseWindowView : Window
    {
        [SerializeField] private RectTransform _popupRect;
        [SerializeField] private ActionButton _backToMainMenuButton;
        [SerializeField] private ActionButton _pauseButton;
        [SerializeField] private ActionButton _resumeButton;

        [Space(5), Header("Animations Setup")]
        [SerializeField] private float _toggleAnimationDuration = 0.5f;

        private Tween _openTween;
        private Tween _closeTween;

        public Action OnPauseOpened;
        public Action OnPauseClosed;

        private void Awake()
        {
            _backToMainMenuButton.OnButtonClick += HandleBackToMainMenuButtonClick;
            _pauseButton.OnButtonClick += HanndlePauseButtonClick;
            _resumeButton.OnButtonClick += HandleResumeButtonClick;
        }

        private void OnDestroy()
        {
            _backToMainMenuButton.OnButtonClick -= HandleBackToMainMenuButtonClick;
            _pauseButton.OnButtonClick -= HanndlePauseButtonClick;
            _resumeButton.OnButtonClick -= HandleResumeButtonClick;

            _openTween?.Kill();
            _closeTween?.Kill();
        }

        public override void Open(Action onComplete = null)
        {
            _popupRect.localScale = Vector2.zero;
            _popupRect.gameObject.SetActive(true);

            _openTween?.Kill();

            _openTween = _popupRect
                .DOScale(Vector2.one, _toggleAnimationDuration)
                .SetEase(Ease.InOutSine)
                .SetUpdate(true)
                .OnComplete(() => onComplete?.Invoke());
        }

        public override void Close(Action onComplete = null)
        {
            _closeTween?.Kill();

            _closeTween = _popupRect
                .DOScale(Vector2.zero, _toggleAnimationDuration)
                .SetEase(Ease.OutSine)
                .SetUpdate(true)
                .OnComplete(() =>
                {
                    _popupRect.gameObject.SetActive(false);
                    onComplete?.Invoke();
                });
        }

        private void HanndlePauseButtonClick() => Open(OnPauseOpened);

        private void HandleResumeButtonClick() => Close(OnPauseClosed);

        private void HandleBackToMainMenuButtonClick() => SceneManager.LoadSceneAsync(SceneNames.MAIN_MENU);
    }
}