using Core.Gameplay.Game;
using System;
using UI.Other;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Gameplay.Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerController : MonoBehaviour
    {
        [Header("Phisycs")]
        [SerializeField] private float _jumpForce = 15f;
        [SerializeField] private float _gravityScale = 3f;
        [SerializeField] private float _horizontalSpeed = 15f;
        [SerializeField] private float _horizontalDamping = 0.5f;

        [Space(5), Header("Controll UI")]
        [SerializeField] private HoldButton _leftArrow;
        [SerializeField] private HoldButton _rightArrow;

        [Space(5), Header("Collisions")]
        [SerializeField] private Transform _groundCheck;
        [SerializeField] private float _groundCheckRadius = 0.2f;
        [SerializeField] private LayerMask _groundLayer;

        [Space(5), Header("Platform Spawner Setup")]
        [SerializeField] private PlatformSpawner _platformSpawner;

        [Space(5), Header("Statistics Setup")]
        [SerializeField] private PlayerStatisticsController _playerStatisticsController;
        [SerializeField] private BonusSystemController _bonusSystemController;

        [Space(5), Header("Other")]
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private PlayerBoostsController _playerBoostsController;

        private Rigidbody2D _rb;
        private bool _isGrounded;
        private Vector2 _currentVelocity;

        public event Action OnPlayerDied;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _rb.gravityScale = _gravityScale;

            _spriteRenderer.sprite = PlayerSkinsController.GetCurrentSprite();
        }

        private void Update()
        {
            _platformSpawner.UpdateSpawnPosition();

            _isGrounded = Physics2D.OverlapCircle(_groundCheck.position, _groundCheckRadius, _groundLayer);

            float bonusMultiplier = _bonusSystemController != null ? _bonusSystemController.CurrentMultiplier : 1f;
            float boostMultiplier = _playerBoostsController != null ? _playerBoostsController.ScoreMultiplier : 1f;
            _playerStatisticsController.UpdateCurrentScore(transform.position.y, bonusMultiplier * boostMultiplier);
        }

        private void FixedUpdate()
        {
            HandleHorizontalMovement();
            HandleJump();
        }

        public void FreezePlayer() => _rb.constraints = RigidbodyConstraints2D.FreezeAll;
        public void UnfreezePlayer() => _rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        public void Die()
        {
            if (_playerBoostsController != null && _playerBoostsController.TryUseShield())
                return;

            _rb.constraints = RigidbodyConstraints2D.FreezeAll;
            OnPlayerDied?.Invoke();
        }

        private void HandleJump()
        {
            if (_isGrounded && _rb.linearVelocity.y <= 0.1f)
                _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, _jumpForce);
        }

        private void HandleHorizontalMovement()
        {
            float tilt = 0f;

            if (_leftArrow.IsHeld)
                tilt = -1f;
            else if(_rightArrow.IsHeld)
                tilt = 1f;

            float targetX = Mathf.Clamp(tilt * _horizontalSpeed, -_horizontalSpeed, _horizontalSpeed);

            _currentVelocity.x = Mathf.Lerp(_currentVelocity.x, targetX, Time.deltaTime / _horizontalDamping);
            _rb.linearVelocity = new Vector2(_currentVelocity.x, _rb.linearVelocity.y);
        }
    }
}