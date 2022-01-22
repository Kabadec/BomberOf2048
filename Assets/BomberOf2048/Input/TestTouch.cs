using UnityEngine;

namespace BomberOf2048.Input
{
    public class TestTouch : MonoBehaviour
    {
        private InputManager _inputManager;
        private Camera _mainCamera;

        private void Awake()
        {
            _inputManager = InputManager.Instance;
            _mainCamera = Camera.main;
        }

        private void OnEnable()
        {
            //_inputManager.OnStartTouch += Move;
        }

        private void OnDisable()
        {
            //_inputManager.OnStartTouch -= Move;
        }

        private void Move(Vector2 position, float time)
        {
            var screenCoordinates = new Vector3(position.x, position.y, 0f);
            var worldCoordinates = _mainCamera.ScreenToWorldPoint(screenCoordinates);
            worldCoordinates.z = 0f;
            transform.position = worldCoordinates;
        }
    }
}