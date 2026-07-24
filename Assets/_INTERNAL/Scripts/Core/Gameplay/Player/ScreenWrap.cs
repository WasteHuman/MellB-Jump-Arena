using UnityEngine;

namespace Core.Gameplay.Player
{
    public class ScreenWrap : MonoBehaviour
    {
        private Camera _mainCamera;
        private SpriteRenderer _spriteRenderer;
        private float _spriteHalfWidth;
        private float _minX;
        private float _maxX;

        private void Awake()
        {
            _mainCamera = Camera.main;
            _spriteRenderer = GetComponent<SpriteRenderer>();

            if (_spriteRenderer != null)
                _spriteHalfWidth = _spriteRenderer.bounds.extents.x;

            float depth = Mathf.Abs(_mainCamera.transform.position.z - transform.position.z);
            Vector3 left = _mainCamera.ScreenToWorldPoint(new Vector3(0, 0, depth));
            Vector3 right = _mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, 0, depth));
            _minX = left.x + _spriteHalfWidth;
            _maxX = right.x - _spriteHalfWidth;
        }

        private void Update()
        {
            Vector3 pos = transform.position;

            if (pos.x > _maxX)
                pos.x = _minX;
            else if (pos.x < _minX)
                pos.x = _maxX;

            transform.position = pos;
        }
    }
}