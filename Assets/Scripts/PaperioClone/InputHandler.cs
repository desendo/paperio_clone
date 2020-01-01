using PaperIOClone.Installers;
using UnityEngine;
using Zenject;

namespace PaperIOClone
{
    public class InputHandler : ITickable
    {
        private readonly GameSettingsInstaller.DebugSettings _debugSettings;
        private readonly InputState _inputState;

        public InputHandler(InputState inputState, GameSettingsInstaller.DebugSettings debugSettings)
        {
            _inputState = inputState;
            _debugSettings = debugSettings;
        }

        public void Tick()
        {
            if (_debugSettings.useWASD)
            {
                if (Input.GetKey(KeyCode.A))
                    _inputState.TotalDelta += new Vector2(-1f, 0) * (Time.deltaTime * 100f);
                else if (Input.GetKey(KeyCode.D))
                    _inputState.TotalDelta += new Vector2(1f, 0) * (Time.deltaTime * 100f);
                else if (Input.GetKey(KeyCode.W))
                    _inputState.TotalDelta += new Vector2(0f, 1f) * (Time.deltaTime * 100f);
                else if (Input.GetKey(KeyCode.S))
                    _inputState.TotalDelta += new Vector2(0f, -1f) * (Time.deltaTime * 100f);
                else
                    _inputState.TotalDelta = Vector2.zero;
            }
            else
            {
                if (Input.touchCount > 0)
                    HandleTouches();
            }
        }

        private void HandleTouches()
        {
            var currentTouch = Input.touches[0];
            if (currentTouch.phase == TouchPhase.Moved)
                _inputState.TotalDelta += currentTouch.deltaPosition;
            else if (currentTouch.phase != TouchPhase.Stationary)
                _inputState.TotalDelta = Vector2.zero;
        }
    }
}