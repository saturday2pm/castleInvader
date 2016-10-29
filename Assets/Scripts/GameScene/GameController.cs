using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using Simulator;
using Newtonsoft.Json;
using System.Linq;
using ProtocolCS;

public class GameController : MonoBehaviour
{
    Dictionary<int, Simulator.Player> playerDictionary = new Dictionary<int, Simulator.Player>();
    MatchOption option;
    Match match;

    public MapController MapController;
    public int PlayerCount;
    public int PlayFrame;

    void Start()
    {
        //데이터 받기
        string optionPath = Application.dataPath + "/Resources/" + "Option.json";
        if (!File.Exists(optionPath))
            throw new FileLoadException("Option.json is not exist!");

        string optionStr = File.ReadAllText(optionPath);
        option = JsonConvert.DeserializeObject<MatchOption>(optionStr);

        //매치 데이터 없는경우 싱글 게임을 위한 AI 플레이어 생성
        if (MatchModule.LastSuccessMatch == null)
        {
            List<Simulator.Player> testPlayers = new List<Simulator.Player>();
            for (int i = 0; i < PlayerCount; i++)
            {
                Simulator.Player player;
                if (i == 0)
                {
                    player = new SingleUserPlayerObject();
                    player.Name = "User";
                }
                else
                {
                    player = new AIPlayerObject();
                    player.Name = "AI" + player.Id.ToString();
                }

                player.Id = i + 1;
                testPlayers.Add(player);
            }
            BeginPlay(testPlayers, -1);
        }
    }

    public bool BeginPlay(List<Simulator.Player> _players, long _seed)
    {
        if (option == null || _players == null || _players.Count < 1)
            return false;

        foreach(var player in _players)
        {
            playerDictionary[player.Id] = player;
        }

        //매치 시뮬레이터 초기화
        match = new Match(option, _players.Cast<Simulator.Player>().ToList(), (int)_seed);
        match.Init();

        //맵 컨트롤러 사이즈 설정
        MapController.SetMapSize(new Vector2(option.Width, option.Height));

        //매치 데이터 없는경우 싱글 게임을 위한 코루틴 시작
        if(MatchModule.LastSuccessMatch == null)
            StartCoroutine(UpdatePlayFrameByTime());

        return true;
    }

    public Frame UpdatePlayFrameByNetwork(Frame _frame)
    {
        foreach(var e in _frame.events)
        {
            var player = playerDictionary[e.player.id] as NetworkPlayerObject;
            if(player != null)
            {
                player.InputEvent.Add(e);
            }
        }

        UpdateGame();

        var outEventList = new List<IngameEvent>();
        foreach (var player in playerDictionary.Values)
        {
            var p = player as NetworkPlayerObject;
            if(p != null)
            {
                outEventList.AddRange(p.OutputEvent);
                p.OutputEvent.Clear();
            }
        }

        return new Frame()
        {
            frameNo = _frame.frameNo,
            events = outEventList.ToArray(),
            senderId = _frame.senderId
        };
    }

    IEnumerator UpdatePlayFrameByTime()
    {
        float deltaTime = (1.0f / PlayFrame);

        while (true)
        {
            if (match.IsEnd())
                break;

            UpdateGame();

            yield return new WaitForSeconds(deltaTime);
        }
    }

    private void UpdateGame()
    {
        match.Update();

        foreach (var castle in match.Castles)
        {
            var castleView = MapController.GetCastleObject(castle);
            Color playerColor = Color.white;
            bool isUserCastle = false;
            if (castle.Owner != null)
            {
                playerColor = PlayerColorSelector.GetColorById(castle.Owner.Id);
                isUserCastle = castle.Owner is SingleUserPlayerObject;
            }
            castleView.UpdateCastle(castle.UnitNum, castle.Radius, playerColor, castle.IsUpgradable, isUserCastle);
        }

        foreach (var unitQueue in match.Units.Values)
        {
            foreach (var unit in unitQueue)
            {
                var unitView = MapController.GetUnitObject(unit);
                unitView.UpdateUnit(new Vector2(unit.Pos.X, unit.Pos.Y), unit.Num);
            }
        }

        foreach (var matchEvent in match.EventQueue)
        {
            HandleMatchEvent(matchEvent);
        }
    }

    void HandleMatchEvent(MatchEvent _event)
    {
        TypeSwitch.Do(_event,
            TypeSwitch.Case<UnitDeadEvent>(OnUnitDead),
            TypeSwitch.Case<PlayerDeadEvent>(OnPlayerDead),
            TypeSwitch.Default(OnUnhandledEvent));
    }

    private void OnUnitDead(UnitDeadEvent _event)
    {
        MapController.RemoveUnitObject(_event.UnitId);
    }

    private void OnPlayerDead(PlayerDeadEvent _event)
    {

    }

    private void OnUnhandledEvent()
    {
        Debug.LogError("Unhandled Match Event Occur!");
    }
}
