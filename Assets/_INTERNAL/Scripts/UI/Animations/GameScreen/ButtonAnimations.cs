using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Animations.GameScreen
{
    [RequireComponent(typeof(Button))]
    [RequireComponent(typeof(RectTransform))]
    public class ButtonAnimations : MonoBehaviour
    {
        [SerializeField] private float _clickAnimationDuration = 0.25f;
        [SerializeField] private Vector2 _clickedScale = Vector2.one;

        private RectTransform _rectTransform;
        private Button _button;

        private Tween _clickTween;

        private void Awake()
        {
            _button = GetComponent<Button>();
            _rectTransform = GetComponent<RectTransform>();

            _button.onClick.AddListener(HandleButtonClick);
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveListener(HandleButtonClick);
        }

        public void StopAnimations()
        {
            _clickTween?.Kill();
        }

        private void HandleButtonClick()
        {
            StopAnimations();

            _clickTween = _rectTransform
                .DOScale(_clickedScale, _clickAnimationDuration)
                .SetEase(Ease.InOutSine)
                .SetLoops(1, LoopType.Yoyo);
        }
    }
}