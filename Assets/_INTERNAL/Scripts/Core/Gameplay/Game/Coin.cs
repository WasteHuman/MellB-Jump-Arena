using Core.Gameplay.Player;
using UI.Animations.Game;
using UnityEngine;

namespace Core.Gameplay.Game
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Coin : ObjectAnimations
    {
        [SerializeField] private float _amount = 5f;

        private bool _isCollected = false;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (_isCollected)
                return;

            if (collision.gameObject.TryGetComponent<PlayerController>(out var player))
                Collect();
        }

        public void MagnetCollect(Vector2 playerPosition) => MoveTo(playerPosition, () => Collect());

        public void Collect()
        {
            if (_isCollected)
                return;

            _isCollected = true;
            Hide(true);
            EconomyController.Instance.AddCollectedCoins(_amount);
        }

        public void Show()
        {
            _isCollected = false;
            gameObject.SetActive(true);
            Appear(transform.localScale);
        }

        public void Hide(bool withAnimation = false)
        {
            if (withAnimation)
            {
                Appear(Vector3.zero, () => gameObject.SetActive(false));
                return;
            }

            gameObject.SetActive(false);
        }
    }
}