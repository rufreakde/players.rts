using manager.ioc;

namespace players.rts
{
    [System.Serializable]
    public class PlayerDictionary : CustomDict<string, Player>
    {
        public PlayerDictionary() : base()
        {
            //other stuff here
        }
    }
}