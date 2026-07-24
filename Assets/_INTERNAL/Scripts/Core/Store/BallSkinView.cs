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
        [SerializeField] private GameObject _lockIcon;

        [Space(5), Header("Other")]
        [SerializeField] private Image _selectButtonImage;
        [SerializeField] private Sprite _selectedButtonSprite;
        [SerializeField] private Sprite _selectButtonSprite;
        [SerializeField] private TextMeshProUGUI _selectLabel;
        [SerializeField] private Image _background;
        [SerializeField] private Sprite _selectedSprite;
        [SerializeField] private Sprite _unselectedSprite;
        [SerializeField] private bool _isMainMenuBall;

        public bool IsSelected => _isSelected;
        public Sprite Skin => _skin;

        public event Action<BallSkinView> OnSkinSelected;

        private void Awake()
        {
            _background = GetComponent<Image>();

            FindLockIconIfNeeded();

            if (_selectButton != null)
                _selectButton.OnButtonClick += HandleSelectButton;
        }

        private void OnDestroy()
        {
            if (_selectButton != null)
                _selectButton.OnButtonClick -= HandleSelectButton;
        }

        public void SetAvailableState(bool isAvailable)
        {
            if (_lockIcon != null)
                _lockIcon.SetActive(!isAvailable);

            if (!isAvailable)
                SetLockedState();
        }

        public void SetSelectedState()
        {
            _isSelected = true;

            if (_selectButtonImage != null)
                _selectButtonImage.sprite = _selectedButtonSprite;

            if (_selectLabel != null)
            {
                if(_isMainMenuBall)
                    _selectLabel.gameObject.SetActive(true);
                else
                {
                    _selectLabel.text = $"Selected";
                    _selectLabel.color = Color.black;
                }
            }

            _background.sprite = _selectedSprite;
        }

        public void SetUnselectedState()
        {
            _isSelected = false;

            if (_selectButtonImage != null)
                _selectButtonImage.sprite = _selectButtonSprite;

            if (_selectLabel != null)
            {
                if (_isMainMenuBall)
                    _selectLabel.gameObject.SetActive(false);
                else
                {
                    _selectLabel.text = $"Select";
                    _selectLabel.color = Color.white;
                }
            }

            _background.sprite = _unselectedSprite;
        }

        private void SetLockedState()
        {
            _isSelected = false;

            if (_selectButtonImage != null)
                _selectButtonImage.sprite = _selectButtonSprite;

            if (_lockIcon == null)
                FindLockIconIfNeeded();

            _lockIcon.SetActive(true);
        }

        private void FindLockIconIfNeeded()
        {
            if (_lockIcon != null)
                return;

            foreach (Transform child in transform)
            {
                if (child.name.Contains("Viewport"))
                {
                    foreach(Transform item in child.transform)
                    {
                        if (item.name.Contains("Lock_Icon"))
                        {
                            _lockIcon = child.gameObject;
                            return;
                        }
                    }
                }
            }
        }

        private void HandleSelectButton()
        {
            if (_isSelected)
                return;

            OnSkinSelected?.Invoke(this);
        }
    }
}