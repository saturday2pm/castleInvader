using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace ProtocolCS
{
    public class IngameEvent
    {
        public Player player { get; set; }
    }

    public class MoveEvent : IngameEvent
    {
        public Waypoint from { get; set; }
        public Waypoint to { get; set; }
    }

    public class UpgradeEvent : IngameEvent
    {
        public Castle castle { get; set; }
        
        public CastleType upgradeTo { get; set; }
    }
}
