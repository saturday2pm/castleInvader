using UnityEngine;
using System.Collections.Generic;
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

        List<Player> players = new List<Player>();
        for (int i = 0; i < PlayerCount; i++)
        {
            players.Add(new AI());
        }

        match = new Match(option, players);
        match.Init();

        MapController.SetMapSize(new Vector2(option.Width, option.Height));
  
        foreach(var castle in match.Castles)
        {
            var id = castle.Id;
            var pos = new Vector2(castle.Pos.X, castle.Pos.Y);
            var count = castle.UnitNum;
            var size = castle.Radius;

            MapController.MakeCastleObject(id, pos, count, size);
        }

        timer = new Timer(UpdatePlayFrame, null, Timeout.Infinite, PlayFrame);
    }

    void UpdatePlayFrame(object _obj)
    {
        if (match.IsEnd())
            return;

        match.Update();
    }

    void InvalidateDraw()
    {

    }
}
