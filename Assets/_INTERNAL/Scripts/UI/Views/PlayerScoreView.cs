using TMPro;
using UnityEngine;

namespace UI.Views
{
    public class PlayerScoreView : MonoBehaviour
    {
        [Header("Labels Setup")]
        [SerializeField] private TextMeshProUGUI _bestScoreLabel;
        [SerializeField] private TextMeshProUGUI _scoreLabel;

        public void UpdateUI(float score, float bestScore = 0f)
        {
            if (_scoreLabel != null)
                _scoreLabel.text = Mathf.RoundToInt(score).ToString();

            if (_bestScoreLabel != null)
                _bestScoreLabel.text = $"Best score: {Mathf.RoundToInt(bestScore)}";
        }
    }
}