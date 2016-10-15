using ProtocolCS;
using Simulator;
using System.Collections.Generic;
using UnityEngine;

public class NetworkPlayerObject : Simulator.Player
{
    public List<IngameEvent> EventQueue = new List<IngameEvent>();
    Match match;

    public override void Init(Match match)
    {

    }

    public override void Update(Match _match)
    {
        match = _match;
        foreach (var e in EventQueue)
        {
            HandleEvent(e);
        }
        EventQueue.Clear();
    }

    void HandleEvent(IngameEvent _event)
    {
        TypeSwitch.Do(_event,
            TypeSwitch.Case<MoveEvent>(OnMove),
            TypeSwitch.Case<UpgradeEvent>(OnUpgrade),
            TypeSwitch.Default(OnUnhandledEvent));
    }

    void OnMove(MoveEvent _event)
    {
        var srcCastle = match.Castles[_event.from.id];
        srcCastle.Attack(new Simulator.Waypoint(_event.to.id));
    }

    void OnUpgrade(UpgradeEvent _event)
    {
        var targetCastle = match.Castles[_event.castle.id];
        CastleType src = _event.castle.type;
        CastleType dst = _event.upgradeTo;
        int upgradeStep = dst - src;
        for (int i = 0; i < upgradeStep; ++i)
        {
            targetCastle.Upgrade();
        }
    }

    void OnUnhandledEvent()
    {
        Debug.LogError("[" + Id.ToString() + "]" + "Player Got Unhandled Event!");
    }
}

