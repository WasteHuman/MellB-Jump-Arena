using UnityEngine;

namespace Core.Gameplay.Game
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class Platform : MonoBehaviour
    {
        [Header("Platform Content Setup")]
        [SerializeField] private Coin _coin;
        [SerializeField] private Gem _gem;
        [SerializeField] private Trap _trap;

        [Space(5), Header("Spawn Chances (0 to 1, sum should be <= 1)")]
        [SerializeField] private float _trapChance = 0.3f;
        [SerializeField] private float _coinChance = 0.15f;
        [SerializeField] private float _gemChance = 0.03f;

        public void Initialize()
        {
            HideAllAttachments();

            float randomValue = Random.value;

            if (randomValue < _gemChance && _gem != null)
            {
                _gem.Show();
                _coin.Hide();
                _trap.Hide();
            }
            if (randomValue < _gemChance + _coinChance && _coin != null)
            {
                _coin.Show();
                _gem.Hide();
                _trap.Hide();
            }
            else if (randomValue < _gemChance + _coinChance + _trapChance && _trap != null)
            {
                _trap.Show();
                _gem.Hide();
                _coin.Hide();
            }
        }

        private void OnDisable() => HideAllAttachments();

        private void HideAllAttachments()
        {
            if (_coin == null || _gem == null || _trap == null)
                return;

            _coin.Hide();
            _gem.Hide();
            _trap.Hide();
        }
    }
}