using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Game
{
    public interface IBotState
    {
        void EnterState();
        void ExitState();
        void Update();
        void FixedUpdate();
    }

    public enum BotState
    {
        Grow,
        Attack,
        Retreat,
        None
    }
    public class BotStateManager :  ITickable, IFixedTickable, IInitializable
    {
        IBotState _currentStateHandler;
        BotState _currentState = BotState.None;
        List<IBotState> _states;

        [Inject]
        public void Construct(
            BotStateGrow grow, BotStateAttack attack, BotStateRetreat retreat
            )
        {
           _states = new List<IBotState>{ grow, attack, retreat };
        }
        public BotState CurrentState
        {
            get { return _currentState; }
        }
        public void Initialize()
        {
            ChangeState(BotState.Grow);
        }
        public void ChangeState(BotState state)
        {
            if (_currentState == state)
            {
                // Already in state
                return;
            }
            _currentState = state;

            if (_currentStateHandler != null)
            {
                _currentStateHandler.ExitState();
                _currentStateHandler = null;
            }

            _currentStateHandler = _states[(int)state];
            _currentStateHandler.EnterState();
        }

        public void Tick()
        {
            _currentStateHandler.Update();
        }

        public void FixedTick()
        {
            _currentStateHandler.FixedUpdate();
        }
    }
}