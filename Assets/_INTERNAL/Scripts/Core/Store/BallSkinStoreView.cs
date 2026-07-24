using System;
using UI.Other;
using UnityEngine;

namespace Core.Store
{
    public class BallSkinStoreView : MonoBehaviour
    {
        [SerializeField] private ActionButton _buyButton;
        [SerializeField] private Sprite _skin;
        [SerializeField] private float _price;
        [SerializeField] private bool _isAvailableByDefault = false;

        private bool _isPurchased = false;

        public Sprite Skin => _skin;
        public float Price => _price;
        public bool IsAvailableByDefault => _isAvailableByDefault;

        public event Action<BallSkinStoreView> OnBuyButtonClicked;

        private void Awake()
        {
            if (_buyButton != null)
                _buyButton.OnButtonClick += HandleBuyButtonClick;

            if (_isPurchased)
                MarkAsPurchased();
        }

        private void OnDestroy()
        {
            if (_buyButton != null)
                _buyButton.OnButtonClick -= HandleBuyButtonClick;
        }

        public void MarkAsPurchased()
        {
            _isPurchased = true;

            if (_buyButton != null)
                _buyButton.gameObject.SetActive(false);
        }

        public void MarkAsAvailableForPurchase()
        {
            _isPurchased = false;

            if (_buyButton != null)
                _buyButton.gameObject.SetActive(true);
        }

        private void HandleBuyButtonClick()
        {
            if (_isPurchased)
                return;

            OnBuyButtonClicked?.Invoke(this);
        }
    }
}