using System;
using System.Collections.Generic;

namespace BaseballStats.Models
{
    public partial class CollegePlaying
    {
        public string PlayerId { get; set; }
        public short YearId { get; set; }
        public string SchoolId { get; set; }

        public virtual People Player { get; set; }
        public virtual Schools School { get; set; }
    }
}
