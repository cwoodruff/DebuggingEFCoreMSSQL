using System;
using System.Collections.Generic;

namespace BaseballStats.Models
{
    public partial class Schools
    {
        public Schools()
        {
            CollegePlaying = new HashSet<CollegePlaying>();
        }

        public string SchoolId { get; set; }
        public string NameFull { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }

        public virtual ICollection<CollegePlaying> CollegePlaying { get; set; }
    }
}
