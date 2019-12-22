using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Game
{
    public class InputHandler : ITickable
    {
        readonly InputState _inputState;

        public InputHandler(InputState inputState)
        {
            _inputState = inputState;
        }

        public void Tick()
        {
            if (Input.touchCount > 0)
                HandleTouches();

        }
        void HandleTouches()
        {
            Touch currentTouch = Input.touches[0];
            if (currentTouch.phase == TouchPhase.Moved)
                _inputState.totalDelta += currentTouch.deltaPosition;
            else if(currentTouch.phase != TouchPhase.Stationary)
                _inputState.totalDelta = Vector2.zero;

        }
    }
}