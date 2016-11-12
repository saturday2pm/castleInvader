using ProtocolCS;
using System.Linq;
using UnityEngine.SceneManagement;

class SessionModule : NetworkModule
{
    public GameController GameController;
    public MatchSuccess MatchData { get; set; }

    protected override void OnStart()
    {
        MatchData = MatchModule.LastSuccessMatch;
        if (MatchData == null)
            return;

        Connect(UriBuilder.Create(MatchData.gameServerAddress,  UserType.Guset, MatchModule.CurrentPlayerId.ToString(), MatchData.matchToken));

        AddHandler<StartGame>(OnStartGame);
        AddHandler<CancelGame>(OnCancelGame);
        AddHandler<Frame>(OnFrameUpdate);
    }

    public bool RequestJoinGame()
    {
        if (!IsAlive)
            return false;

        var joinReq = new JoinGame() { matchToken = MatchData.matchToken };
        Send(joinReq);

        return true;
    }

    protected override void OnOpen()
    {
        RequestJoinGame();  
    }

    private void OnStartGame(StartGame _startData)
    {
        var playerObjects = _startData.players.ToList().ConvertAll(x => 
        {
            Simulator.Player player;
            if(x.id == MatchModule.CurrentPlayerId)
            {
                player = new NetworkUserPlayerObject()
                {
                    Id = x.id,
                    Name = x.name,
                };
            }
            else
            {
                player = new NetworkPlayerObject()
                {
                    Id = x.id,
                    Name = x.name,
                };
            }
            return player;
        }).ToList();

        GameController.BeginPlay(playerObjects, _startData.seed);

        var emptyFrame = new Frame() { events = new IngameEvent[] { }, senderId = MatchData.senderId };
        Send(emptyFrame);
    }

    private void OnCancelGame(CancelGame _cancleData)
    {
        Close();
        SceneManager.LoadScene("TitleScene");
    }

    private void OnFrameUpdate(Frame _frame)
    {
        Frame nextFrame = GameController.UpdatePlayFrameByNetwork(_frame);
        Send(nextFrame);
    }
}

