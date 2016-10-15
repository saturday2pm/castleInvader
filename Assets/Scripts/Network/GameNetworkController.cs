using ProtocolCS;
using System.Linq;
using UnityEngine;

class GameNetworkController : NetworkController
{
    public GameController GameController;
    public MatchSuccess MatchData { get; set; }
    public bool Init()
    {
        MatchData = MatchNetworkController.LastSuccessMatch;
        if (MatchData == null)
            return false;

        Init(UriBuilder.Create(MatchData.gameServerAddress, MatchData.senderId.ToString(), MatchData.matchToken));

        PacketHelper.AddHandler<StartGame>(OnStartGame);

        return true;
    }

    public bool RequestJoinGame()
    {
        if (!IsAlive)
            return false;

        var joinReq = new JoinGame() { matchToken = MatchData.matchToken };
        Send(joinReq);

        return true;
    }

    private void OnStartGame(StartGame startData)
    {
        //GameController.Init(, startData.seed);
    }


}

