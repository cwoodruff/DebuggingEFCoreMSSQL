using System.Collections.Generic;

namespace BaseballStats.Models
{
    public partial class Parks
    {
        public Parks()
        {
            HomeGames = new HashSet<HomeGames>();
        }

        public string ParkId { get; set; }
        public string Name { get; set; }
        public string Alias { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }

        public virtual ICollection<HomeGames> HomeGames { get; set; }
    }
}
