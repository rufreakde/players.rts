using System.Collections.Generic;
using manager.ioc;

namespace players.rts
{
    [System.Serializable]
    public class TeamDictionary : CustomDict<int, List<Player>>
    {
        public TeamDictionary() : base()
        {
        }

        /// <summary>
        /// Add a player to specific team
        /// </summary>
        /// <param name="_TeamId"></param>
        /// <param name="_PlayerToAdd"></param>
        /// <returns>New playercount of the team</returns>
        public int AddToSpecificTeam(int _TeamId, Player _PlayerToAdd)
        {
            List<Player> team;
            this.TryGetValue(_TeamId, out team);
            team.Add(_PlayerToAdd);
            this.AddOrReplace(_TeamId, team);
            return team.Count;
        }

        /// <summary>
        /// Remove a player from a specific team
        /// </summary>
        /// <param name="_TeamId"></param>
        /// <param name="_PlayerToRemove"></param>
        /// <returns>New playercount of the team</returns>
        public int RemoveFromSpecificTeam(int _TeamId, Player _PlayerToRemove)
        {
            List<Player> team;
            this.TryGetValue(_TeamId, out team);
            team.Remove(_PlayerToRemove);
            this.AddOrReplace(_TeamId, team);
            return team.Count;
        }
    }
}