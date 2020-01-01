using System.Collections.Generic;
using PaperIOClone.Player;

namespace PaperIOClone
{
    public class PlayersRegistry
    {
        public List<PlayerLine> Lines { get; } = new List<PlayerLine>();
        public List<PlayerFacade> PlayerFacades { get; } = new List<PlayerFacade>();
        public List<PlayerZone> Zones { get; } = new List<PlayerZone>();

        public void AddPlayer(PlayerFacade player)
        {
            PlayerFacades.Add(player);
            Zones.Add(player.Zone);
            Lines.Add(player.Line);
        }

        public void RemovePlayer(PlayerFacade player)
        {
            PlayerFacades.Remove(player);
            Zones.Remove(player.Zone);
            Lines.Remove(player.Line);
        }
    }
}