using UI.Other;
using UnityEngine;

namespace UI
{
    public class WindowsController : MonoBehaviour
    {
        [Header("Windows")]
        [SerializeField] private Window _settingsWindow;

        [Space(5), Header("Buttons Actions")]
        [SerializeField] private ButtonsController _buttonsController;

        private void Start()
        {
            _buttonsController.OnSettingsButtonClick += HandleSettingsButtonClick;
        }

        private void HandleSettingsButtonClick() => _settingsWindow.Open();
    }
}