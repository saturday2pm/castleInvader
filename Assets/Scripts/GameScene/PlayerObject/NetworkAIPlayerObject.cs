using ProtocolCS;
using Simulator;
using System;

public class NetworkAIPlayerObject : NetworkPlayerObject
{
    public override void Init(Match match)
    {

    }

    public override void Update(Match _match)
    {
        base.Update(_match);
        foreach (var castle in OwnCastles)
        {
            CastleUpdate(_match, castle);
        }
    }

    void CastleUpdate(Match match, Simulator.Castle _castle)
    {
        if (_castle.IsUpgradable && _castle.UnitNum > _castle.Cost * 1.2)
            Upgrade(_castle);

        Simulator.Castle emptyCastle = null;
        float emptyValue = 987654321;
        Simulator.Castle weekCastle = null;
        float weekValue = 987654321;
        Simulator.Castle allyCastle = null;
        float allyValue = 987654321;

        float maxDist = 400.0f;

        foreach (var c in match.Castles)
        {
            if (c == _castle)
                continue;

            var dist = (float)Math.Sqrt(c.Pos.DistSquare(_castle.Pos));
            float value = c.UnitNum + dist;

            if (c.Owner == null && dist < maxDist && emptyValue > dist)
            {
                emptyCastle = c;
                emptyValue = dist;
            }

            if (c.Owner != _castle.Owner && dist < maxDist && weekValue > value)
            {
                weekCastle = c;
                weekValue = value;
            }

            if (c.Owner == _castle.Owner && dist < maxDist && c.UnitNum < _castle.UnitNum && allyValue < value)
            {
                allyCastle = c;
                allyValue = value;
            }
        }

        if (emptyCastle != null)
        {
            Attack(_castle, emptyCastle);
            return;
        }

        if (weekCastle != null)
        {
            Attack(_castle, weekCastle);
            return;
        }

        if (allyCastle != null)
        {
            Attack(_castle, allyCastle);
            return;
        }

        //if (castle.EndPoint.Count > 0)
            //castle.CancelAttack(castle.EndPoint[0]);
    }

    void Attack(Simulator.Castle castle, Simulator.Castle end)
    {
        OutputEvent.Add(new MoveEvent()
        {
            from = new ProtocolCS.Waypoint() { id = castle.Id },
            to = new ProtocolCS.Waypoint() { id = end.Id },
            player = new ProtocolCS.Player() { id = castle.Owner.Id },
        });
    }


    void Upgrade(Simulator.Castle castle)
    {
        OutputEvent.Add(new UpgradeEvent()
        {
            castle = new ProtocolCS.Castle() { id = castle.Id, type = (CastleType)(castle.Level + 1) },
            player = new ProtocolCS.Player() { id = castle.Owner.Id },
        });
    }
}
