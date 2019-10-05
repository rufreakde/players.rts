using manager.ioc;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;

namespace players.rts
{
    public class PlayerManager : MonoBehaviour, IamSingleton
    {
        [Mandatory]
        public GameObject DefaultEventSystemPrefab;
        [AutoAssign]
        public EventSystem EventSystem;

        [Mandatory]
        public GameObject DefaultPlayerUiPrefab;
        [AutoAssign]
        public GameObject PlayersUiCanvasPanel;

        [SerializeField]
        [ReadOnly(true)]
        protected PlayerDictionary PlayerLobby = new PlayerDictionary();
        [SerializeField]
        [ReadOnly(true)]
        protected TeamDictionary Teams = new TeamDictionary();

        public void iInitialize()
        {
            EventSystem = MANAGER.CheckScriptAvailability<EventSystem>("EventSystem", DefaultEventSystemPrefab);

            PlayersUiCanvasPanel = MANAGER.CheckGameObjectAvailability("PlayersCanvas", DefaultPlayerUiPrefab);
        }


        /// <summary>
        /// Since every Player stores his own team ID the only thing we need to do is to keep this array up to date.
        /// So every time a player betrays others and joins another faction just call this here and all the teams will be updated accordingly.
        /// WARNING: Remember that if you create a UI where you can change those settings you have to "mirror" the current state with a "deep copy".
        /// Then after the user changed the "deep copy" override the corresponding Player in 'PlayerLobby' with it. Afterwards call this function.
        /// </summary>
        public void OverrideTeamSettings(TeamDictionary _Clone)
        {
            Teams = _Clone;
        }

        /// <summary>
        /// Use this for views where people can modify team stats to get a clone and in the end override the TeamSettings with the clone.
        /// </summary>
        /// <param name="_AllPlayersFromLobby"></param>
        public TeamDictionary GetTeamCloneForTempViews()
        {
            TeamDictionary clone = new TeamDictionary();
            foreach (List<Player> team in Teams)
            {
                foreach (Player player in team)
                {
                    clone.AddToSpecificTeam(player.Team.Id, player);
                }
            }
            return clone;
        }

        /// <summary>
        /// Use this for views where people can modify player to get a clone and in the end override the playerLobby with the clone.
        /// </summary>
        /// <param name="_AllPlayersFromLobby"></param>
        public PlayerDictionary GetLobbyCloneForTempViews()
        {
            PlayerDictionary clone = new PlayerDictionary();
            foreach (Player player in PlayerLobby)
            {
                clone.Add(player.PlayerName, new Player(player));
            }
            return clone;
        }

        /// <summary>
        /// Use this to override lobbies with temp view lobbies.
        /// </summary>
        /// <param name="_Clone"></param>
        public void OverrideLobby(PlayerDictionary _Clone)
        {
            PlayerLobby = _Clone;
        }

        /// <summary>
        /// Joins Lobby
        /// </summary>
        /// <param name="_Player"></param>
        public void AddPlayer(Player _Player)
        {
            PlayerLobby.Add(_Player.PlayerName, _Player);
        }

        /// <summary>
        /// Left Lobby
        /// </summary>
        /// <param name="_Player"></param>
        public void RemovePlayer(Player _Player)
        {
            PlayerLobby.Remove(_Player.PlayerName);
        }
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
        public string PlayerName = "Unkown";
        public int LobbySlot = -1;
        public PlayerTeamSettings Team = new PlayerTeamSettings(-1, true, false, true );
        public Color PlayerColor = Color.red;
        public bool IsLocalPlayer = false;
        public bool AI = false;
        public Dictionary<string, int> Resources = new Dictionary<string, int>();
        public Dictionary<string, string> ChosenSettings = new Dictionary<string, string>();

        public Player() : this("Unkown",  -1, new PlayerTeamSettings(),  Color.red,  false){}

        public Player(Player _ToClone)
        {
            PlayerName = _ToClone.PlayerName;
            LobbySlot = _ToClone.LobbySlot;
            Team = _ToClone.Team;
            PlayerColor = _ToClone.PlayerColor;
            IsLocalPlayer = _ToClone.IsLocalPlayer;
            Resources = new Dictionary<string, int>();
            ChosenSettings = new Dictionary<string, string>(); // not intedet to be set during a game lol
            AI = _ToClone.AI;


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

        public Player(bool _IsLocalPlayer) : this("Unkown", -1, new PlayerTeamSettings(), Color.red, _IsLocalPlayer){}

        public Player(string _Name, int _LobbySlot, PlayerTeamSettings _Team, Color _Color, bool _IsLocalPlayer) : this( _Name,  _LobbySlot,  _Team,  _Color,  _IsLocalPlayer, false) {}

        public Player(string _Name, int _LobbySlot, PlayerTeamSettings _Team, Color _Color, bool _IsLocalPlayer, bool _IsAI)
        {
            PlayerName = _Name;
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
        public int AddResource(string _ResourceName, int _Amount)
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
        public int AddResourceNotBelowZero(string _ResourceName, int _Amount)
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