using UnityEngine;
using UnityEngine.UI;

namespace UI.Views
{
    public class BonusProgressBarView : MonoBehaviour
    {
        [Header("Bars")]
        [SerializeField] private GameObject _x3Root;
        [SerializeField] private GameObject _x5Root;

        [Header("Segments")]
        [SerializeField] private Image[] _x3Segments;
        [SerializeField] private Image[] _x5Segments;

        private Image[] _currentSegments;

        /// <summary>
        /// Показываем бар зарядки (до активации бонуса).
        /// Обычно используется стиль x3.
        /// </summary>
        public void ShowFillingBar()
        {
            SetActiveBar(_x3Segments, _x3Root, _x5Root);
        }

        /// <summary>
        /// Показываем активный бонус с нужным множителем.
        /// </summary>
        public void ShowBonus(float multiplier)
        {
            if (Mathf.Approximately(multiplier, 5f))
                SetActiveBar(_x5Segments, _x5Root, _x3Root);
            else
                SetActiveBar(_x3Segments, _x3Root, _x5Root);
        }

        /// <summary>
        /// Скрываем оба бара.
        /// </summary>
        public void HideBonus()
        {
            _x3Root.SetActive(false);
            _x5Root.SetActive(false);

            ResetSegments(_x3Segments);
            ResetSegments(_x5Segments);

            _currentSegments = null;
        }

        /// <summary>
        /// progress01: 0..1
        /// 0 — пусто, 1 — полностью заполнено.
        /// Используется и для заполнения, и для обратного отсчёта.
        /// </summary>
        public void UpdateProgress(float progress01)
        {
            progress01 = Mathf.Clamp01(progress01);

            int activeSegments = Mathf.CeilToInt(progress01 * _currentSegments.Length);

            for (int i = 0; i < _currentSegments.Length; i++)
                _currentSegments[i].enabled = i < activeSegments;
        }

        private void SetActiveBar(Image[] segments, GameObject activeRoot, GameObject inactiveRoot)
        {
            activeRoot.SetActive(true);
            inactiveRoot.SetActive(false);

            _currentSegments = segments;

            ResetSegments(_currentSegments);
        }

        private static void ResetSegments(Image[] segments)
        {
            foreach (var segment in segments)
                segment.fillAmount = 0f;
        }
    }
}