using manager.ioc;
using UnityEngine;
using System.Collections.Generic;

namespace players.rts
{
    public class PlayerManager : MonoBehaviour, IamSingleton
    {
        [SerializeField]
        [ReadOnly(true)]
        protected PlayerDictionary PlayerLobby = new PlayerDictionary();
        [SerializeField]
        [ReadOnly(true)]
        protected TeamDictionary Teams = new TeamDictionary();


        public void iInitialize()
        {
            // do we need another script for players?
            // I guess not since there is no visual aspect for player management
        }

        /// <summary>
        /// Since every Player stores his own team ID the only thing we need to do is to keep this array up to date.
        /// So every time a player betrays others and joins another faction just call this here and all the teams will be updated accordingly.
        /// WARNING: Remember that if you create a UI where you can change those settings you have to "mirror" the current state with a "deep copy".
        /// Then after the user changed the "deep copy" override the corresponding Player in 'PlayerLobby' with it. Afterwards call this function.
        /// </summary>
        public void updateTeamDict()
        {
            // TODO Extend the CustomDict class for iterator and other needed normal Dict functions!

            /*Dictionary<int, List<Player>> changedTeams = new Dictionary<int, List<Player>>();

            foreach (KeyValuePair<string, Player> player in PlayerLobby)
            {
                changedTeams[player.Value.Team.Id].Add(player.Value);
            }

            Teams = changedTeams;*/
        }

        /// <summary>
        /// When creating: usually only lobby creator (as local player) 
        /// But who knows maybe you have a system to create lobbies directly with friends via steam for example (think about local players then as well!)
        /// When joining: give the local player every other slots of players and ofc himself (giving the local player flag)
        /// </summary>
        /// <param name="_AllPlayersFromLobby"></param>
        public void initPlayerDict(Dictionary<string, Player> _AllPlayersFromLobby)
        {
            //PlayerLobby = _AllPlayersFromLobby;
        }

        /// <summary>
        /// Joins Lobby
        /// </summary>
        /// <param name="_Player"></param>
        public void addPlayer(Player _Player)
        {
            //PlayerLobby[_Player.Playername] = _Player;
        }

        /// <summary>
        /// Left Lobby
        /// </summary>
        /// <param name="_Player"></param>
        public void removePlayer(Player _Player)
        {
            PlayerLobby.Remove(_Player.Playername);
        }
        // TODO we need Teams
        // TODO we need to manage those
    }

    [System.Serializable]
    public class PlayerTeamSettings
    {
        public int Id = -1; // means FFA no team
        public bool AllowsAlliedVisibility = true;
        public bool AllowsAlliedControl = false;
        public bool CanTrade = true;

        public PlayerTeamSettings() : this(-1, true, false, true){}

        public PlayerTeamSettings(int _TeamID, bool _AllowsAlliedVisibility, bool _AllowsAlliedControl, bool _CanTrade)
        {
            this.Id = _TeamID;
            this.AllowsAlliedVisibility = _AllowsAlliedVisibility;
            this.AllowsAlliedControl = _AllowsAlliedControl;
            this.CanTrade = _CanTrade;
        }
    }

    [System.Serializable]
    public class Player
    {
        public string Playername = "Unkown";
        public int LobbySlot = -1;
        public PlayerTeamSettings Team = new PlayerTeamSettings(-1, true, false, true );
        public Color PlayerColor = Color.red;
        public bool IsLocalPlayer = false;
        public bool AI = false;
        public Dictionary<string, int> Resources = new Dictionary<string, int>();
        public Dictionary<string, string> ChosenSettings = new Dictionary<string, string>();

        public Player() : this("Unkown",  -1, new PlayerTeamSettings(),  Color.red,  false){}

        public Player(bool _isLocalPlayer) : this("Unkown", -1, new PlayerTeamSettings(), Color.red, _isLocalPlayer){}

        public Player(string _Name, int _LobbySlot, PlayerTeamSettings _Team, Color _Color, bool _IsLocalPlayer) : this( _Name,  _LobbySlot,  _Team,  _Color,  _IsLocalPlayer, false) {}

        public Player(string _Name, int _LobbySlot, PlayerTeamSettings _Team, Color _Color, bool _IsLocalPlayer, bool _IsAI)
        {
            Playername = _Name;
            LobbySlot = _LobbySlot;
            Team = _Team;
            PlayerColor = _Color;
            IsLocalPlayer = _IsLocalPlayer;
            Resources = new Dictionary<string, int>();
            ChosenSettings = new Dictionary<string, string>(); // not intedet to be set during a game lol
            AI = _IsAI;


            // TODO export this in a .env file maybe or another settings file?
            //These are initial game start values
            //Example Resources I work with but you can add what ever you want here:
            Resources.Add("gold", 500);
            Resources.Add("wood", 200);
            Resources.Add("food", 0); // this is the current value the player has I subscribe OnEnable and OnDisable of buildings that provide this "food"
            //Example settings I work with but you can add what ever you want here:
            ChosenSettings.Add("race", "orc");
            ChosenSettings.Add("handycap", "100");
        }


        /// <summary>
        /// If Empty then creating Dict entry otherwise it will add the _Amount on top. Negative values will lower the ammount. Resource can be negative!! (example : energy)
        /// </summary>
        /// <param name="_ResourceName"> example: wood</param>
        /// <param name="_Amount">negative values will substract the ammount</param>
        /// <returns></returns>        
        public int add(string _ResourceName, int _Amount)
        {
            if (Resources.ContainsKey(_ResourceName))
            {
                Resources[_ResourceName] += _Amount;
                return Resources[_ResourceName];
            }
            else
            {
                Resources[_ResourceName] = _Amount;
                return Resources[_ResourceName];
            }
        }

        /// <summary>
        /// If Empty then creating Dict entry otherwise it will add the _Amount on top. Negative values will lower the ammount. Resource can NOT be negative!!
        /// </summary>
        /// <param name="_ResourceName"> example: wood</param>
        /// <param name="_Amount">negative values will substract the ammount</param>
        /// <returns></returns>        
        public int addNoBelowZero(string _ResourceName, int _Amount)
        {
            if (Resources.ContainsKey(_ResourceName))
            {
                if (_Amount <= 0 && Mathf.Abs(_Amount) >= Resources[_ResourceName])
                {
                    return Resources[_ResourceName];
                }
                else
                {
                    Resources[_ResourceName] += _Amount;
                    return Resources[_ResourceName];
                }
            }
            else
            {
                if (_Amount > 0)
                {
                    Resources[_ResourceName] = _Amount;
                }
                else
                {
                    Resources[_ResourceName] = 0;
                }
                return Resources[_ResourceName];
            }
        }
    }
}