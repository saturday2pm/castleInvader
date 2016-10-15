using ProtocolCS;

class GameNetworkController : NetworkController
{
    void Init()
    {
        MatchSuccess lastSuccessMatch = MatchNetworkController.LastSuccessMatch;
        Init(UriBuilder.Create(lastSuccessMatch.gameServerAddress, lastSuccessMatch.senderId.ToString(), lastSuccessMatch.matchToken));
    }
}

