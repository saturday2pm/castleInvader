using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Simulator
{
    public class GameEvent
    {
    }

    public class UnitDeadEvent : GameEvent
    {
        public UnitDeadEvent(int _deadUnitId)
        {
            UnitId = _deadUnitId;
        }

        public int UnitId { get; private set; }
    }

    public class PlayerDeadEvent : GameEvent
    {
        public PlayerDeadEvent(int _deadPlayerId)
        {
            PlayerId = _deadPlayerId;
        }

        public int PlayerId { get; private set; }
    }
}
