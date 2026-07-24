using DG.Tweening;
using System;
using UI.Other;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI.Views
{
    public class SettingsView : Window
    {
        [Header("Buttons")]
        [SerializeField] private Button _closeSettingsButton;
        [SerializeField] private Button _toggleNotifications;
        [SerializeField] private Button _toggleVibrations;

        [Space(5), Header("Toogle Sprites")]
        [SerializeField] private Sprite _onSprite;
        [SerializeField] private Sprite _offSprite;

        [Space(5), Header("Animation Setup")]
        [SerializeField] private float _toggleAnimationDuration = 0.5f;

        [SerializeField] private RectTransform _rectTransform;

        private UnityAction _closeAction;

        private Image _notificationsImage;
        private Image _vibrationsImage;

        private Tween _openTween;
        private Tween _closeTween;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();

            _closeAction = () => Close();

            _closeSettingsButton.onClick.AddListener(_closeAction);
            _rectTransform.localScale = Vector3.zero;

            _toggleNotifications.onClick.AddListener(HandleToggleNotifications);
            _toggleVibrations.onClick.AddListener(HandleToggleVibrations);
        }

        private void Start()
        {
            _notificationsImage = _toggleNotifications.gameObject.GetComponent<Image>();
            _vibrationsImage = _toggleVibrations.gameObject.GetComponent<Image>();

            InitToggles();
        }

        private void OnDestroy()
        {
            _closeSettingsButton.onClick.RemoveListener(_closeAction);

            _openTween?.Kill();
            _closeTween?.Kill();

            _toggleNotifications.onClick.RemoveListener(HandleToggleNotifications);
            _toggleVibrations.onClick.RemoveListener(HandleToggleVibrations);
        }

        public override void Open(Action onComplete = null)
        {
            gameObject.SetActive(true);

            _openTween?.Kill();

            _openTween = _rectTransform
                .DOScale(1f, _toggleAnimationDuration)
                .SetEase(Ease.OutSine);
        }

        public override void Close(Action onComplete = null)
        {
            _closeTween?.Kill();

            _closeTween = _rectTransform
                .DOScale(0f, _toggleAnimationDuration)
                .SetEase(Ease.InOutSine)
                .OnComplete(() =>
                {
                    gameObject.SetActive(false);
                });
        }

        private void InitToggles()
        {
            if (PlayerPrefs.GetInt("Notifications") == 1)
            {
                _notificationsImage.sprite = _offSprite;
                PlayerPrefs.SetInt("Notifications", 0);
            }
            else
                _notificationsImage.sprite = _onSprite;

            if (PlayerPrefs.GetInt("Vibrations") == 1)
            {
                _vibrationsImage.sprite = _offSprite;
                PlayerPrefs.SetInt("Vibrations", 0);
            }
            else
            {
                _vibrationsImage.sprite = _onSprite;
                PlayerPrefs.SetInt("Vibrations", 1);
            }
        }

        private void HandleToggleNotifications()
        {
            if(PlayerPrefs.GetInt("Notifications") == 1)
            {
                _notificationsImage.sprite = _offSprite;
                PlayerPrefs.SetInt("Notifications", 0);
            }
            else
            {
                _notificationsImage.sprite = _onSprite;
                PlayerPrefs.SetInt("Notifications", 1);
            }
        }

        private void HandleToggleVibrations()
        {
            if (PlayerPrefs.GetInt("Vibrations") == 1)
            {
                _vibrationsImage.sprite = _offSprite;
                PlayerPrefs.SetInt("Vibrations", 0);
            }
            else
            {
                _vibrationsImage.sprite = _onSprite;
                PlayerPrefs.SetInt("Vibrations", 1);
            }
        }
    }
}