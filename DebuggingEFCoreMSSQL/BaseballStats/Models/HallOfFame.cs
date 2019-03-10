namespace BaseballStats.Models
{
    public partial class HallOfFame
    {
        public string PlayerId { get; set; }
        public short YearId { get; set; }
        public string VotedBy { get; set; }
        public short? Ballots { get; set; }
        public short? Needed { get; set; }
        public short? Votes { get; set; }
        public string Inducted { get; set; }
        public string Category { get; set; }
        public string NeededNote { get; set; }

        public virtual People Player { get; set; }
    }
}
