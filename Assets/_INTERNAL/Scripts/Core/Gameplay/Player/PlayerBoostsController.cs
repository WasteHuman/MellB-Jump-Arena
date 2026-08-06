using Core.Gameplay.Game;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Gameplay.Player
{
    public class PlayerBoostsController : MonoBehaviour
    {
        [SerializeField] private Transform _playerTransform;
        [SerializeField] private float _shieldDuration = 10f;
        [SerializeField] private float _slowMotionDuration = 10f;
        [SerializeField, Range(0.05f, 1f)] private float _slowMotionTimeScale = 0.5f;
        [SerializeField] private float _scoreX2Duration = 10f;
        [SerializeField] private float _magnetDuration = 10f;
        [SerializeField] private float _magnetRadius = 4f;
        [SerializeField] private LayerMask _collectibleLayerMask = ~0;

        [Space(5), Header("Timers Setup")]
        [SerializeField] private Image _shieldFillTimer;
        [SerializeField] private Image _slowMotionFillTimer;
        [SerializeField] private Image _scoreX2FillTImer;
        [SerializeField] private Image _magnetFillTimer;

        private float _shieldTimer;
        private float _slowMotionTimer;
        private float _scoreX2Timer;
        private float _magnetTimer;
        private float _defaultFixedDeltaTime;

        public bool IsShieldActive => _shieldTimer > 0f;
        public bool IsMagnetActive => _magnetTimer > 0f;
        public float ScoreMultiplier => _scoreX2Timer > 0f ? 2f : 1f;

        private void Awake()
        {
            _defaultFixedDeltaTime = Time.fixedDeltaTime;
        }

        private void OnDestroy()
        {
            DOTween.KillAll();
        }

        private void OnDisable()
        {
            DisableSlowMotion();
        }

        private void Update()
        {
            TickTimers();

            if (IsMagnetActive)
                CollectNearbyItems();
        }

        public bool ActivateBoost(BoostType type)
        {
            if (!PlayerBoostsInventory.TryConsume(type))
                return false;

            switch (type)
            {
                case BoostType.Shield:
                    _shieldTimer = _shieldDuration;
                    break;
                case BoostType.SlowMotion:
                    _slowMotionTimer = _slowMotionDuration;
                    EnableSlowMotion();
                    break;
                case BoostType.ScoreX2:
                    _scoreX2Timer = _scoreX2Duration;
                    break;
                case BoostType.Magnet:
                    _magnetTimer = _magnetDuration;
                    break;
            }

            return true;
        }

        public bool TryUseShield()
        {
            if (!IsShieldActive)
                return false;

            _shieldTimer = 0f;
            return true;
        }

        private void TickTimers()
        {
            _shieldTimer = Mathf.Max(0f, _shieldTimer - Time.unscaledDeltaTime);
            _scoreX2Timer = Mathf.Max(0f, _scoreX2Timer - Time.unscaledDeltaTime);
            _magnetTimer = Mathf.Max(0f, _magnetTimer - Time.unscaledDeltaTime);

            _shieldFillTimer.DOFillAmount(Mathf.Clamp01(_shieldTimer / _shieldDuration), 1f)
                .OnComplete(() => _shieldFillTimer.fillAmount = 1f);
            _scoreX2FillTImer.DOFillAmount(Mathf.Clamp01(_scoreX2Timer / _scoreX2Duration), 1f)
                .OnComplete(() => _scoreX2FillTImer.fillAmount = 1f);
            _magnetFillTimer.DOFillAmount(Mathf.Clamp01(_magnetTimer / _magnetDuration), 1f)
                .OnComplete(() => _magnetFillTimer.fillAmount = 1f);

            if (_slowMotionTimer <= 0f)
                return;

            _slowMotionTimer = Mathf.Max(0f, _slowMotionTimer - Time.unscaledDeltaTime);
            _slowMotionFillTimer.DOFillAmount(Mathf.Clamp01(_slowMotionTimer / _slowMotionDuration), 1f)
                .OnComplete(() => _slowMotionFillTimer.fillAmount = 1f);
            if (_slowMotionTimer <= 0f)
                DisableSlowMotion();
        }

        private void EnableSlowMotion()
        {
            Time.timeScale = _slowMotionTimeScale;
            Time.fixedDeltaTime = _defaultFixedDeltaTime * _slowMotionTimeScale;
        }

        private void DisableSlowMotion()
        {
            if (Time.timeScale == _slowMotionTimeScale)
                Time.timeScale = 1f;

            Time.fixedDeltaTime = _defaultFixedDeltaTime;
        }

        private void CollectNearbyItems()
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(_playerTransform.position, _magnetRadius, _collectibleLayerMask);
            foreach (var hit in hits)
            {
                if (hit == null)
                    continue;

                if (hit.TryGetComponent<Coin>(out var coin))
                    coin.MagnetCollect(_playerTransform.position);
                else if (hit.TryGetComponent<Gem>(out var gem))
                    gem.MagnetCollect(_playerTransform.position);
            }
        }
    }
}