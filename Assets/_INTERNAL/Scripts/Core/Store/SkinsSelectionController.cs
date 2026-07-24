using Core.Gameplay.Player;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Store
{
    public class SkinsSelectionController : MonoBehaviour
    {
        [SerializeField] private Transform _container;
        [SerializeField] private List<BallSkinView> _skins = new();

        private void Awake()
        {
            var availableSkins = PlayerSkinsController.GetAvailableSkins();

            if(availableSkins.Count > 0)
            {
                for (int i = 1; i < availableSkins.Count; i++)
                {
                    var data = availableSkins[i];

                    _skins.Add(data);
                }
            }
        }
    }
}