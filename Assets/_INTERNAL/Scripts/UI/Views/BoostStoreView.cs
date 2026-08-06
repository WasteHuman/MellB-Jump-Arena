using Core.Gameplay.Game;
using System;
using TMPro;
using UI.Other;
using UnityEngine;

namespace UI.Views
{
    public class BoostStoreView : MonoBehaviour
    {
        [SerializeField] private BoostType _boostType;
        [SerializeField] private float _price;
        [SerializeField] private ActionButton _buyButton;
        [SerializeField] private TextMeshProUGUI _countLabel;

        public BoostType BoostType => _boostType;
        public float Price => _price;

        public event Action<BoostStoreView> OnBuyButtonClicked;

        private void Awake()
        {
            if (_buyButton != null)
                _buyButton.OnButtonClick += HandleBuyButtonClick;
        }

        private void OnEnable()
        {
            PlayerBoostsInventory.OnBoostCountChanged += HandleBoostCountChanged;
            RefreshCount();
        }

        private void OnDisable()
        {
            PlayerBoostsInventory.OnBoostCountChanged -= HandleBoostCountChanged;
        }

        private void OnDestroy()
        {
            if (_buyButton != null)
                _buyButton.OnButtonClick -= HandleBuyButtonClick;
        }

        public void RefreshCount()
        {
            if (_countLabel != null)
                _countLabel.text = PlayerBoostsInventory.GetCount(_boostType).ToString();
        }

        private void HandleBoostCountChanged(BoostType type, int count)
        {
            if (type == _boostType)
                RefreshCount();
        }

        private void HandleBuyButtonClick() => OnBuyButtonClicked?.Invoke(this);
    }
}