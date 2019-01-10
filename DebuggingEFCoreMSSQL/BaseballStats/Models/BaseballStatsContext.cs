using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace BaseballStats.Models
{
    public partial class BaseballStatsContext : DbContext
    {
        public BaseballStatsContext(DbContextOptions<BaseballStatsContext> options)
            : base(options)
        {
        }
        
        public DbQuery<PlayerBattingTotals> PlayerBattingTotals { get; set; }

        public virtual DbSet<AllstarFull> AllstarFull { get; set; }
        public virtual DbSet<Appearances> Appearances { get; set; }
        public virtual DbSet<AwardsManagers> AwardsManagers { get; set; }
        public virtual DbSet<AwardsPlayers> AwardsPlayers { get; set; }
        public virtual DbSet<AwardsShareManagers> AwardsShareManagers { get; set; }
        public virtual DbSet<AwardsSharePlayers> AwardsSharePlayers { get; set; }
        public virtual DbSet<Batting> Batting { get; set; }
        public virtual DbSet<BattingPost> BattingPost { get; set; }
        public virtual DbSet<CollegePlaying> CollegePlaying { get; set; }
        public virtual DbSet<Fielding> Fielding { get; set; }
        public virtual DbSet<FieldingOf> FieldingOf { get; set; }
        public virtual DbSet<FieldingOfsplit> FieldingOfsplit { get; set; }
        public virtual DbSet<FieldingPost> FieldingPost { get; set; }
        public virtual DbSet<HallOfFame> HallOfFame { get; set; }
        public virtual DbSet<HomeGames> HomeGames { get; set; }
        public virtual DbSet<Managers> Managers { get; set; }
        public virtual DbSet<ManagersHalf> ManagersHalf { get; set; }
        public virtual DbSet<Parks> Parks { get; set; }
        public virtual DbSet<People> People { get; set; }
        public virtual DbSet<Pitching> Pitching { get; set; }
        public virtual DbSet<PitchingPost> PitchingPost { get; set; }
        public virtual DbSet<Salaries> Salaries { get; set; }
        public virtual DbSet<Schools> Schools { get; set; }
        public virtual DbSet<SeriesPost> SeriesPost { get; set; }
        public virtual DbSet<Teams> Teams { get; set; }
        public virtual DbSet<TeamsFranchises> TeamsFranchises { get; set; }
        public virtual DbSet<TeamsHalf> TeamsHalf { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.1-servicing-10028");

            modelBuilder.Entity<AllstarFull>(entity =>
            {
                entity.HasKey(e => new { e.PlayerId, e.TeamId, e.LgId, e.YearId, e.GameId });

                entity.Property(e => e.PlayerId)
                    .HasColumnName("playerID")
                    .HasMaxLength(9);

                entity.Property(e => e.TeamId)
                    .HasColumnName("teamID")
                    .HasMaxLength(3);

                entity.Property(e => e.LgId)
                    .HasColumnName("lgID")
                    .HasMaxLength(2);

                entity.Property(e => e.YearId).HasColumnName("yearID");

                entity.Property(e => e.GameId)
                    .HasColumnName("gameID")
                    .HasMaxLength(12);

                entity.Property(e => e.GameNum).HasColumnName("gameNum");

                entity.Property(e => e.Gp).HasColumnName("GP");

                entity.Property(e => e.StartingPos).HasColumnName("startingPos");

                entity.HasOne(d => d.Player)
                    .WithMany(p => p.AllstarFull)
                    .HasForeignKey(d => d.PlayerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AllstarFull_People");

                entity.HasOne(d => d.Teams)
                    .WithMany(p => p.AllstarFull)
                    .HasForeignKey(d => new { d.TeamId, d.LgId, d.YearId })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AllstarFull_Teams");
            });

            modelBuilder.Query<PlayerBattingTotals>().ToView("PlayerBattingTotals");

            modelBuilder.Entity<Appearances>(entity =>
            {
                entity.HasKey(e => new { e.PlayerId, e.YearId, e.LgId, e.TeamId });

                entity.Property(e => e.PlayerId)
                    .HasColumnName("playerID")
                    .HasMaxLength(9);

                entity.Property(e => e.YearId).HasColumnName("yearID");

                entity.Property(e => e.LgId)
                    .HasColumnName("lgID")
                    .HasMaxLength(2);

                entity.Property(e => e.TeamId)
                    .HasColumnName("teamID")
                    .HasMaxLength(3);

                entity.Property(e => e.G1b).HasColumnName("G_1b");

                entity.Property(e => e.G2b).HasColumnName("G_2b");

                entity.Property(e => e.G3b).HasColumnName("G_3b");

                entity.Property(e => e.GAll).HasColumnName("G_all");

                entity.Property(e => e.GBatting).HasColumnName("G_batting");

                entity.Property(e => e.GC).HasColumnName("G_c");

                entity.Property(e => e.GCf).HasColumnName("G_cf");

                entity.Property(e => e.GDefense).HasColumnName("G_defense");

                entity.Property(e => e.GDh).HasColumnName("G_dh");

                entity.Property(e => e.GLf).HasColumnName("G_lf");

                entity.Property(e => e.GOf).HasColumnName("G_of");

                entity.Property(e => e.GP).HasColumnName("G_p");

                entity.Property(e => e.GPh).HasColumnName("G_ph");

                entity.Property(e => e.GPr).HasColumnName("G_pr");

                entity.Property(e => e.GRf).HasColumnName("G_rf");

                entity.Property(e => e.GSs).HasColumnName("G_ss");

                entity.Property(e => e.Gs).HasColumnName("GS");

                entity.HasOne(d => d.Player)
                    .WithMany(p => p.Appearances)
                    .HasForeignKey(d => d.PlayerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Appearances_People");

                entity.HasOne(d => d.Teams)
                    .WithMany(p => p.Appearances)
                    .HasForeignKey(d => new { d.TeamId, d.LgId, d.YearId })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Appearances_Teams");
            });

            modelBuilder.Entity<AwardsManagers>(entity =>
            {
                entity.HasKey(e => new { e.PlayerId, e.YearId, e.LgId, e.AwardId });

                entity.Property(e => e.PlayerId)
                    .HasColumnName("playerID")
                    .HasMaxLength(9);

                entity.Property(e => e.YearId).HasColumnName("yearID");

                entity.Property(e => e.LgId)
                    .HasColumnName("lgID")
                    .HasMaxLength(2);

                entity.Property(e => e.AwardId)
                    .HasColumnName("awardID")
                    .HasMaxLength(75);

                entity.Property(e => e.Notes)
                    .HasColumnName("notes")
                    .HasMaxLength(100);

                entity.Property(e => e.Tie)
                    .HasColumnName("tie")
                    .HasMaxLength(1);

                entity.HasOne(d => d.Player)
                    .WithMany(p => p.AwardsManagers)
                    .HasForeignKey(d => d.PlayerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AwardsManagers_People");
            });

            modelBuilder.Entity<AwardsPlayers>(entity =>
            {
                entity.HasKey(e => new { e.PlayerId, e.YearId, e.LgId, e.AwardId });

                entity.Property(e => e.PlayerId)
                    .HasColumnName("playerID")
                    .HasMaxLength(9);

                entity.Property(e => e.YearId).HasColumnName("yearID");

                entity.Property(e => e.LgId)
                    .HasColumnName("lgID")
                    .HasMaxLength(2);

                entity.Property(e => e.AwardId)
                    .HasColumnName("awardID")
                    .HasMaxLength(25);

                entity.Property(e => e.Notes)
                    .HasColumnName("notes")
                    .HasMaxLength(100);

                entity.Property(e => e.Tie)
                    .HasColumnName("tie")
                    .HasMaxLength(50);

                entity.HasOne(d => d.Player)
                    .WithMany(p => p.AwardsPlayers)
                    .HasForeignKey(d => d.PlayerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AwardsPlayers_People");
            });

            modelBuilder.Entity<AwardsShareManagers>(entity =>
            {
                entity.HasKey(e => new { e.PlayerId, e.YearId, e.LgId, e.AwardId });

                entity.Property(e => e.PlayerId)
                    .HasColumnName("playerID")
                    .HasMaxLength(9);

                entity.Property(e => e.YearId).HasColumnName("yearID");

                entity.Property(e => e.LgId)
                    .HasColumnName("lgID")
                    .HasMaxLength(2);

                entity.Property(e => e.AwardId)
                    .HasColumnName("awardID")
                    .HasMaxLength(25);

                entity.Property(e => e.PointsMax).HasColumnName("pointsMax");

                entity.Property(e => e.PointsWon).HasColumnName("pointsWon");

                entity.Property(e => e.VotesFirst).HasColumnName("votesFirst");

                entity.HasOne(d => d.Player)
                    .WithMany(p => p.AwardsShareManagers)
                    .HasForeignKey(d => d.PlayerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AwardsShareManagers_People");
            });

            modelBuilder.Entity<AwardsSharePlayers>(entity =>
            {
                entity.HasKey(e => new { e.PlayerId, e.YearId, e.LgId, e.AwardId });

                entity.Property(e => e.PlayerId)
                    .HasColumnName("playerID")
                    .HasMaxLength(9);

                entity.Property(e => e.YearId).HasColumnName("yearID");

                entity.Property(e => e.LgId)
                    .HasColumnName("lgID")
                    .HasMaxLength(2);

                entity.Property(e => e.AwardId)
                    .HasColumnName("awardID")
                    .HasMaxLength(25);

                entity.Property(e => e.PointsMax).HasColumnName("pointsMax");

                entity.Property(e => e.PointsWon).HasColumnName("pointsWon");

                entity.Property(e => e.VotesFirst).HasColumnName("votesFirst");

                entity.HasOne(d => d.Player)
                    .WithMany(p => p.AwardsSharePlayers)
                    .HasForeignKey(d => d.PlayerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AwardsSharePlayers_People");
            });

            modelBuilder.Entity<Batting>(entity =>
            {
                entity.HasKey(e => new { e.PlayerId, e.YearId, e.LgId, e.TeamId, e.Stint });

                entity.Property(e => e.PlayerId)
                    .HasColumnName("playerID")
                    .HasMaxLength(9);

                entity.Property(e => e.YearId).HasColumnName("yearID");

                entity.Property(e => e.LgId)
                    .HasColumnName("lgID")
                    .HasMaxLength(2);

                entity.Property(e => e.TeamId)
                    .HasColumnName("teamID")
                    .HasMaxLength(3);

                entity.Property(e => e.Stint).HasColumnName("stint");

                entity.Property(e => e.Ab).HasColumnName("AB");

                entity.Property(e => e.Bb).HasColumnName("BB");

                entity.Property(e => e.Cs).HasColumnName("CS");

                entity.Property(e => e.Gidp).HasColumnName("GIDP");

                entity.Property(e => e.Hbp).HasColumnName("HBP");

                entity.Property(e => e.Hr).HasColumnName("HR");

                entity.Property(e => e.Ibb).HasColumnName("IBB");

                entity.Property(e => e.Rbi).HasColumnName("RBI");

                entity.Property(e => e.Sb).HasColumnName("SB");

                entity.Property(e => e.Sf).HasColumnName("SF");

                entity.Property(e => e.Sh).HasColumnName("SH");

                entity.Property(e => e.So).HasColumnName("SO");

                entity.Property(e => e._2b).HasColumnName("2B");

                entity.Property(e => e._3b).HasColumnName("3B");

                entity.HasOne(d => d.Player)
                    .WithMany(p => p.Batting)
                    .HasForeignKey(d => d.PlayerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Batting_People");

                entity.HasOne(d => d.Teams)
                    .WithMany(p => p.Batting)
                    .HasForeignKey(d => new { d.TeamId, d.LgId, d.YearId })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Batting_Teams");
            });

            modelBuilder.Entity<BattingPost>(entity =>
            {
                entity.HasKey(e => new { e.PlayerId, e.YearId, e.LgId, e.TeamId, e.Round });

                entity.Property(e => e.PlayerId)
                    .HasColumnName("playerID")
                    .HasMaxLength(9);

                entity.Property(e => e.YearId).HasColumnName("yearID");

                entity.Property(e => e.LgId)
                    .HasColumnName("lgID")
                    .HasMaxLength(2);

                entity.Property(e => e.TeamId)
                    .HasColumnName("teamID")
                    .HasMaxLength(3);

                entity.Property(e => e.Round)
                    .HasColumnName("round")
                    .HasMaxLength(10);

                entity.Property(e => e.Ab).HasColumnName("AB");

                entity.Property(e => e.Bb).HasColumnName("BB");

                entity.Property(e => e.Cs).HasColumnName("CS");

                entity.Property(e => e.Gidp).HasColumnName("GIDP");

                entity.Property(e => e.Hbp).HasColumnName("HBP");

                entity.Property(e => e.Hr).HasColumnName("HR");

                entity.Property(e => e.Ibb).HasColumnName("IBB");

                entity.Property(e => e.Rbi).HasColumnName("RBI");

                entity.Property(e => e.Sb).HasColumnName("SB");

                entity.Property(e => e.Sf).HasColumnName("SF");

                entity.Property(e => e.Sh).HasColumnName("SH");

                entity.Property(e => e.So).HasColumnName("SO");

                entity.Property(e => e._2b).HasColumnName("2B");

                entity.Property(e => e._3b).HasColumnName("3B");

                entity.HasOne(d => d.Player)
                    .WithMany(p => p.BattingPost)
                    .HasForeignKey(d => d.PlayerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BattingPost_People");

                entity.HasOne(d => d.Teams)
                    .WithMany(p => p.BattingPost)
                    .HasForeignKey(d => new { d.TeamId, d.LgId, d.YearId })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BattingPost_Teams");
            });

            modelBuilder.Entity<CollegePlaying>(entity =>
            {
                entity.HasKey(e => new { e.PlayerId, e.YearId, e.SchoolId });

                entity.Property(e => e.PlayerId)
                    .HasColumnName("playerID")
                    .HasMaxLength(9);

                entity.Property(e => e.YearId).HasColumnName("yearID");

                entity.Property(e => e.SchoolId)
                    .HasColumnName("schoolID")
                    .HasMaxLength(15);

                entity.HasOne(d => d.Player)
                    .WithMany(p => p.CollegePlaying)
                    .HasForeignKey(d => d.PlayerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CollegePlaying_People");

                entity.HasOne(d => d.School)
                    .WithMany(p => p.CollegePlaying)
                    .HasForeignKey(d => d.SchoolId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CollegePlaying_Schools");
            });

            modelBuilder.Entity<Fielding>(entity =>
            {
                entity.HasKey(e => new { e.PlayerId, e.YearId, e.LgId, e.TeamId, e.Stint, e.Pos });

                entity.Property(e => e.PlayerId)
                    .HasColumnName("playerID")
                    .HasMaxLength(9);

                entity.Property(e => e.YearId).HasColumnName("yearID");

                entity.Property(e => e.LgId)
                    .HasColumnName("lgID")
                    .HasMaxLength(2);

                entity.Property(e => e.TeamId)
                    .HasColumnName("teamID")
                    .HasMaxLength(3);

                entity.Property(e => e.Stint).HasColumnName("stint");

                entity.Property(e => e.Pos)
                    .HasColumnName("POS")
                    .HasMaxLength(2);

                entity.Property(e => e.Cs).HasColumnName("CS");

                entity.Property(e => e.Dp).HasColumnName("DP");

                entity.Property(e => e.Gs).HasColumnName("GS");

                entity.Property(e => e.Pb).HasColumnName("PB");

                entity.Property(e => e.Po).HasColumnName("PO");

                entity.Property(e => e.Sb).HasColumnName("SB");

                entity.Property(e => e.Wp).HasColumnName("WP");

                entity.Property(e => e.Zr).HasColumnName("ZR");

                entity.HasOne(d => d.Player)
                    .WithMany(p => p.Fielding)
                    .HasForeignKey(d => d.PlayerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Fielding_People");

                entity.HasOne(d => d.Teams)
                    .WithMany(p => p.Fielding)
                    .HasForeignKey(d => new { d.TeamId, d.LgId, d.YearId })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Fielding_Teams");
            });

            modelBuilder.Entity<FieldingOf>(entity =>
            {
                entity.HasKey(e => new { e.PlayerId, e.YearId, e.Stint });

                entity.ToTable("FieldingOF");

                entity.Property(e => e.PlayerId)
                    .HasColumnName("playerID")
                    .HasMaxLength(9);

                entity.Property(e => e.YearId).HasColumnName("yearID");

                entity.Property(e => e.Stint).HasColumnName("stint");

                entity.HasOne(d => d.Player)
                    .WithMany(p => p.FieldingOf)
                    .HasForeignKey(d => d.PlayerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FieldingOF_People");
            });

            modelBuilder.Entity<FieldingOfsplit>(entity =>
            {
                entity.HasKey(e => new { e.PlayerId, e.YearId, e.LgId, e.TeamId, e.Stint, e.Pos });

                entity.ToTable("FieldingOFsplit");

                entity.Property(e => e.PlayerId)
                    .HasColumnName("playerID")
                    .HasMaxLength(9);

                entity.Property(e => e.YearId).HasColumnName("yearID");

                entity.Property(e => e.LgId)
                    .HasColumnName("lgID")
                    .HasMaxLength(2);

                entity.Property(e => e.TeamId)
                    .HasColumnName("teamID")
                    .HasMaxLength(3);

                entity.Property(e => e.Stint).HasColumnName("stint");

                entity.Property(e => e.Pos)
                    .HasColumnName("POS")
                    .HasMaxLength(2);

                entity.Property(e => e.Cs).HasColumnName("CS");

                entity.Property(e => e.Dp).HasColumnName("DP");

                entity.Property(e => e.Gs).HasColumnName("GS");

                entity.Property(e => e.Pb).HasColumnName("PB");

                entity.Property(e => e.Po).HasColumnName("PO");

                entity.Property(e => e.Sb).HasColumnName("SB");

                entity.Property(e => e.Wp).HasColumnName("WP");

                entity.Property(e => e.Zr).HasColumnName("ZR");

                entity.HasOne(d => d.Player)
                    .WithMany(p => p.FieldingOfsplit)
                    .HasForeignKey(d => d.PlayerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FieldingOFsplit_People");

                entity.HasOne(d => d.Teams)
                    .WithMany(p => p.FieldingOfsplit)
                    .HasForeignKey(d => new { d.TeamId, d.LgId, d.YearId })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FieldingOFsplit_Teams");
            });

            modelBuilder.Entity<FieldingPost>(entity =>
            {
                entity.HasKey(e => new { e.PlayerId, e.YearId, e.LgId, e.TeamId, e.Round, e.Pos });

                entity.Property(e => e.PlayerId)
                    .HasColumnName("playerID")
                    .HasMaxLength(9);

                entity.Property(e => e.YearId).HasColumnName("yearID");

                entity.Property(e => e.LgId)
                    .HasColumnName("lgID")
                    .HasMaxLength(2);

                entity.Property(e => e.TeamId)
                    .HasColumnName("teamID")
                    .HasMaxLength(3);

                entity.Property(e => e.Round)
                    .HasColumnName("round")
                    .HasMaxLength(10);

                entity.Property(e => e.Pos)
                    .HasColumnName("POS")
                    .HasMaxLength(2);

                entity.Property(e => e.Cs).HasColumnName("CS");

                entity.Property(e => e.Dp).HasColumnName("DP");

                entity.Property(e => e.Gs).HasColumnName("GS");

                entity.Property(e => e.Pb).HasColumnName("PB");

                entity.Property(e => e.Po).HasColumnName("PO");

                entity.Property(e => e.Sb).HasColumnName("SB");

                entity.Property(e => e.Tp).HasColumnName("TP");

                entity.HasOne(d => d.Player)
                    .WithMany(p => p.FieldingPost)
                    .HasForeignKey(d => d.PlayerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FieldingPost_People");

                entity.HasOne(d => d.Teams)
                    .WithMany(p => p.FieldingPost)
                    .HasForeignKey(d => new { d.TeamId, d.LgId, d.YearId })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FieldingPost_Teams");
            });

            modelBuilder.Entity<HallOfFame>(entity =>
            {
                entity.HasKey(e => new { e.PlayerId, e.YearId, e.VotedBy });

                entity.Property(e => e.PlayerId)
                    .HasColumnName("playerID")
                    .HasMaxLength(9);

                entity.Property(e => e.YearId).HasColumnName("yearID");

                entity.Property(e => e.VotedBy)
                    .HasColumnName("votedBy")
                    .HasMaxLength(64);

                entity.Property(e => e.Ballots).HasColumnName("ballots");

                entity.Property(e => e.Category)
                    .HasColumnName("category")
                    .HasMaxLength(20);

                entity.Property(e => e.Inducted)
                    .HasColumnName("inducted")
                    .HasMaxLength(1);

                entity.Property(e => e.Needed).HasColumnName("needed");

                entity.Property(e => e.NeededNote)
                    .HasColumnName("needed_note")
                    .HasMaxLength(25);

                entity.Property(e => e.Votes).HasColumnName("votes");

                entity.HasOne(d => d.Player)
                    .WithMany(p => p.HallOfFame)
                    .HasForeignKey(d => d.PlayerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_HallOfFame_People");
            });

            modelBuilder.Entity<HomeGames>(entity =>
            {
                entity.HasKey(e => new { e.YearId, e.LgId, e.TeamId, e.ParkId });

                entity.Property(e => e.YearId).HasColumnName("yearID");

                entity.Property(e => e.LgId)
                    .HasColumnName("lgID")
                    .HasMaxLength(2);

                entity.Property(e => e.TeamId)
                    .HasColumnName("teamID")
                    .HasMaxLength(3);

                entity.Property(e => e.ParkId)
                    .HasColumnName("parkID")
                    .HasMaxLength(6);

                entity.Property(e => e.Attendance).HasColumnName("attendance");

                entity.Property(e => e.EndDate)
                    .HasColumnName("endDate")
                    .HasColumnType("date");

                entity.Property(e => e.Games).HasColumnName("games");

                entity.Property(e => e.Openings).HasColumnName("openings");

                entity.Property(e => e.StartDate)
                    .HasColumnName("startDate")
                    .HasColumnType("date");

                entity.HasOne(d => d.Park)
                    .WithMany(p => p.HomeGames)
                    .HasForeignKey(d => d.ParkId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_HomeGames_Parks");

                entity.HasOne(d => d.Teams)
                    .WithMany(p => p.HomeGames)
                    .HasForeignKey(d => new { d.TeamId, d.LgId, d.YearId })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_HomeGames_Teams");
            });

            modelBuilder.Entity<Managers>(entity =>
            {
                entity.HasKey(e => new { e.PlayerId, e.YearId, e.LgId, e.TeamId, e.Inseason });

                entity.Property(e => e.PlayerId)
                    .HasColumnName("playerID")
                    .HasMaxLength(9);

                entity.Property(e => e.YearId).HasColumnName("yearID");

                entity.Property(e => e.LgId)
                    .HasColumnName("lgID")
                    .HasMaxLength(2);

                entity.Property(e => e.TeamId)
                    .HasColumnName("teamID")
                    .HasMaxLength(3);

                entity.Property(e => e.Inseason).HasColumnName("inseason");

                entity.Property(e => e.PlyrMgr)
                    .HasColumnName("plyrMgr")
                    .HasMaxLength(1);

                entity.Property(e => e.Rank).HasColumnName("rank");

                entity.HasOne(d => d.Player)
                    .WithMany(p => p.Managers)
                    .HasForeignKey(d => d.PlayerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Managers_People");

                entity.HasOne(d => d.Teams)
                    .WithMany(p => p.Managers)
                    .HasForeignKey(d => new { d.TeamId, d.LgId, d.YearId })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Managers_Teams");
            });

            modelBuilder.Entity<ManagersHalf>(entity =>
            {
                entity.HasKey(e => new { e.PlayerId, e.YearId, e.LgId, e.TeamId, e.Inseason, e.Half })
                    .HasName("PK_ManagersHalf_1");

                entity.Property(e => e.PlayerId)
                    .HasColumnName("playerID")
                    .HasMaxLength(9);

                entity.Property(e => e.YearId).HasColumnName("yearID");

                entity.Property(e => e.LgId)
                    .HasColumnName("lgID")
                    .HasMaxLength(2);

                entity.Property(e => e.TeamId)
                    .HasColumnName("teamID")
                    .HasMaxLength(3);

                entity.Property(e => e.Inseason).HasColumnName("inseason");

                entity.Property(e => e.Half).HasColumnName("half");

                entity.Property(e => e.Rank).HasColumnName("rank");

                entity.HasOne(d => d.Player)
                    .WithMany(p => p.ManagersHalf)
                    .HasForeignKey(d => d.PlayerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ManagersHalf_People");

                entity.HasOne(d => d.Teams)
                    .WithMany(p => p.ManagersHalf)
                    .HasForeignKey(d => new { d.TeamId, d.LgId, d.YearId })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ManagersHalf_Teams");
            });

            modelBuilder.Entity<Parks>(entity =>
            {
                entity.HasKey(e => e.ParkId);

                entity.Property(e => e.ParkId)
                    .HasColumnName("parkID")
                    .HasMaxLength(6)
                    .ValueGeneratedNever();

                entity.Property(e => e.Alias)
                    .HasColumnName("alias")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.City)
                    .HasColumnName("city")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Country)
                    .HasColumnName("country")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.State)
                    .HasColumnName("state")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<People>(entity =>
            {
                entity.HasKey(e => e.PlayerId);

                entity.Property(e => e.PlayerId)
                    .HasColumnName("playerID")
                    .HasMaxLength(9)
                    .ValueGeneratedNever();

                entity.Property(e => e.Bats)
                    .HasColumnName("bats")
                    .HasMaxLength(255);

                entity.Property(e => e.BbrefId)
                    .HasColumnName("bbrefID")
                    .HasMaxLength(255);

                entity.Property(e => e.BirthCity)
                    .HasColumnName("birthCity")
                    .HasMaxLength(255);

                entity.Property(e => e.BirthCountry)
                    .HasColumnName("birthCountry")
                    .HasMaxLength(255);

                entity.Property(e => e.BirthDay).HasColumnName("birthDay");

                entity.Property(e => e.BirthMonth).HasColumnName("birthMonth");

                entity.Property(e => e.BirthState)
                    .HasColumnName("birthState")
                    .HasMaxLength(255);

                entity.Property(e => e.BirthYear).HasColumnName("birthYear");

                entity.Property(e => e.DeathCity)
                    .HasColumnName("deathCity")
                    .HasMaxLength(255);

                entity.Property(e => e.DeathCountry)
                    .HasColumnName("deathCountry")
                    .HasMaxLength(255);

                entity.Property(e => e.DeathDay).HasColumnName("deathDay");

                entity.Property(e => e.DeathMonth).HasColumnName("deathMonth");

                entity.Property(e => e.DeathState)
                    .HasColumnName("deathState")
                    .HasMaxLength(255);

                entity.Property(e => e.DeathYear).HasColumnName("deathYear");

                entity.Property(e => e.Debut)
                    .HasColumnName("debut")
                    .HasMaxLength(255);

                entity.Property(e => e.FinalGame)
                    .HasColumnName("finalGame")
                    .HasMaxLength(255);

                entity.Property(e => e.Height).HasColumnName("height");

                entity.Property(e => e.NameFirst)
                    .HasColumnName("nameFirst")
                    .HasMaxLength(255);

                entity.Property(e => e.NameGiven)
                    .HasColumnName("nameGiven")
                    .HasMaxLength(255);

                entity.Property(e => e.NameLast)
                    .HasColumnName("nameLast")
                    .HasMaxLength(255);

                entity.Property(e => e.RetroId)
                    .HasColumnName("retroID")
                    .HasMaxLength(255);

                entity.Property(e => e.Throws)
                    .HasColumnName("throws")
                    .HasMaxLength(255);

                entity.Property(e => e.Weight).HasColumnName("weight");
            });

            modelBuilder.Entity<Pitching>(entity =>
            {
                entity.HasKey(e => new { e.PlayerId, e.YearId, e.LgId, e.TeamId, e.Stint });

                entity.Property(e => e.PlayerId)
                    .HasColumnName("playerID")
                    .HasMaxLength(9);

                entity.Property(e => e.YearId).HasColumnName("yearID");

                entity.Property(e => e.LgId)
                    .HasColumnName("lgID")
                    .HasMaxLength(2);

                entity.Property(e => e.TeamId)
                    .HasColumnName("teamID")
                    .HasMaxLength(3);

                entity.Property(e => e.Stint).HasColumnName("stint");

                entity.Property(e => e.Baopp).HasColumnName("BAOpp");

                entity.Property(e => e.Bb).HasColumnName("BB");

                entity.Property(e => e.Bfp).HasColumnName("BFP");

                entity.Property(e => e.Bk).HasColumnName("BK");

                entity.Property(e => e.Cg).HasColumnName("CG");

                entity.Property(e => e.Er).HasColumnName("ER");

                entity.Property(e => e.Era).HasColumnName("ERA");

                entity.Property(e => e.Gf).HasColumnName("GF");

                entity.Property(e => e.Gidp).HasColumnName("GIDP");

                entity.Property(e => e.Gs).HasColumnName("GS");

                entity.Property(e => e.Hbp).HasColumnName("HBP");

                entity.Property(e => e.Hr).HasColumnName("HR");

                entity.Property(e => e.Ibb).HasColumnName("IBB");

                entity.Property(e => e.Ipouts).HasColumnName("IPouts");

                entity.Property(e => e.Sf).HasColumnName("SF");

                entity.Property(e => e.Sh).HasColumnName("SH");

                entity.Property(e => e.Sho).HasColumnName("SHO");

                entity.Property(e => e.So).HasColumnName("SO");

                entity.Property(e => e.Sv).HasColumnName("SV");

                entity.Property(e => e.Wp).HasColumnName("WP");

                entity.HasOne(d => d.Player)
                    .WithMany(p => p.Pitching)
                    .HasForeignKey(d => d.PlayerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Pitching_People");

                entity.HasOne(d => d.Teams)
                    .WithMany(p => p.Pitching)
                    .HasForeignKey(d => new { d.TeamId, d.LgId, d.YearId })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Pitching_Teams");
            });

            modelBuilder.Entity<PitchingPost>(entity =>
            {
                entity.HasKey(e => new { e.PlayerId, e.YearId, e.LgId, e.TeamId, e.Round });

                entity.Property(e => e.PlayerId)
                    .HasColumnName("playerID")
                    .HasMaxLength(9);

                entity.Property(e => e.YearId).HasColumnName("yearID");

                entity.Property(e => e.LgId)
                    .HasColumnName("lgID")
                    .HasMaxLength(2);

                entity.Property(e => e.TeamId)
                    .HasColumnName("teamID")
                    .HasMaxLength(3);

                entity.Property(e => e.Round)
                    .HasColumnName("round")
                    .HasMaxLength(10);

                entity.Property(e => e.Baopp).HasColumnName("BAOpp");

                entity.Property(e => e.Bb).HasColumnName("BB");

                entity.Property(e => e.Bfp).HasColumnName("BFP");

                entity.Property(e => e.Bk).HasColumnName("BK");

                entity.Property(e => e.Cg).HasColumnName("CG");

                entity.Property(e => e.Er).HasColumnName("ER");

                entity.Property(e => e.Era).HasColumnName("ERA");

                entity.Property(e => e.Gf).HasColumnName("GF");

                entity.Property(e => e.Gidp).HasColumnName("GIDP");

                entity.Property(e => e.Gs).HasColumnName("GS");

                entity.Property(e => e.Hbp).HasColumnName("HBP");

                entity.Property(e => e.Hr).HasColumnName("HR");

                entity.Property(e => e.Ibb).HasColumnName("IBB");

                entity.Property(e => e.Ipouts).HasColumnName("IPouts");

                entity.Property(e => e.Sf).HasColumnName("SF");

                entity.Property(e => e.Sh).HasColumnName("SH");

                entity.Property(e => e.Sho).HasColumnName("SHO");

                entity.Property(e => e.So).HasColumnName("SO");

                entity.Property(e => e.Sv).HasColumnName("SV");

                entity.Property(e => e.Wp).HasColumnName("WP");

                entity.HasOne(d => d.Player)
                    .WithMany(p => p.PitchingPost)
                    .HasForeignKey(d => d.PlayerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PitchingPost_People");

                entity.HasOne(d => d.Teams)
                    .WithMany(p => p.PitchingPost)
                    .HasForeignKey(d => new { d.TeamId, d.LgId, d.YearId })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PitchingPost_Teams");
            });

            modelBuilder.Entity<Salaries>(entity =>
            {
                entity.HasKey(e => new { e.PlayerId, e.YearId, e.LgId, e.TeamId });

                entity.Property(e => e.PlayerId)
                    .HasColumnName("playerID")
                    .HasMaxLength(9);

                entity.Property(e => e.YearId).HasColumnName("yearID");

                entity.Property(e => e.LgId)
                    .HasColumnName("lgID")
                    .HasMaxLength(2);

                entity.Property(e => e.TeamId)
                    .HasColumnName("teamID")
                    .HasMaxLength(3);

                entity.Property(e => e.Salary).HasColumnName("salary");

                entity.HasOne(d => d.Player)
                    .WithMany(p => p.Salaries)
                    .HasForeignKey(d => d.PlayerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Salaries_People");

                entity.HasOne(d => d.Teams)
                    .WithMany(p => p.Salaries)
                    .HasForeignKey(d => new { d.TeamId, d.LgId, d.YearId })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Salaries_Teams");
            });

            modelBuilder.Entity<Schools>(entity =>
            {
                entity.HasKey(e => e.SchoolId);

                entity.Property(e => e.SchoolId)
                    .HasColumnName("schoolID")
                    .HasMaxLength(15)
                    .ValueGeneratedNever();

                entity.Property(e => e.City)
                    .HasColumnName("city")
                    .HasMaxLength(55);

                entity.Property(e => e.Country)
                    .HasColumnName("country")
                    .HasMaxLength(55);

                entity.Property(e => e.NameFull)
                    .HasColumnName("name_full")
                    .HasMaxLength(255);

                entity.Property(e => e.State)
                    .HasColumnName("state")
                    .HasMaxLength(55);
            });

            modelBuilder.Entity<SeriesPost>(entity =>
            {
                entity.HasKey(e => new { e.TeamIdwinner, e.YearId, e.LgIdwinner, e.Round });

                entity.HasIndex(e => new { e.TeamIdloser, e.YearId, e.LgIdloser, e.Round })
                    .HasName("IX_SeriesPost");

                entity.Property(e => e.TeamIdwinner)
                    .HasColumnName("teamIDwinner")
                    .HasMaxLength(3);

                entity.Property(e => e.YearId).HasColumnName("yearID");

                entity.Property(e => e.LgIdwinner)
                    .HasColumnName("lgIDwinner")
                    .HasMaxLength(2);

                entity.Property(e => e.Round)
                    .HasColumnName("round")
                    .HasMaxLength(5);

                entity.Property(e => e.LgIdloser)
                    .HasColumnName("lgIDloser")
                    .HasMaxLength(2);

                entity.Property(e => e.Losses).HasColumnName("losses");

                entity.Property(e => e.TeamIdloser)
                    .HasColumnName("teamIDloser")
                    .HasMaxLength(3);

                entity.Property(e => e.Ties).HasColumnName("ties");

                entity.Property(e => e.Wins).HasColumnName("wins");

                entity.HasOne(d => d.Teams)
                    .WithMany(p => p.SeriesPostTeams)
                    .HasForeignKey(d => new { d.TeamIdloser, d.LgIdloser, d.YearId })
                    .HasConstraintName("FK_SeriesPost_Teams1");

                entity.HasOne(d => d.TeamsNavigation)
                    .WithMany(p => p.SeriesPostTeamsNavigation)
                    .HasForeignKey(d => new { d.TeamIdwinner, d.LgIdwinner, d.YearId })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SeriesPost_Teams");
            });

            modelBuilder.Entity<Teams>(entity =>
            {
                entity.HasKey(e => new { e.TeamId, e.LgId, e.YearId });

                entity.Property(e => e.TeamId)
                    .HasColumnName("teamID")
                    .HasMaxLength(3);

                entity.Property(e => e.LgId)
                    .HasColumnName("lgID")
                    .HasMaxLength(2);

                entity.Property(e => e.YearId).HasColumnName("yearID");

                entity.Property(e => e.Ab).HasColumnName("AB");

                entity.Property(e => e.Attendance).HasColumnName("attendance");

                entity.Property(e => e.Bb).HasColumnName("BB");

                entity.Property(e => e.Bba).HasColumnName("BBA");

                entity.Property(e => e.Bpf).HasColumnName("BPF");

                entity.Property(e => e.Cg).HasColumnName("CG");

                entity.Property(e => e.Cs).HasColumnName("CS");

                entity.Property(e => e.DivId)
                    .HasColumnName("divID")
                    .HasMaxLength(1);

                entity.Property(e => e.DivWin).HasMaxLength(1);

                entity.Property(e => e.Dp).HasColumnName("DP");

                entity.Property(e => e.Er).HasColumnName("ER");

                entity.Property(e => e.Era).HasColumnName("ERA");

                entity.Property(e => e.Fp).HasColumnName("FP");

                entity.Property(e => e.FranchId)
                    .HasColumnName("franchID")
                    .HasMaxLength(3);

                entity.Property(e => e.Ha).HasColumnName("HA");

                entity.Property(e => e.Hbp).HasColumnName("HBP");

                entity.Property(e => e.Hr).HasColumnName("HR");

                entity.Property(e => e.Hra).HasColumnName("HRA");

                entity.Property(e => e.Ipouts).HasColumnName("IPouts");

                entity.Property(e => e.LgWin).HasMaxLength(1);

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(50);

                entity.Property(e => e.Park)
                    .HasColumnName("park")
                    .HasMaxLength(255);

                entity.Property(e => e.Ppf).HasColumnName("PPF");

                entity.Property(e => e.Ra).HasColumnName("RA");

                entity.Property(e => e.Sb).HasColumnName("SB");

                entity.Property(e => e.Sf).HasColumnName("SF");

                entity.Property(e => e.Sho).HasColumnName("SHO");

                entity.Property(e => e.So).HasColumnName("SO");

                entity.Property(e => e.Soa).HasColumnName("SOA");

                entity.Property(e => e.Sv).HasColumnName("SV");

                entity.Property(e => e.TeamIdbr)
                    .HasColumnName("teamIDBR")
                    .HasMaxLength(3);

                entity.Property(e => e.TeamIdlahman45)
                    .HasColumnName("teamIDlahman45")
                    .HasMaxLength(3);

                entity.Property(e => e.TeamIdretro)
                    .HasColumnName("teamIDretro")
                    .HasMaxLength(3);

                entity.Property(e => e.Wcwin)
                    .HasColumnName("WCWin")
                    .HasMaxLength(1);

                entity.Property(e => e.Wswin)
                    .HasColumnName("WSWin")
                    .HasMaxLength(1);

                entity.Property(e => e._2b).HasColumnName("2B");

                entity.Property(e => e._3b).HasColumnName("3B");

                entity.HasOne(d => d.Franch)
                    .WithMany(p => p.Teams)
                    .HasForeignKey(d => d.FranchId)
                    .HasConstraintName("FK_Teams_TeamsFranchises");
            });

            modelBuilder.Entity<TeamsFranchises>(entity =>
            {
                entity.HasKey(e => e.FranchId);

                entity.Property(e => e.FranchId)
                    .HasColumnName("franchID")
                    .HasMaxLength(3)
                    .ValueGeneratedNever();

                entity.Property(e => e.Active)
                    .HasColumnName("active")
                    .HasMaxLength(2);

                entity.Property(e => e.FranchName)
                    .HasColumnName("franchName")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Naassoc)
                    .HasColumnName("NAassoc")
                    .HasMaxLength(3);
            });

            modelBuilder.Entity<TeamsHalf>(entity =>
            {
                entity.HasKey(e => new { e.YearId, e.LgId, e.TeamId, e.Half });

                entity.Property(e => e.YearId).HasColumnName("yearID");

                entity.Property(e => e.LgId)
                    .HasColumnName("lgID")
                    .HasMaxLength(2);

                entity.Property(e => e.TeamId)
                    .HasColumnName("teamID")
                    .HasMaxLength(3);

                entity.Property(e => e.Half).HasMaxLength(1);

                entity.Property(e => e.DivId)
                    .HasColumnName("divID")
                    .HasMaxLength(1);

                entity.Property(e => e.DivWin).HasMaxLength(1);

                entity.HasOne(d => d.Teams)
                    .WithMany(p => p.TeamsHalf)
                    .HasForeignKey(d => new { d.TeamId, d.LgId, d.YearId })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TeamsHalf_Teams");
            });
        }
    }
}
