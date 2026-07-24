using Core.Gameplay.Player;
using UI.Animations.Game;
using UnityEngine;

namespace Core.Gameplay.Game
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Gem : ObjectAnimations
    {
        [SerializeField] private float _amount = 1f;

        private bool _isCollected = false;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (_isCollected)
                return;

            if (collision.gameObject.TryGetComponent<PlayerController>(out var player))
            {
                _isCollected = true;
                Hide(true);
                EconomyController.Instance.AddGems(_amount);
            }
        }

        public void Show()
        {
            gameObject.SetActive(true);
            Appear(transform.localScale);
        }

        public void Hide(bool withAnimation = false)
        {
            _isCollected = false;

            if (withAnimation)
            {
                Appear(Vector3.zero, () => gameObject.SetActive(false));
                return;
            }

            gameObject.SetActive(false);
        }
    }
}