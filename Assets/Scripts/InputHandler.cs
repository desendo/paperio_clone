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
#pragma warning disable CS0162 // Обнаружен недостижимый код

            if (true)
            {
                if (Input.GetKey(KeyCode.A))
                    _inputState.totalDelta += Time.deltaTime * new Vector2(-1f, 0) *100f;
                else if (Input.GetKey(KeyCode.D))
                    _inputState.totalDelta += Time.deltaTime * new Vector2(1f, 0) * 100f;
                else if (Input.GetKey(KeyCode.W))
                    _inputState.totalDelta += Time.deltaTime * new Vector2(0f, 1f) * 100f;
                else if (Input.GetKey(KeyCode.S))
                    _inputState.totalDelta += Time.deltaTime * new Vector2(0f, -1f) * 100f;
                else
                    _inputState.totalDelta = Vector2.zero;
            }
            else
            {
                if (Input.touchCount > 0)
                    HandleTouches();
            }
#pragma warning restore CS0162 // Обнаружен недостижимый код

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