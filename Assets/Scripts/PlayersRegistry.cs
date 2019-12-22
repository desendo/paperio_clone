using System.Collections.Generic;

namespace Game
{
    public class PlayersRegistry
    {
        readonly List<PlayerFacade> _players = new List<PlayerFacade>();

        public IEnumerable<PlayerFacade> Players
        {
            get { return _players; }
        }

        public void AddPlayer(PlayerFacade player)
        {
            _players.Add(player);
        }

        public void RemovePlayer(PlayerFacade player)
        {
            _players.Remove(player);
        }
    }
}
