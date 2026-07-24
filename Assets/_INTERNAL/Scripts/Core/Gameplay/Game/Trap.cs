using Core.Gameplay.Player;
using UI.Animations.Game;
using UnityEngine;

namespace Core.Gameplay.Game
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Trap : ObjectAnimations
    {
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.TryGetComponent<PlayerController>(out var player))
            {
                var rb = player.GetComponent<Rigidbody2D>();

                if (rb.linearVelocityY <= 0.01f)
                {
                    Hide(true);
                    player.Die();
                }
            }
        }

        public void Show()
        {
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