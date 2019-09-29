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
    }
}