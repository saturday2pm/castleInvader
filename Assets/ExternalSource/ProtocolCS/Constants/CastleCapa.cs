using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace ProtocolCS.Constants
{
    public class CastleCapa
    {
        public static readonly CastleCapa Normal =
            new CastleCapa() { genRate = 1, maxCapacity = 100 };
        public static readonly CastleCapa Medium =
            new CastleCapa() { genRate = 2, maxCapacity = 200 };
        public static readonly CastleCapa Large =
            new CastleCapa() { genRate = 3, maxCapacity = 300 };

        /// <summary>
        /// 초당 생산량
        /// </summary>
        public double genRate { get; set; }
        /// <summary>
        /// 최대 수용량
        /// </summary>
        public int maxCapacity { get; set; }
    }
}
