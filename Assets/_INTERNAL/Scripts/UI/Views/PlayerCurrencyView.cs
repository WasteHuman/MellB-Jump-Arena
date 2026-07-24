using Core.Gameplay;
using TMPro;
using UnityEngine;
using Utils.Formatter;

namespace UI.Views
{
    public class PlayerCurrencyView : MonoBehaviour
    {
        [Header("Labeles Setup")]
        [SerializeField] private TextMeshProUGUI _coinsBalanceLabel;
        [SerializeField] private TextMeshProUGUI _gemsBalanceLabel;

        private readonly NumberFormatter _formatter = new();

        private void Awake()
        {
            EconomyController.Instance.OnCoinsBalanceChanged += HandleChangedCoinsBalance;
            EconomyController.Instance.OnGemsBalanceChanged += HandleChangedGemsBalance;
        }

        private void Start()
        {
            EconomyController.Instance.RequestCoinsBalance();
            EconomyController.Instance.RequestGemsBanalce();
        }

        private void OnDestroy()
        {
            EconomyController.Instance.OnCoinsBalanceChanged -= HandleChangedCoinsBalance;
            EconomyController.Instance.OnGemsBalanceChanged -= HandleChangedGemsBalance;
        }

        private void HandleChangedGemsBalance(float amount) => _gemsBalanceLabel.text = _formatter.FormatNumber(amount);

        private void HandleChangedCoinsBalance(float amount) => _coinsBalanceLabel.text = _formatter.FormatNumber(amount);
    }
}