using System;
using UI.Views;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Core.Gameplay.Game
{
    public class BonusSystemController : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float _fillDuration = 5f;
        [SerializeField] private float _activationChance = 0.3f;
        [SerializeField] private float _activeDuration = 5f;
        [SerializeField] private float _cooldownDuration = 30f;

        [Header("Multiplier Settings")]
        [SerializeField, Range(0f, 1f)]
        private float _multiplier3Chance = 0.7f;

        [Header("View")]
        [SerializeField] private BonusProgressBarView _view;

        private enum BonusState
        {
            Filling,
            Active,
            Cooldown
        }

        private BonusState _currentState;

        private float _fillTimer;
        private float _activeTimer;
        private float _cooldownTimer;

        public float CurrentMultiplier { get; private set; } = 1f;

        public event Action<float> OnMultiplierChanged;

        private void Start()
        {
            _view.HideBonus();
            StartFilling();
        }

        private void Update()
        {
            switch (_currentState)
            {
                case BonusState.Filling:
                    ProcessFilling();
                    break;

                case BonusState.Active:
                    ProcessActive();
                    break;

                case BonusState.Cooldown:
                    ProcessCooldown();
                    break;
            }
        }

        private void ProcessFilling()
        {
            _fillTimer += Time.deltaTime;

            float progress = Mathf.Clamp01(_fillTimer / _fillDuration);

            _view.UpdateProgress(progress);

            if (progress >= 1f)
            {
                if (Random.value <= _activationChance)
                    ActivateBonus();
                else
                    StartCooldown();
            }
        }

        private void ProcessActive()
        {
            _activeTimer -= Time.deltaTime;

            float progress = Mathf.Clamp01(_activeTimer / _activeDuration);
            _view.UpdateProgress(progress);

            if (_activeTimer <= 0f)
                StartCooldown();
        }

        private void ProcessCooldown()
        {
            _cooldownTimer -= Time.deltaTime;

            if (_cooldownTimer <= 0f)
                StartFilling();
        }

        private void ActivateBonus()
        {
            _currentState = BonusState.Active;
            _activeTimer = _activeDuration;

            CurrentMultiplier = Random.value <= _multiplier3Chance ? 3f : 5f;

            _view.ShowBonus(CurrentMultiplier);
            _view.UpdateProgress(1f);

            OnMultiplierChanged?.Invoke(CurrentMultiplier);
        }

        private void StartCooldown()
        {
            _currentState = BonusState.Cooldown;
            _cooldownTimer = _cooldownDuration;

            _fillTimer = 0f;
            _activeTimer = 0f;

            CurrentMultiplier = 1f;

            _view.HideBonus();

            OnMultiplierChanged?.Invoke(CurrentMultiplier);
        }

        private void StartFilling()
        {
            _currentState = BonusState.Filling;
            _fillTimer = 0f;

            _view.ShowFillingBar();
            _view.UpdateProgress(0f);
        }
    }
}