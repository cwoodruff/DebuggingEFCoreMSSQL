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
            var baseballStatsContext = _context.SeriesPost.Include(s => s.Teams).Include(s => s.TeamsNavigation);
            return View(await baseballStatsContext.ToListAsync());
        }
    }
}
