using UI.Views;
using UnityEngine;

namespace Core.Gameplay.Game
{
    public class PlayerStatisticsController : MonoBehaviour
    {
        private const string BEST_SCORE_KEY = "Best_Score";

        [SerializeField] private PlayerScoreView _view;

        private float _highestY;
        private float _currentScore;
        private float _bestScore;

        public float BestScore => Mathf.RoundToInt(_bestScore);
        public float Score => Mathf.RoundToInt(_currentScore);

        private void Awake()
        {
            if (PlayerPrefs.HasKey(BEST_SCORE_KEY))
                _bestScore = PlayerPrefs.GetInt(BEST_SCORE_KEY);

            _view.UpdateUI(_currentScore, _bestScore);
        }

        public void UpdateCurrentScore(float highestY, float multiplier = 1f)
        {
            if (highestY <= _highestY)
                return;

            float delta = highestY - _highestY;

            _highestY = highestY;

            _currentScore += delta * multiplier;

            _view.UpdateUI(_currentScore, _bestScore);
        }

        public void SaveBestScore()
        {
            if(_currentScore > _bestScore)
            {
                _bestScore = _currentScore;
                PlayerPrefs.SetInt(BEST_SCORE_KEY, Mathf.RoundToInt(_currentScore));
            }
        }
    }
}