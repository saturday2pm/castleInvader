using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Threading;
using System.IO;
using Simulator;
using Newtonsoft.Json;

public class GameController : MonoBehaviour
{
    List<Player> players;
    Match match;
    Timer timer;

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
        var option = JsonConvert.DeserializeObject<MatchOption>(optionStr);

        //플레이어 생성
        List<Player> players = new List<Player>();
        for (int i = 0; i < PlayerCount; i++)
        {
            var player = new AI();
            player.Id = i + 1;
            players.Add(player);
        }

        //매치 시뮬레이터 초기화
        match = new Match(option, players);
        match.Init();

        //맵 컨트롤러 사이즈 설정
        MapController.SetMapSize(new Vector2(option.Width, option.Height));

        //게임 시작
        StartCoroutine(UpdatePlayFrame());
    }

    IEnumerator UpdatePlayFrame()
    {
        float deltaTime = PlayFrame * (1.0f / 60.0f);

        while (true)
        {
            if (match.IsEnd())
                break;

            match.Update();

            foreach (var castle in match.Castles)
            {
                var castleView = MapController.GetCastleView(castle);
                int ownerId = 0;
                if (castle.Owner != null)
                    ownerId = castle.Owner.Id;

                castleView.UpdateCastle(castle.UnitNum, castle.Radius, PlayerColorSelector.GetColorByNumber(ownerId));
            }

            foreach (var unitQueue in match.Units.Values)
            {
                foreach (var unit in unitQueue)
                {
                    var unitView = MapController.GetUnitView(unit);
                    unitView.UpdateUnit(new Vector2(unit.Pos.X, unit.Pos.Y), unit.Num, deltaTime);
                }
            }

            foreach(var gameEvent in match.EventQueue)
            {
                HandleEvent(gameEvent);
            }

            yield return new WaitForSeconds(deltaTime);
        }
    }

    void HandleEvent(GameEvent e)
    {
        if(e is UnitDeadEvent)
        {
            MapController.RemoveUnitObject((e as UnitDeadEvent).UnitId);
        }
    }
}
