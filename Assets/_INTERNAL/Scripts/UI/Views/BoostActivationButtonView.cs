using Core.Gameplay.Game;
using Core.Gameplay.Player;
using TMPro;
using UI.Other;
using UnityEngine;

namespace UI.Views
{
    public class BoostActivationButtonView : MonoBehaviour
    {
        [SerializeField] private BoostType _boostType;
        [SerializeField] private PlayerBoostsController _playerBoostsController;
        [SerializeField] private ActionButton _activateButton;
        [SerializeField] private TextMeshProUGUI _countLabel;

        private void Awake()
        {
            if (_activateButton != null)
                _activateButton.OnButtonClick += HandleActivateButtonClick;
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
            if (_activateButton != null)
                _activateButton.OnButtonClick -= HandleActivateButtonClick;
        }

        public void RefreshCount()
        {
            int count = PlayerBoostsInventory.GetCount(_boostType);

            if (_countLabel != null)
                _countLabel.text = count.ToString();

            //if (_activateButton != null)
            //    _activateButton.gameObject.SetActive(count > 0);
        }

        private void HandleActivateButtonClick()
        {
            if (_playerBoostsController != null && _playerBoostsController.ActivateBoost(_boostType))
                RefreshCount();
        }

        private void HandleBoostCountChanged(BoostType type, int count)
        {
            if (type == _boostType)
                RefreshCount();
        }
    }
}