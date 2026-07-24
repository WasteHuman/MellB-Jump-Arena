using Core.Gameplay.Player;
using System;
using TMPro;
using UI.Other;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Store
{
    public class BallSkinView : MonoBehaviour
    {
        [Header("View Setup")]
        [SerializeField] private ActionButton _selectButton;
        [SerializeField] private Sprite _skin;
        [SerializeField] private bool _isSelected = false;

        [Space(5), Header("Other")]
        [SerializeField] private Image _selectButtonImage;
        [SerializeField] private Sprite _selectedButtonSprite;
        [SerializeField] private Sprite _selectButtonSprite;
        [SerializeField] private TextMeshProUGUI _selectLabel;

        public bool IsSelected => _isSelected;
        public Sprite Skin => _skin;

        public event Action<BallSkinView> OnSkinSelected;

        private void Awake()
        {
            _selectButton.OnButtonClick += HandleSelectButton;
        }

        private void OnDestroy()
        {
            _selectButton.OnButtonClick -= HandleSelectButton;
        }

        public void Initialize(BallSkinView ballSkinView)
        {
            _skin = ballSkinView.Skin;
            _isSelected = ballSkinView.IsSelected;
        }

        public void SetSelectedState()
        {
            _isSelected = true;

            if(_selectButton != null)
                _selectButtonImage.sprite = _selectedButtonSprite;

            _selectLabel.text = $"Selected";
            _selectLabel.color = Color.black;
        }

        public void SetUnselectedState()
        {
            _isSelected = false;

            
            _selectLabel.text = $"Select";
            _selectLabel.color = Color.white;
        }

        private void HandleSelectButton()
        {
            if (_isSelected)
                return;

            OnSkinSelected?.Invoke(this);
        }
    }
}