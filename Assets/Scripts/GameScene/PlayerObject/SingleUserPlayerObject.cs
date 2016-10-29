using ProtocolCS;
using Simulator;
using System.Collections.Generic;
using UnityEngine;
using System;

class SingleUserPlayerObject : Simulator.Player
{
    public List<IngameEvent> InputEvent = new List<IngameEvent>();
    static InputController InputController;

    public override void Init(Match _match)
    {
        if (!InputController)
        {
            InputController = GameObject.FindObjectOfType<InputController>();
        }

        InputController.OnMove += OnMoveInput;
        InputController.OnUpgrade += OnUpgradeInput;
    }


    public override void Update(Match _match)
    {    
        foreach(var e in InputEvent)
        {
            HandleEvent(e, _match);
        }
        InputEvent.Clear();
    }

    void HandleEvent(IngameEvent _event, Match _match)
    {
        TypeSwitch.Do(_event,
            TypeSwitch.Case<MoveEvent>(x => { OnMove(x, _match); }),
            TypeSwitch.Case<UpgradeEvent>(x => { OnUpgrade(x, _match); }),
            TypeSwitch.Default(() => { OnUnhandledEvent(_match); }));
            
    }

    private void OnUnhandledEvent(Match _match)
    {
        throw new NotImplementedException();
    }

    private void OnUpgrade(UpgradeEvent _event, Match _match)
    {
        int castleId = _event.castle.id;
        Simulator.Castle castle = OwnCastles.Find(Castle => Castle.Id == castleId);
        castle.Upgrade();
    }

    private void OnMove(MoveEvent _event, Match _match)
    {
        int srcId = _event.from.id;
        int dstId = _event.to.id;
        Simulator.Castle srcCastle = null;
        Simulator.Castle dstCastle = null;
        foreach (var castle in _match.Castles)
        {
            if (castle.Id == srcId)
                srcCastle = castle;
            else if (castle.Id == dstId)
                dstCastle = castle;
        }

        if (srcCastle != null && dstCastle != null)
        {
            srcCastle.Attack(dstCastle);
        }
    }

    private void OnUpgradeInput(int target)
    {
        UpgradeEvent e = new UpgradeEvent();
        Simulator.Castle c = OwnCastles.Find(castle => castle.Id == target);
        if (c != null)
        {
            e.castle = c.ToProtocolCastle();
            e.upgradeTo = (ProtocolCS.CastleType)c.Level;
            e.player = ToProtocolPlayer();
            InputEvent.Add(e);
        }
        
    }

    private void OnMoveInput(int src, int dst)
    {
        if (OwnCastles.Exists(castle => castle.Id == src))
        {
            InputEvent.Add(new MoveEvent()
            {
                from = new ProtocolCS.Waypoint() { id = src },
                to = new ProtocolCS.Waypoint() { id = dst },
                player = new ProtocolCS.Player() { id = Id },
            });
        }
    }

    public ProtocolCS.Player ToProtocolPlayer()
    {
        ProtocolCS.Player player = new ProtocolCS.Player();
        player.id = Id;
        player.name = Name;
        return player;
    }
}