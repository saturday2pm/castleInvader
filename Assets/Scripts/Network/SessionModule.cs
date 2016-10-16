using ProtocolCS;
using System.Linq;
using UnityEngine.SceneManagement;
using WebSocketSharp;
using System;

class SessionModule : NetworkModule
{
    public GameController GameController;
    public MatchSuccess MatchData { get; set; }

    void Start()
    {
        MatchData = MatchModule.LastSuccessMatch;
        if (MatchData == null)
            return;

        Connect(ProtocolCS.UriBuilder.Create(MatchData.gameServerAddress, MatchData.senderId.ToString(), MatchData.matchToken));

        PacketHelper.AddHandler<StartGame>(OnStartGame);
        PacketHelper.AddHandler<CancelGame>(OnCancelGame);
        PacketHelper.AddHandler<Frame>(OnFrameUpdate);
    }

    public bool RequestJoinGame()
    {
        if (!IsAlive)
            return false;

        var joinReq = new JoinGame() { matchToken = MatchData.matchToken };
        Send(joinReq);

        return true;
    }

    protected override void OnOpen(object sender, EventArgs e)
    {
        RequestJoinGame();  
    }

    private void OnStartGame(StartGame _startData)
    {
        var playerObjects = _startData.players.ToList().ConvertAll(x => {
            return new NetworkAIPlayerObject()
            {
                Id = x.id,
                Name = x.name,
            };
        }).Cast<Simulator.Player>().ToList();

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

