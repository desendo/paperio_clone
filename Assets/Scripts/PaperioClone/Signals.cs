using PaperIOClone.Player;

namespace PaperIOClone
{
    public struct SignalDie
    {
        public PlayerFacade killer;
        public PlayerFacade victim;
    }

    public struct SignalZoneChanged
    {
    }
}