using System.Collections.Generic;

namespace Game
{
    public class PlayersRegistry
    {
        readonly List<PlayerFacade> _players = new List<PlayerFacade>();
        readonly List<PlayerZone> _zones = new List<PlayerZone>();
        readonly List<PlayerLine> _lines = new List<PlayerLine>();
        

        public IEnumerable<PlayerFacade> Players
        {
            get { return _players; }
        }
        public List<PlayerZone> Zones
        {
            get { return _zones; }
        }
        public void AddPlayer(PlayerFacade player)
        {
            _players.Add(player);
            _zones.Add(player.Zone);
            _lines.Add(player.Line);

        }

        public void RemovePlayer(PlayerFacade player)
        {
            _players.Remove(player);
            _zones.Remove(player.Zone);
            _lines.Remove(player.Line);
        }
    }
}
