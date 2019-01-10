using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
//            SELECT DISTINCT TeamsFranchises.franchName, People.nameFirst, People.nameLast
//                FROM  SeriesPost INNER JOIN
//                Teams ON SeriesPost.teamIDwinner = Teams.teamID AND SeriesPost.lgIDwinner = Teams.lgID AND SeriesPost.yearID = Teams.yearID INNER JOIN
//            TeamsFranchises ON Teams.franchID = TeamsFranchises.franchID INNER JOIN
//            Pitching ON Teams.teamID = Pitching.teamID AND Teams.lgID = Pitching.lgID AND Teams.yearID = Pitching.yearID INNER JOIN
//            People ON Pitching.playerID = People.playerID
//            WHERE SeriesPost.yearID = 2018 AND
//            SeriesPost.round = 'WS'
//            ORDER BY People.nameLast
                
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
                .Distinct().ToListAsync();
                
            return View(winningPitchers);
        }
    }
}
