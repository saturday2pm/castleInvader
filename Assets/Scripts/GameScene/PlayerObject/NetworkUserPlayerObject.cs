using ProtocolCS;
using Simulator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class NetworkUserPlayerObject : NetworkPlayerObject
{
    static InputController InputController;
    
    public override void Init(Match match)
    {
        if(!InputController)
        {
            InputController = GameObject.FindObjectOfType<InputController>();
        }

        InputController.OnMove += OnMoveInput;
        InputController.OnUpgrade += OnUpgradeInput;
    }

    void OnMoveInput(int src, int dst)
    {
        if (OwnCastles.Exists(castle => castle.Id == src))
        {
            OutputEvent.Add(new MoveEvent()
            {
                from = new ProtocolCS.Waypoint() { id = src },
                to = new ProtocolCS.Waypoint() { id = dst },
                player = new ProtocolCS.Player() { id = Id },
            });
        }
    }

    void OnUpgradeInput(int target)
    {
        var castle = OwnCastles.Find(c => c.Id == target);
        if (castle != null)
        {
            OutputEvent.Add(new UpgradeEvent()
            {
                castle = castle.ToProtocolCastle(),
                upgradeTo = (CastleType)castle.Level + 1,
                player = new ProtocolCS.Player() { id = Id },
            });
        }
    }
}
