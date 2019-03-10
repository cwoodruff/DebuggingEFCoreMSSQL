using System;

namespace BaseballStats.Models
{
    public partial class HomeGames
    {
        public string TeamId { get; set; }
        public string LgId { get; set; }
        public short YearId { get; set; }
        public string ParkId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public short? Games { get; set; }
        public short? Openings { get; set; }
        public int? Attendance { get; set; }

        public virtual Parks Park { get; set; }
        public virtual Teams Teams { get; set; }
    }
}
