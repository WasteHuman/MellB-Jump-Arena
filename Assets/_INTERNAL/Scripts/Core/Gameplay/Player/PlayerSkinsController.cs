using Core.Store;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Gameplay.Player
{
    public static class PlayerSkinsController
    {
        private static List<BallSkinView> _availableSkins = new();

        private static Sprite _currentPlayerSkin;

        public static event Action<Sprite> OnSkinChanged;

        public static void AddNewSkin(BallSkinView ballSkinView)
        {
            if (_availableSkins.Contains(ballSkinView))
                return;

            _availableSkins.Add(ballSkinView);
        }

        public static Sprite GetCurrentSprite() => _currentPlayerSkin;
        public static IReadOnlyList<BallSkinView> GetAvailableSkins() => _availableSkins.AsReadOnly();

        public static void SelectPlayerSkin(Sprite skin)
        {
            _currentPlayerSkin = skin;
            OnSkinChanged?.Invoke(_currentPlayerSkin);
        } 
    }
}