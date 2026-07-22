using DG.Tweening;
using UnityEngine;

namespace UI.Animations.GameScreen
{
    [RequireComponent(typeof(RectTransform))]
    public class BallOnPlatformAnimation : MonoBehaviour
    {
        [Header("Animation Setup")]
        [SerializeField] private float _hoverUpOffset = 5f;
        [SerializeField] private float _hoverAnimationDuration = 1f;
        [SerializeField] private RectTransform _ballRect;

        private Tween _hoverTween;

        private void Awake()
        {
            if(_ballRect == null)
                _ballRect = GetComponent<RectTransform>();

            StartAnimation();
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
    }
}