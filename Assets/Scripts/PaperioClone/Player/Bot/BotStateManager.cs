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
        private List<IBotState> _states;
        private IBotState _currentStateHandler;

        [Inject]
        public void Constructor(BotStateGrow grow, BotStateAttack attack, BotStateRetreat retreat)
        {
            _states = new List<IBotState> {grow, attack, retreat};
        }

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