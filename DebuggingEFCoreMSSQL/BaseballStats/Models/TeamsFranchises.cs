using System;
using System.Collections.Generic;

namespace BaseballStats.Models
{
    public partial class TeamsFranchises
    {
        public TeamsFranchises()
        {
            Teams = new HashSet<Teams>();
        }

        public string FranchId { get; set; }
        public string FranchName { get; set; }
        public string Active { get; set; }
        public string Naassoc { get; set; }

        public virtual ICollection<Teams> Teams { get; set; }
    }
}
