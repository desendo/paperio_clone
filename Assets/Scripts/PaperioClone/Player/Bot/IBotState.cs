namespace PaperIOClone.Player.Bot
{
    public interface IBotState
    {
        void EnterState();
        void ExitState();
        void Update();
        void FixedUpdate();
    }
}