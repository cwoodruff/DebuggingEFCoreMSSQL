using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BaseballStats.Models;

namespace BaseballStats.Controllers
{
    public class SeriesPostsController : Controller
    {
        private readonly BaseballStatsContext _context;

        public SeriesPostsController(BaseballStatsContext context)
        {
            _context = context;
        }

        // GET: SeriesPosts
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
                
            var winningPitchers = await (from sp in _context.SeriesPost
                    join t in _context.Teams on new {A = sp.TeamIdwinner, B = sp.LgIdwinner, C = sp.YearId}
                        equals new {A = t.TeamId, B = t.LgId, C = t.YearId}
                    join tf in _context.TeamsFranchises on t.FranchId equals tf.FranchId
                    join p in _context.Pitching on new {A = t.TeamId, B = t.LgId, C = t.YearId}
                        equals new {A = p.TeamId, B = p.LgId, C = p.YearId}
                    join plp in _context.People on p.PlayerId equals plp.PlayerId
                    where sp.Round == "WS" && sp.YearId == 2018
                    orderby plp.NameLast
                    select new WinningPitchers {NameFirst = plp.NameFirst, NameLast = plp.NameLast, Franchise = tf.FranchName})
                .TagWith("Description: Query for Players of the 2018 World Series Champions")
                .TagWith("Query located: BaseballStats.Controllers.SeriesPostsController.Index() method")
                .TagWith(
                    @"Parameters:
                    None")
                .Distinct().ToListAsync();
                
            return View(winningPitchers);
        }
    }
}
