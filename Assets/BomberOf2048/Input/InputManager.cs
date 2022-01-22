using System;
using BomberOf2048.Utils;
using BomberOf2048.Utils.Disposables;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BomberOf2048.Input
{
    [DefaultExecutionOrder(-1)]
    public class InputManager : Singleton<InputManager>
    {
        [SerializeField] private float _deadZone = 70f;

        
        public delegate void TapTouchEvent(Vector2 position, float time);
        public event TapTouchEvent OnTapTouch;

        public delegate void SwipeEvent(Vector2 swipeSide);
        public event SwipeEvent OnSwipeEvent;

        public readonly Lock InputLocker = new Lock();
        
        
        private TouchControls _touchControls;

        private bool _isTouchStarted = false;
        private Vector2 _startTouchPos;
        
        private Vector2 TouchPosition => _touchControls.Touch.TouchPosition.ReadValue<Vector2>();

        private void Awake()
        {
            _touchControls = new TouchControls();
        }
        private void Start()
        {
            _touchControls.Touch.TouchPress.started += StartTouch;
            _touchControls.Touch.TouchPress.canceled += EndTouch;
            _touchControls.Touch.TouchTap.started += TapTouch;
        }

        private void Update()
        {
            if(!_isTouchStarted)
                return;
            if(Vector2.Distance(_startTouchPos, TouchPosition) < _deadZone)
                return;
            var dist = TouchPosition - _startTouchPos;
            if (Mathf.Abs(dist.x) >= Mathf.Abs(dist.y))
            {
                OnSwipeEvent?.Invoke(dist.x > 0f ? Vector2.right : Vector2.left);
            }
            else
            {
                OnSwipeEvent?.Invoke(dist.y > 0f ? Vector2.up : Vector2.down);
            }

            _isTouchStarted = false;
        }

        private void StartTouch(InputAction.CallbackContext context)
        {
            if(InputLocker.IsLocked)
                return;
            _isTouchStarted = true;
            _startTouchPos = TouchPosition;
        }
        
        private void EndTouch(InputAction.CallbackContext context)
        {
            _isTouchStarted = false;
        }

        private void TapTouch(InputAction.CallbackContext context)
        {
            if(InputLocker.IsLocked)
                return;
            OnTapTouch?.Invoke(TouchPosition, (float)context.time);
        }

        public IDisposable SubscribeOnTapTouch(TapTouchEvent call)
        {
            OnTapTouch += call;
            return new ActionDisposable(() => OnTapTouch -= call);
        }
        
        public IDisposable SubscribeOnSwipe(SwipeEvent call)
        {
            OnSwipeEvent += call;
            return new ActionDisposable(() => OnSwipeEvent -= call);
        }
        
        private void OnEnable()
        {
            _touchControls.Enable();
        }
        private void OnDisable()
        {
            _touchControls.Disable();
        }

        private void OnDestroy()
        {
            _touchControls.Touch.TouchPress.started -= StartTouch;
            _touchControls.Touch.TouchPress.canceled -= EndTouch;
            _touchControls.Touch.TouchTap.started -= TapTouch;
        }
    }
}