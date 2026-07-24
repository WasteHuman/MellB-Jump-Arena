using Core.Gameplay.Player;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Animations.GameScreen
{
    [RequireComponent(typeof(RectTransform))]
    public class BallOnPlatformAnimation : MonoBehaviour
    {
        [Header("Animation Setup")]
        [SerializeField] private float _hoverUpOffset = 5f;
        [SerializeField] private float _hoverAnimationDuration = 1f;
        [SerializeField] private RectTransform _ballRect;

        [Space(5), Header("Visual Setup")]
        [SerializeField] private Image _ballImage;

        private Tween _hoverTween;

        private void Awake()
        {
            if(_ballRect == null)
                _ballRect = GetComponent<RectTransform>();

            StartAnimation();
        }

        private void Start()
        {
            _ballImage.sprite = PlayerSkinsController.GetCurrentSprite();
            PlayerSkinsController.OnSkinChanged += HandleChangedSkin;
        }

        private void OnDestroy()
        {
            PlayerSkinsController.OnSkinChanged -= HandleChangedSkin;
        }

        public void StartAnimation()
        {
            StopAnimation();

            var targetPosition = _ballRect.localPosition.y + _hoverUpOffset;

            _hoverTween = _ballRect
                .DOLocalMoveY(targetPosition, _hoverAnimationDuration)
                .SetEase(Ease.InOutSine)
                .SetLoops(-1, LoopType.Yoyo);
        }

        public void StopAnimation() => _hoverTween?.Kill();

        private void HandleChangedSkin(Sprite skin) => _ballImage.sprite = skin;
    }
}