using Core.Gameplay;
using Core.Gameplay.Player;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Store
{
    public class StoreController : MonoBehaviour
    {
        [SerializeField] private Transform _container;
        [SerializeField] private List<BallSkinStoreView> _skins = new();

        private void Awake()
        {
            CollectSkinsFromContainer();
            RegisterDefaultSkins();
            SubscribeToSkins();
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
        }

        private void HandleBuyButtonClicked(BallSkinStoreView skin)
        {
            if (skin == null || PlayerSkinsController.IsSkinAvailable(skin.Skin))
                return;

            if (EconomyController.Instance == null)
            {
                Debug.LogWarning("Can not buy skin: EconomyController is not initialized.");
                return;
            }

            if (!EconomyController.Instance.SpendCoins(skin.Price))
                return;

            PlayerSkinsController.UnlockSkin(skin.Skin);
            skin.MarkAsPurchased();
            RefreshStoreState();
        }

        private void RefreshStoreState()
        {
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

        private void CollectSkinsFromContainer()
        {
            if (_container == null || _skins.Count > 0)
                return;

            _container.GetComponentsInChildren(true, _skins);
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
                    skin.OnBuyButtonClicked += HandleBuyButtonClicked;
            }
        }

        private void UnsubscribeFromSkins()
        {
            foreach (var skin in _skins)
            {
                if (skin != null)
                    skin.OnBuyButtonClicked -= HandleBuyButtonClicked;
            }
        }
    }
}