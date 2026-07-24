using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace Core.Gameplay.Game
{
    public class PlatformSpawner : MonoBehaviour
    {
        [Header("Platforms Pool Setup")]
        [SerializeField] private int _initialPoolSize = 20;
        [SerializeField] private bool _autoExpand = true;
        [SerializeField] private Transform _platformContainer;
        [SerializeField] private Platform _platformPrefab;

        [Space(5), Header("Spawn Setup")]
        [SerializeField] private float _minVerticalDistance = 1.5f;
        [SerializeField] private float _maxVerticalDistance = 3.0f;
        [SerializeField] private float _horizontalSpawnRange = 4.0f;

        [Space(5), Header("Screen & Count Setup")]
        [SerializeField] private int _minPlatformsOnScreen = 3;
        [SerializeField] private int _maxPlatformsOnScreen = 10;
        [SerializeField] private float _minHorizontalGap = 1.5f;
        [SerializeField] private float _ySpawnOffest = 2f;

        private float _lastSpawnedY;
        private Camera _mainCamera;

        private ObjectPool<Platform> _platformPool;
        private List<Platform> _activePlatforms = new();

        private void Awake()
        {
            _platformPool = new(_platformPrefab, _initialPoolSize, _platformContainer)
            {
                AutoExpand = _autoExpand
            };

            _mainCamera = Camera.main;

            InitialSpawnPlatforms();
        }

        private void Update() => CheckAndRecyclePlatforms();

        private void InitialSpawnPlatforms()
        {
            _lastSpawnedY = -0.5f;

            float screenTop = _mainCamera.transform.position.y + (_mainCamera.orthographicSize * 2);

            while (_lastSpawnedY < screenTop || _activePlatforms.Count < _minPlatformsOnScreen)
                SpawnNextRow();
        }

        private void SpawnNextRow()
        {
            float nextY = _lastSpawnedY + Random.Range(_minVerticalDistance, _maxVerticalDistance);

            int platformsInRow = Random.Range(1, 4);

            List<float> validXPositions = GenerateValidXPositions(platformsInRow);

            foreach (float x in validXPositions)
                SpawnPlatform(new Vector3(x, nextY, 0f));

            _lastSpawnedY = nextY;
        }

        private List<float> GenerateValidXPositions(int count)
        {
            List<float> positions = new List<float>();

            for (int i = 0; i < count; i++)
            {
                float newX;
                int attempts = 0;
                bool isValid = false;

                do
                {
                    newX = Random.Range(-_horizontalSpawnRange, _horizontalSpawnRange);
                    isValid = true;

                    foreach (float existingX in positions)
                    {
                        if (Mathf.Abs(newX - existingX) < _minHorizontalGap)
                        {
                            isValid = false;
                            break;
                        }
                    }
                    attempts++;

                    if (attempts > 20)
                    {
                        if (positions.Count == 0) positions.Add(newX);
                        break;
                    }

                } while (!isValid);

                if (isValid) positions.Add(newX);
            }

            return positions;
        }

        private void SpawnPlatform(Vector3 position)
        {
            Platform newPlatform = _platformPool.GetFreeElement();
            newPlatform.transform.position = position;
            newPlatform.Initialize();
            _activePlatforms.Add(newPlatform);
            _lastSpawnedY = position.y;
        }

        private void CheckAndRecyclePlatforms()
        {
            float bottomEdge = _mainCamera.transform.position.y - _mainCamera.orthographicSize - 2f;

            for (int i = _activePlatforms.Count - 1; i >= 0; i--)
            {
                if (_activePlatforms[i].transform.position.y < bottomEdge)
                {
                    _platformPool.ReturnToPool(_activePlatforms[i]);
                    _activePlatforms.RemoveAt(i);
                }
            }
        }

        public void UpdateSpawnPosition()
        {
            float screenTop = _mainCamera.transform.position.y + _mainCamera.orthographicSize;

            float spawnUpTo = screenTop + _ySpawnOffest;

            if (_lastSpawnedY < screenTop)
                _lastSpawnedY = screenTop;

            while (_lastSpawnedY < spawnUpTo && _activePlatforms.Count < _maxPlatformsOnScreen)
                SpawnNextRow();
        }
    }
}