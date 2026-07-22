using System;
using UnityEngine;

namespace Core.Gameplay
{
    public class EconomyController : MonoBehaviour
    {
        private static EconomyController _instance;

        [SerializeField] private float _initialBalance = 100000f;

        private float _currentCoinsBalance;

        public event Action<float> OnBalanceChanged;

        public static EconomyController Instance
        {
            get => _instance;
        }

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
            _currentCoinsBalance = _initialBalance;

            DontDestroyOnLoad(gameObject);
        }

        /// <summary>
        /// Получить текущий баланс Coins
        /// </summary>
        public float GetCoinsBalance() => _currentCoinsBalance;

        /// <summary>
        /// Запросить текущий баланс Coins (invoke события)
        /// </summary>
        public void RequestCoinsBalance() => OnBalanceChanged?.Invoke(_currentCoinsBalance);

        /// <summary>
        /// Добавить средства (выигрыш, бонус)
        /// </summary>
        public void AddCoins(float amount)
        {
            if (amount < 0)
            {
                Debug.LogWarning($"Attempt to add a negattive amount: {amount}. Use the SpendCoins() method");
                return;
            }

            _currentCoinsBalance += amount;
            OnBalanceChanged?.Invoke(_currentCoinsBalance);

            Debug.Log($"[Economy] Added: +{amount}. New balance: {_currentCoinsBalance}");
        }

        /// <summary>
        /// Списать средства (ставка, проигрыш)
        /// </summary>
        public bool SpendCoins(float amount)
        {
            if (amount < 0)
            {
                Debug.LogWarning($"Attempt to debit a negative amount: {amount}. Use the AddCoins() method");
                return false;
            }

            if (!HasEnoughBalance(amount))
            {
                Debug.LogWarning($"Not enough coins! Balance: {_currentCoinsBalance}, needed: {amount}");
                return false;
            }

            _currentCoinsBalance -= amount;
            OnBalanceChanged?.Invoke(_currentCoinsBalance);

            Debug.Log($"[Economy] Debited: -{amount}. New balance: {_currentCoinsBalance}");
            return true;
        }

        /// <summary>
        /// Проверить, достаточно ли средств
        /// </summary>
        public bool HasEnoughBalance(float amount) => _currentCoinsBalance >= amount;

        /// <summary>
        /// Установить баланс (для тестирования или загрузки из сохранений)
        /// </summary>
        public void SetBalance(float amount)
        {
            _currentCoinsBalance = Mathf.Max(0, amount);
            OnBalanceChanged?.Invoke(_currentCoinsBalance);
        }

        /// <summary>
        /// Сбросить баланс на начальное значение
        /// </summary>
        public void ResetBalance()
        {
            _currentCoinsBalance = _initialBalance;
            OnBalanceChanged?.Invoke(_currentCoinsBalance);
        }
    }
}