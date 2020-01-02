using PaperIOClone.Installers;
using UnityEngine;
using Zenject;

namespace PaperIOClone
{
    public class InputHandler : ITickable
    {
        private readonly AngleState _angleState;
        private readonly GameSettingsInstaller.DebugSettings _debugSettings;

        public InputHandler(AngleState angleState, GameSettingsInstaller.DebugSettings debugSettings)
        {
            _angleState = angleState;
            _debugSettings = debugSettings;
        }

        public void Tick()
        {
            if (_debugSettings.useWASD)
            {
                if (Input.GetKey(KeyCode.A))
                    _angleState.TotalDelta += new Vector2(-1f, 0) * (Time.deltaTime * 10f);
                else if (Input.GetKey(KeyCode.D))
                    _angleState.TotalDelta += new Vector2(1f, 0) * (Time.deltaTime * 10f);
                else if (Input.GetKey(KeyCode.W))
                    _angleState.TotalDelta += new Vector2(0f, 1f) * (Time.deltaTime * 10f);
                else if (Input.GetKey(KeyCode.S))
                    _angleState.TotalDelta += new Vector2(0f, -1f) * (Time.deltaTime * 10f);
                _angleState.TotalDelta.Normalize();
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
                _angleState.TotalDelta += currentTouch.deltaPosition;
            else if (currentTouch.phase != TouchPhase.Stationary)
                _angleState.TotalDelta.Normalize();
        }
    }
}