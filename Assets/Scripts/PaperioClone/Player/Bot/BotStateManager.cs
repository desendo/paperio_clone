using System.Collections.Generic;
using PaperIOClone.Player.Bot.States;
using Zenject;

namespace PaperIOClone.Player.Bot
{
    public enum BotState
    {
        Grow,
        Attack,
        Retreat,
        None
    }

    public class BotStateManager : ITickable, IFixedTickable, IInitializable
    {
        private IBotState _currentStateHandler;
        private List<IBotState> _states;

        private BotState CurrentState { get; set; } = BotState.None;

        public void FixedTick()
        {
            _currentStateHandler.FixedUpdate();
        }

        public void Initialize()
        {
            ChangeState(BotState.Grow);
        }

        public void Tick()
        {
            _currentStateHandler.Update();
        }

        [Inject]
        public void Constructor(BotStateGrow grow, BotStateAttack attack, BotStateRetreat retreat)
        {
            _states = new List<IBotState> {grow, attack, retreat};
        }

        public void ChangeState(BotState state)
        {
            if (CurrentState == state) return;
            CurrentState = state;

            if (_currentStateHandler != null)
            {
                _currentStateHandler.ExitState();
                _currentStateHandler = null;
            }

            _currentStateHandler = _states[(int) state];
            _currentStateHandler.EnterState();
        }
    }
}