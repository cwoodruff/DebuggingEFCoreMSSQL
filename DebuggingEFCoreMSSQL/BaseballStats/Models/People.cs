using System.Collections.Generic;

namespace BaseballStats.Models
{
    public partial class People
    {
        public People()
        {
            AllstarFull = new HashSet<AllstarFull>();
            Appearances = new HashSet<Appearances>();
            AwardsManagers = new HashSet<AwardsManagers>();
            AwardsPlayers = new HashSet<AwardsPlayers>();
            AwardsShareManagers = new HashSet<AwardsShareManagers>();
            AwardsSharePlayers = new HashSet<AwardsSharePlayers>();
            Batting = new HashSet<Batting>();
            BattingPost = new HashSet<BattingPost>();
            CollegePlaying = new HashSet<CollegePlaying>();
            Fielding = new HashSet<Fielding>();
            FieldingOf = new HashSet<FieldingOf>();
            FieldingOfsplit = new HashSet<FieldingOfsplit>();
            FieldingPost = new HashSet<FieldingPost>();
            HallOfFame = new HashSet<HallOfFame>();
            Managers = new HashSet<Managers>();
            ManagersHalf = new HashSet<ManagersHalf>();
            Pitching = new HashSet<Pitching>();
            PitchingPost = new HashSet<PitchingPost>();
            Salaries = new HashSet<Salaries>();
        }

        public string PlayerId { get; set; }
        public short? BirthYear { get; set; }
        public short? BirthMonth { get; set; }
        public short? BirthDay { get; set; }
        public string BirthCountry { get; set; }
        public string BirthState { get; set; }
        public string BirthCity { get; set; }
        public short? DeathYear { get; set; }
        public short? DeathMonth { get; set; }
        public short? DeathDay { get; set; }
        public string DeathCountry { get; set; }
        public string DeathState { get; set; }
        public string DeathCity { get; set; }
        public string NameFirst { get; set; }
        public string NameLast { get; set; }
        public string NameGiven { get; set; }
        public short? Weight { get; set; }
        public short? Height { get; set; }
        public string Bats { get; set; }
        public string Throws { get; set; }
        public string Debut { get; set; }
        public string FinalGame { get; set; }
        public string RetroId { get; set; }
        public string BbrefId { get; set; }

        public virtual ICollection<AllstarFull> AllstarFull { get; set; }
        public virtual ICollection<Appearances> Appearances { get; set; }
        public virtual ICollection<AwardsManagers> AwardsManagers { get; set; }
        public virtual ICollection<AwardsPlayers> AwardsPlayers { get; set; }
        public virtual ICollection<AwardsShareManagers> AwardsShareManagers { get; set; }
        public virtual ICollection<AwardsSharePlayers> AwardsSharePlayers { get; set; }
        public virtual ICollection<Batting> Batting { get; set; }
        public virtual ICollection<BattingPost> BattingPost { get; set; }
        public virtual ICollection<CollegePlaying> CollegePlaying { get; set; }
        public virtual ICollection<Fielding> Fielding { get; set; }
        public virtual ICollection<FieldingOf> FieldingOf { get; set; }
        public virtual ICollection<FieldingOfsplit> FieldingOfsplit { get; set; }
        public virtual ICollection<FieldingPost> FieldingPost { get; set; }
        public virtual ICollection<HallOfFame> HallOfFame { get; set; }
        public virtual ICollection<Managers> Managers { get; set; }
        public virtual ICollection<ManagersHalf> ManagersHalf { get; set; }
        public virtual ICollection<Pitching> Pitching { get; set; }
        public virtual ICollection<PitchingPost> PitchingPost { get; set; }
        public virtual ICollection<Salaries> Salaries { get; set; }
    }
}
