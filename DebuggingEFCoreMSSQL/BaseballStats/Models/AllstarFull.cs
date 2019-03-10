namespace BaseballStats.Models
{
    public partial class AllstarFull
    {
        public string PlayerId { get; set; }
        public string TeamId { get; set; }
        public string LgId { get; set; }
        public short YearId { get; set; }
        public string GameId { get; set; }
        public short? GameNum { get; set; }
        public short? Gp { get; set; }
        public short? StartingPos { get; set; }

        public virtual People Player { get; set; }
        public virtual Teams Teams { get; set; }
    }
}
