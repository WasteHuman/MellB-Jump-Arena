using System;
using UnityEngine;

namespace Core.Gameplay.Player
{
    public class CameraFollower : MonoBehaviour
    {
        [Header("Smoothing")]
        [SerializeField] private float _positionSmoothTime = 0.2f;
        [SerializeField] private float _verticalOffset = 1.5f;

        [SerializeField] private Camera _camera;
        [SerializeField] private Transform _target;

        private Vector3 _velocity;
        private float _highestY;
        private float _highestLowerBound;

        public float CurrentLowerBound => transform.position.y - _camera.orthographicSize;
        public event Action<float> OnHighestYChanged;

        private void LateUpdate()
        {
            Vector3 desired = _target.position + Vector3.up * _verticalOffset;
            desired.z = _camera.transform.position.z;

            if (desired.y < _highestY)
                desired.y = _highestY;
            else
                _highestY = desired.y;

            Vector3 current = _camera.transform.position;
            float newY = Mathf.SmoothDamp(current.y, desired.y, ref _velocity.y, _positionSmoothTime);

            _camera.transform.position = new Vector3(current.x, newY, current.z);
            float currentLowerBound = newY - _camera.orthographicSize;

            if (currentLowerBound > _highestLowerBound)
            {
                _highestLowerBound = currentLowerBound;
                OnHighestYChanged?.Invoke(_highestLowerBound);
            }
        }
    }
}