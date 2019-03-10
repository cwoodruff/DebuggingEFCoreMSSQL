using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BaseballStats.Models;

namespace BaseballStats.Controllers
{
    public class HallOfFamesController : Controller
    {
        private readonly BaseballStatsContext _context;

        public HallOfFamesController(BaseballStatsContext context)
        {
            _context = context;
        }

        // GET: HallOfFames
        public async Task<IActionResult> Index()
        {
//            SELECT DISTINCT [plp].[nameFirst], [plp].[nameLast], [tf].[franchName] AS [Franchise]
//            FROM [SeriesPost] AS [sp]
//            INNER JOIN [Teams] AS [t] ON (([sp].[teamIDwinner] = [t].[teamID]) AND ([sp].[lgIDwinner] = [t].[lgID])) AND ([sp].[yearID] = [t].[yearID])
//            INNER JOIN [TeamsFranchises] AS [tf] ON [t].[franchID] = [tf].[franchID]
//            INNER JOIN [Pitching] AS [p] ON (([t].[teamID] = [p].[teamID]) AND ([t].[lgID] = [p].[lgID])) AND ([t].[yearID] = [p].[yearID])
//            INNER JOIN [People] AS [plp] ON [p].[playerID] = [plp].[playerID]
//            WHERE ([sp].[round] = N'WS') AND ([sp].[yearID] = CAST(2018 AS smallint))
//            ORDER BY [plp].[nameLast]

            var homeRunLeaders = await (from b in _context.Batting
                join hf in _context.HallOfFame on b.PlayerId equals hf.PlayerId
                join p in _context.People on b.PlayerId equals p.PlayerId
                join t in _context.Teams on new {A = b.TeamId, B = b.LgId, C = b.YearId}
                    equals new {A = t.TeamId, B = t.LgId, C = t.YearId}
                join bt in _context.PlayerBattingTotals on b.PlayerId equals bt.PlayerId
                where bt.Hr > 500
                orderby bt.Hr descending
                select new HomeRunLeaders {NameFirst = p.NameFirst, NameLast = p.NameLast, Hr = bt.Hr})
                .TagWith("Description: Query for All Hitters with 500 Home Runs")
                .TagWith("Query located: BaseballStats.Controllers.HallOfFamesController.Index() method")
                .TagWith(
                    @"Parameters:
                    None")
                .Distinct().ToListAsync();
                
            return View(homeRunLeaders);
        }
    }
}
