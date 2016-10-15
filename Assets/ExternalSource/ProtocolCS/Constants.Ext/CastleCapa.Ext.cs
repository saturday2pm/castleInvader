using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace ProtocolCS
{
    using Constants;

    public static class CastleCapaExt
    {
        public static CastleCapa GetInfo(this Castle castle)
        {
            if (castle == null)
                throw new ArgumentNullException("castle is null");

            if (castle.type == CastleType.Normal)
                return CastleCapa.Normal;
            if (castle.type == CastleType.Medium)
                return CastleCapa.Medium;
            if (castle.type == CastleType.Large)
                return CastleCapa.Large;

            throw new InvalidOperationException("unknown type : {castle.type}");
        }
    }
}
