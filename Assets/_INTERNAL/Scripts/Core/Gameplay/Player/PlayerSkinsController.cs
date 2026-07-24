using Core.Store;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Gameplay.Player
{
    public static class PlayerSkinsController
    {
        private const string PurchasedSkinsKey = "PurchasedSkins";
        private const string SelectedSkinKey = "SelectedSkin";
        private const char Separator = '|';

        private static readonly HashSet<string> _availableSkinIds = new();
        private static readonly List<BallSkinView> _availableSkins = new();

        private static Sprite _currentPlayerSkin;
        private static string _currentPlayerSkinId;
        private static bool _isLoaded;

        public static event Action<Sprite> OnSkinChanged;
        public static event Action OnAvailableSkinsChanged;

        public static void AddNewSkin(BallSkinView ballSkinView)
        {
            if (ballSkinView == null)
                return;

            _availableSkins.Add(ballSkinView);
            UnlockSkin(ballSkinView.Skin);
            CacheAvailableSkinView(ballSkinView);
        }

        public static void RegisterDefaultSkin(Sprite skin)
        {
            EnsureLoaded();
            AddAvailableSkin(skin, true);
        }

        public static bool UnlockSkin(Sprite skin)
        {
            EnsureLoaded();

            if (!AddAvailableSkin(skin, false))
                return false;

            SaveAvailableSkins();
            OnAvailableSkinsChanged?.Invoke();
            return true;
        }

        public static bool IsSkinAvailable(Sprite skin)
        {
            EnsureLoaded();
            return skin != null && _availableSkinIds.Contains(GetSkinId(skin));
        }

        public static Sprite GetCurrentSprite() => _currentPlayerSkin;
        public static string GetCurrentSkinId() => _currentPlayerSkinId;
        public static IReadOnlyCollection<string> GetAvailableSkinIds()
        {
            EnsureLoaded();
            return _availableSkinIds;
        }

        public static IReadOnlyList<BallSkinView> GetAvailableSkins() => _availableSkins.AsReadOnly();

        public static void SelectPlayerSkin(Sprite skin)
        {
            EnsureLoaded();

            if (skin == null || !IsSkinAvailable(skin))
                return;

            _currentPlayerSkin = skin;
            _currentPlayerSkinId = GetSkinId(skin);
            PlayerPrefs.SetString(SelectedSkinKey, _currentPlayerSkinId);
            PlayerPrefs.Save();

            OnSkinChanged?.Invoke(_currentPlayerSkin);
        }

        public static bool IsSelected(Sprite skin)
        {
            EnsureLoaded();
            return skin != null && GetSkinId(skin) == _currentPlayerSkinId;
        }

        private static void EnsureLoaded()
        {
            if (_isLoaded)
                return;

            var savedSkins = PlayerPrefs.GetString(PurchasedSkinsKey, string.Empty);
            if (!string.IsNullOrEmpty(savedSkins))
            {
                var skinIds = savedSkins.Split(Separator);
                foreach (var skinId in skinIds)
                {
                    if (!string.IsNullOrWhiteSpace(skinId))
                        _availableSkinIds.Add(skinId);
                }
            }

            _currentPlayerSkinId = PlayerPrefs.GetString(SelectedSkinKey, string.Empty);
            _isLoaded = true;
        }

        private static bool AddAvailableSkin(Sprite skin, bool isDefaultSkin)
        {
            if (skin == null)
                return false;

            var skinId = GetSkinId(skin);
            var isAdded = _availableSkinIds.Add(skinId);

            if (string.IsNullOrEmpty(_currentPlayerSkinId) || _currentPlayerSkinId == skinId)
            {
                _currentPlayerSkin = skin;
                _currentPlayerSkinId = skinId;

                if (isDefaultSkin && !PlayerPrefs.HasKey(SelectedSkinKey))
                    PlayerPrefs.SetString(SelectedSkinKey, skinId);
            }

            if (isDefaultSkin && isAdded)
                SaveAvailableSkins();

            return isAdded;
        }

        private static void CacheAvailableSkinView(BallSkinView ballSkinView)
        {
            if (_availableSkins.Contains(ballSkinView))
                return;

            _availableSkins.Add(ballSkinView);
        }

        private static void SaveAvailableSkins()
        {
            PlayerPrefs.SetString(PurchasedSkinsKey, string.Join(Separator.ToString(), _availableSkinIds));
            PlayerPrefs.Save();
        }

        private static string GetSkinId(Sprite skin) => skin.name;
    }
}