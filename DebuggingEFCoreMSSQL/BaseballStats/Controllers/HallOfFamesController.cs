﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BaseballStats.Models;
using Remotion.Linq.Clauses;

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
//            SELECT DISTINCT People.nameFirst, People.nameLast, PlayerBattingTotals.HR
//                FROM  Batting
//             INNER JOIN HallOfFame ON Batting.playerID = HallOfFame.playerID
//             INNER JOIN People ON Batting.playerID = People.playerID AND HallOfFame.playerID = People.playerID
//             INNER JOIN Teams ON Batting.teamID = Teams.teamID AND Batting.lgID = Teams.lgID AND Batting.yearID = Teams.yearID
//             INNER JOIN PlayerBattingTotals ON Batting.playerID = PlayerBattingTotals.playerID
//            WHERE (PlayerBattingTotals.HR > 500)
//            ORDER BY PlayerBattingTotals.HR DESC

            var homeRunLeaders = await (from b in _context.Batting
                join hf in _context.HallOfFame on b.PlayerId equals hf.PlayerId
                join p in _context.People on b.PlayerId equals p.PlayerId
                join t in _context.Teams on new {A = b.TeamId, B = b.LgId, C = b.YearId}
                    equals new {A = t.TeamId, B = t.LgId, C = t.YearId}
                join bt in _context.PlayerBattingTotals on b.PlayerId equals bt.PlayerId
                where bt.Hr > 500
                orderby bt.Hr descending
                select new HomeRunLeaders {NameFirst = p.NameFirst, NameLast = p.NameLast, Hr = bt.Hr})
                .Distinct().ToListAsync();
                
            return View(homeRunLeaders);
        }
    }
}
