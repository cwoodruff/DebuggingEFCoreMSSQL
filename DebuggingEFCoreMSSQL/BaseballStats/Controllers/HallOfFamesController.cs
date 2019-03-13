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
//              SELECT DISTINCT [p].[nameFirst], [p].[nameLast], [bt].[Hr]
//              FROM [Batting] AS [b]
//              INNER JOIN [HallOfFame] AS [hf] ON [b].[playerID] = [hf].[playerID]
//              INNER JOIN [People] AS [p] ON [b].[playerID] = [p].[playerID]
//              INNER JOIN [Teams] AS [t] ON (([b].[teamID] = [t].[teamID]) AND ([b].[lgID] = [t].[lgID])) AND ([b].[yearID] = [t].[yearID])
//              INNER JOIN [PlayerBattingTotals] AS [bt] ON [b].[playerID] = [bt].[PlayerId]
//              WHERE [bt].[Hr] > 500
//              ORDER BY [bt].[Hr] DESC

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
