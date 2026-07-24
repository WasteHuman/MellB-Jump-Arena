using Core.Gameplay.Player;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Store
{
    public class SkinsController : MonoBehaviour
    {
        [SerializeField] private List<BallSkinView> _skins = new();

        private void Awake()
        {
            foreach(var skin in _skins)
                skin.OnSkinSelected += HandleSelectedSkin;
        }

        private void HandleSelectedSkin(BallSkinView selectedSkin)
        {
            selectedSkin.SetSelectedState();
            PlayerSkinsController.SelectPlayerSkin(selectedSkin.Skin);

            foreach(var skin in _skins)
            {
                if (skin != selectedSkin)
                    skin.SetUnselectedState();
            }
        }
    }
}