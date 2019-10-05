using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using manager.ioc;

namespace players.rts
{
    public class PlayerLobbyTestScript : MonoBehaviour
    {
        PlayerManager playerManager;
        // Start is called before the first frame update
        public Player currentSetPlayer = new Player();
        public int leavingPlayer = 0;

        void Start()
        {
            playerManager = (PlayerManager)MANAGER.GET.GetSingleton(typeof(PlayerManager));
        }


        public void joinLobby()
        {
            Player player = new Player(currentSetPlayer);
            playerManager.AddPlayer(player);
        }

        void leaveLobby()
        {
            // TODO
        }
    }
}

