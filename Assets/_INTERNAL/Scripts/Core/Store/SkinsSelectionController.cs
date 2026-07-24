using Core.Gameplay.Player;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Store
{
    public class SkinsSelectionController : MonoBehaviour
    {
        [SerializeField] private Transform _container;
        [SerializeField] private int _freeSkinsCount = 3;
        [SerializeField] private List<BallSkinView> _skins = new();

        private void Awake()
        {
            CollectSkinsFromContainer();
            RegisterDefaultSkins();
            SubscribeToSkins();
        }

        private void OnEnable()
        {
            PlayerSkinsController.OnAvailableSkinsChanged += RefreshSkinsState;
            PlayerSkinsController.OnSkinChanged += HandleSkinChanged;
            RefreshSkinsState();
        }

        private void OnDisable()
        {
            PlayerSkinsController.OnAvailableSkinsChanged -= RefreshSkinsState;
            PlayerSkinsController.OnSkinChanged -= HandleSkinChanged;
        }

        private void OnDestroy()
        {
            UnsubscribeFromSkins();
        }

        private void CollectSkinsFromContainer()
        {
            if (_container == null || _skins.Count > 0)
                return;

            _container.GetComponentsInChildren(true, _skins);
        }

        private void RefreshSkinsState()
        {
            foreach (var skin in _skins)
            {
                if (skin == null)
                    continue;

                var isAvailable = PlayerSkinsController.IsSkinAvailable(skin.Skin);
                skin.SetAvailableState(isAvailable);

                if (!isAvailable)
                    continue;

                if (PlayerSkinsController.IsSelected(skin.Skin))
                    skin.SetSelectedState();
                else
                    skin.SetUnselectedState();
            }
        }

        private void RegisterDefaultSkins()
        {
            var defaultSkinsCount = Mathf.Min(_freeSkinsCount, _skins.Count);

            for (var i = 0; i < defaultSkinsCount; i++)
            {
                if (_skins[i] != null)
                    PlayerSkinsController.RegisterDefaultSkin(_skins[i].Skin);
            }

            foreach (var skin in _skins)
            {
                if (skin != null && skin.IsSelected)
                    PlayerSkinsController.RegisterDefaultSkin(skin.Skin);
            }
        }

        private void SubscribeToSkins()
        {
            foreach (var skin in _skins)
            {
                if (skin != null)
                    skin.OnSkinSelected += HandleSelectedSkin;
            }
        }

        private void UnsubscribeFromSkins()
        {
            foreach (var skin in _skins)
            {
                if (skin != null)
                    skin.OnSkinSelected -= HandleSelectedSkin;
            }
        }

        private void HandleSelectedSkin(BallSkinView selectedSkin)
        {
            if (!PlayerSkinsController.IsSkinAvailable(selectedSkin.Skin))
                return;

            PlayerSkinsController.SelectPlayerSkin(selectedSkin.Skin);
        }

        private void HandleSkinChanged(Sprite _) => RefreshSkinsState();
    }
}