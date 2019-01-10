using System;
using System.Collections.Generic;

namespace BaseballStats.Models
{
    public partial class Teams
    {
        public Teams()
        {
            AllstarFull = new HashSet<AllstarFull>();
            Appearances = new HashSet<Appearances>();
            Batting = new HashSet<Batting>();
            BattingPost = new HashSet<BattingPost>();
            Fielding = new HashSet<Fielding>();
            FieldingOfsplit = new HashSet<FieldingOfsplit>();
            FieldingPost = new HashSet<FieldingPost>();
            HomeGames = new HashSet<HomeGames>();
            Managers = new HashSet<Managers>();
            ManagersHalf = new HashSet<ManagersHalf>();
            Pitching = new HashSet<Pitching>();
            PitchingPost = new HashSet<PitchingPost>();
            Salaries = new HashSet<Salaries>();
            SeriesPostTeams = new HashSet<SeriesPost>();
            SeriesPostTeamsNavigation = new HashSet<SeriesPost>();
            TeamsHalf = new HashSet<TeamsHalf>();
        }

        public string TeamId { get; set; }
        public string LgId { get; set; }
        public short YearId { get; set; }
        public string FranchId { get; set; }
        public string DivId { get; set; }
        public short? Rank { get; set; }
        public short? G { get; set; }
        public short? Ghome { get; set; }
        public short? W { get; set; }
        public short? L { get; set; }
        public string DivWin { get; set; }
        public string Wcwin { get; set; }
        public string LgWin { get; set; }
        public string Wswin { get; set; }
        public short? R { get; set; }
        public short? Ab { get; set; }
        public short? H { get; set; }
        public short? _2b { get; set; }
        public short? _3b { get; set; }
        public short? Hr { get; set; }
        public short? Bb { get; set; }
        public short? So { get; set; }
        public short? Sb { get; set; }
        public short? Cs { get; set; }
        public short? Hbp { get; set; }
        public short? Sf { get; set; }
        public short? Ra { get; set; }
        public short? Er { get; set; }
        public double? Era { get; set; }
        public short? Cg { get; set; }
        public short? Sho { get; set; }
        public short? Sv { get; set; }
        public int? Ipouts { get; set; }
        public short? Ha { get; set; }
        public short? Hra { get; set; }
        public short? Bba { get; set; }
        public short? Soa { get; set; }
        public int? E { get; set; }
        public int? Dp { get; set; }
        public short? Fp { get; set; }
        public string Name { get; set; }
        public string Park { get; set; }
        public int? Attendance { get; set; }
        public int? Bpf { get; set; }
        public int? Ppf { get; set; }
        public string TeamIdbr { get; set; }
        public string TeamIdlahman45 { get; set; }
        public string TeamIdretro { get; set; }

        public virtual TeamsFranchises Franch { get; set; }
        public virtual ICollection<AllstarFull> AllstarFull { get; set; }
        public virtual ICollection<Appearances> Appearances { get; set; }
        public virtual ICollection<Batting> Batting { get; set; }
        public virtual ICollection<BattingPost> BattingPost { get; set; }
        public virtual ICollection<Fielding> Fielding { get; set; }
        public virtual ICollection<FieldingOfsplit> FieldingOfsplit { get; set; }
        public virtual ICollection<FieldingPost> FieldingPost { get; set; }
        public virtual ICollection<HomeGames> HomeGames { get; set; }
        public virtual ICollection<Managers> Managers { get; set; }
        public virtual ICollection<ManagersHalf> ManagersHalf { get; set; }
        public virtual ICollection<Pitching> Pitching { get; set; }
        public virtual ICollection<PitchingPost> PitchingPost { get; set; }
        public virtual ICollection<Salaries> Salaries { get; set; }
        public virtual ICollection<SeriesPost> SeriesPostTeams { get; set; }
        public virtual ICollection<SeriesPost> SeriesPostTeamsNavigation { get; set; }
        public virtual ICollection<TeamsHalf> TeamsHalf { get; set; }
    }
}
