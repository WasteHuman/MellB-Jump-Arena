using Core.Gameplay.Game;
using System;
using UnityEngine;

namespace Core.Gameplay.Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerController : MonoBehaviour
    {
        [Header("Phisycs")]
        [SerializeField] private float _jumpForce = 15f;
        [SerializeField] private float _gravityScale = 3f;
        [SerializeField] private float _horizontalSpeed = 10f;
        [SerializeField] private float _horizontalDamping = 0.5f;

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
            _playerStatisticsController.UpdateCurrentScore(transform.position.y, _bonusSystemController.CurrentMultiplier);
        }

        private void FixedUpdate()
        {
            HandleHorizontalMovement();
            HandleJump();
        }

        public void FreezePlayer() => _rb.constraints = RigidbodyConstraints2D.FreezeAll;
        public void UnfreezePlayer() => _rb.constraints = RigidbodyConstraints2D.None;

        public void Die()
        {
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
            float tilt = Input.acceleration.x;
            

#if UNITY_EDITOR
            tilt = Input.GetAxisRaw("Horizontal");
#endif

            float targetX = Mathf.Clamp(tilt * _horizontalSpeed, -_horizontalSpeed, _horizontalSpeed);

            _currentVelocity.x = Mathf.Lerp(_currentVelocity.x, targetX, Time.deltaTime / _horizontalDamping);
            _rb.linearVelocity = new Vector2(_currentVelocity.x, _rb.linearVelocity.y);
        }
    }
}