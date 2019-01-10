using System;
using System.Collections.Generic;

namespace BaseballStats.Models
{
    public partial class TeamsHalf
    {
        public string TeamId { get; set; }
        public string LgId { get; set; }
        public short YearId { get; set; }
        public string Half { get; set; }
        public string DivId { get; set; }
        public string DivWin { get; set; }
        public short? Rank { get; set; }
        public short? G { get; set; }
        public short? W { get; set; }
        public short? L { get; set; }

        public virtual Teams Teams { get; set; }
    }
}
