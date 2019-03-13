using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Chinook.Models;

namespace Chinook.Controllers
{
    public class ArtistsController : Controller
    {
        private readonly ChinookContext _context;

        public ArtistsController(ChinookContext context)
        {
            _context = context;
        }

        // GET: Artists
        public async Task<IActionResult> Index()
        {
            var artists = _context.Artist.Include(a => a.Album)
                .TagWith("Description: Query for Artists")
                .TagWith("Query located: Chinook.Controllers.ArtistsController.Index() method")
                .TagWith(
                    @"Parameters:
                    None");
            return View(await artists.ToListAsync());
        }
    }
}
