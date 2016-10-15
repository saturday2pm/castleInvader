using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace ProtocolCS
{
    public class Player
    {
        public int id { get; set; }

        public string name { get; set; }
    }

    /// <summary>
    /// 병력이 이동 가능한 단위
    /// </summary>
    public class Waypoint
    {
        public int id { get; set; }
    }

    public enum CastleType
    {
        Normal,
        Medium,
        Large
    }
    public class Castle : Waypoint
    {
        public CastleType type { get; set; }
    }
}
