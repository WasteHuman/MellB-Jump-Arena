using Core.Gameplay;
using Core.Gameplay.Game;
using Core.Gameplay.Player;
using System.Collections.Generic;
using UI.Views;
using UnityEngine;

namespace Core.Store
{
    public class StoreController : MonoBehaviour
    {
        [SerializeField] private Transform _ballsContainer;
        [SerializeField] private Transform _boostsContainer;
        [SerializeField] private List<BallSkinStoreView> _skins = new();
        [SerializeField] private List<BoostStoreView> _boosts = new();

        private void Awake()
        {
            CollectSkinsFromContainer();
            CollectBoostsFromContainer();
            RegisterDefaultSkins();
            SubscribeToSkins();
            SubscribeToBoosts();
        }

        private void OnEnable()
        {
            PlayerSkinsController.OnAvailableSkinsChanged += RefreshStoreState;
            RefreshStoreState();
        }

        private void OnDisable()
        {
            PlayerSkinsController.OnAvailableSkinsChanged -= RefreshStoreState;
        }

        private void OnDestroy()
        {
            UnsubscribeFromSkins();
            UnsubscribeFromBoosts();
        }

        private void HandleBuyBoostButtonClicked(BoostStoreView boost)
        {
            if (boost == null)
                return;

            if (EconomyController.Instance == null)
            {
                Debug.LogWarning("Can not buy boost: EconomyController is not initialized.");
                return;
            }

            if (!EconomyController.Instance.SpendGems(boost.Price))
                return;

            PlayerBoostsInventory.Add(boost.BoostType);
            boost.RefreshCount();
            Debug.Log($"[Store Controller] Boost [{boost.gameObject.name}] purchased");
        }

        private void HandleBallBuyButtonClicked(BallSkinStoreView skin)
        {
            if (skin == null || PlayerSkinsController.IsSkinAvailable(skin.Skin))
                return;

            if (EconomyController.Instance == null)
            {
                Debug.LogWarning("Can not buy skin: EconomyController is not initialized.");
                return;
            }

            switch (skin.CurrencyType)
            {
                case CurrencyType.Coins:
                    if (!EconomyController.Instance.SpendCoins(skin.Price))
                        return;
                    break;
                case CurrencyType.Gems:
                    if (!EconomyController.Instance.SpendGems(skin.Price))
                        return;
                    break;
            }

            PlayerSkinsController.UnlockSkin(skin.Skin);
            skin.MarkAsPurchased();
            RefreshStoreState();
        }

        private void RefreshStoreState()
        {
            foreach (var boost in _boosts)
            {
                if (boost != null)
                    boost.RefreshCount();
            }

            foreach (var skin in _skins)
            {
                if (skin == null)
                    continue;

                if (PlayerSkinsController.IsSkinAvailable(skin.Skin))
                    skin.MarkAsPurchased();
                else
                    skin.MarkAsAvailableForPurchase();
            }
        }

        private void CollectBoostsFromContainer()
        {
            if (_boosts.Count > 0)
                return;

            Transform container = _boostsContainer != null ? _boostsContainer : _ballsContainer;
            if (container != null)
                container.GetComponentsInChildren(true, _boosts);
        }

        private void CollectSkinsFromContainer()
        {
            if (_ballsContainer == null || _skins.Count > 0)
                return;

            _ballsContainer.GetComponentsInChildren(true, _skins);
        }

        private void RegisterDefaultSkins()
        {
            foreach (var skin in _skins)
            {
                if (skin != null && skin.IsAvailableByDefault)
                    PlayerSkinsController.RegisterDefaultSkin(skin.Skin);
            }
        }

        private void SubscribeToSkins()
        {
            foreach (var skin in _skins)
            {
                if (skin != null)
                    skin.OnBuyButtonClicked += HandleBallBuyButtonClicked;
            }
        }

        private void UnsubscribeFromSkins()
        {
            foreach (var skin in _skins)
            {
                if (skin != null)
                    skin.OnBuyButtonClicked -= HandleBallBuyButtonClicked;
            }
        }

        private void SubscribeToBoosts()
        {
            foreach (var boost in _boosts)
            {
                if (boost != null)
                    boost.OnBuyButtonClicked += HandleBuyBoostButtonClicked;
            }
        }

        private void UnsubscribeFromBoosts()
        {
            foreach (var boost in _boosts)
            {
                if (boost != null)
                    boost.OnBuyButtonClicked -= HandleBuyBoostButtonClicked;
            }
        }
    }
}