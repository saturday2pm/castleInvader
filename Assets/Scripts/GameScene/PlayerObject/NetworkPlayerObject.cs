using ProtocolCS;
using Simulator;
using System.Collections.Generic;
using UnityEngine;

public class NetworkPlayerObject : Simulator.Player
{
    public List<IngameEvent> InputEvent = new List<IngameEvent>();
    public List<IngameEvent> OutputEvent = new List<IngameEvent>();

    Match match;

    public override void Init(Match match)
    {

    }

    public override void Update(Match _match)
    {
        match = _match;
        foreach (var e in InputEvent)
        {
            HandleEvent(e);
        }
        InputEvent.Clear();
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
        var dstCastle = match.Castles[_event.to.id];
        srcCastle.Attack(dstCastle);
    }

    void OnUpgrade(UpgradeEvent _event)
    {
        var targetCastle = match.Castles[_event.castle.id];
        targetCastle.Upgrade();
    }

    void OnUnhandledEvent()
    {
        Debug.LogError("[" + Id.ToString() + "]" + "Player Got Unhandled Event!");
    }
}

