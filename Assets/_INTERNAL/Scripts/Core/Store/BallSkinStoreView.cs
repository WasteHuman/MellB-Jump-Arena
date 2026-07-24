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

        private bool _isPurchased = false;

        public Sprite Skin => _skin;
        public float Price => _price;

        public event Action<BallSkinStoreView> OnBuyButtonClicked;

        private void Awake()
        {
            _buyButton.OnButtonClick += HandleBuyButtonClick;

            if(_isPurchased)
                _buyButton.gameObject.SetActive(false);
        }

        public void MarkAsPurchased()
        {
            _isPurchased = true;
            _buyButton.gameObject.SetActive(false);
        }

        private void HandleBuyButtonClick()
        {
            OnBuyButtonClicked?.Invoke(this);
        }
    }
}