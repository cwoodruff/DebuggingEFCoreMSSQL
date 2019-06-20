using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Chinook.Models;

namespace Chinook.Controllers
{
    public class AlbumsController : Controller
    {
        private readonly ChinookContext _context;

        public AlbumsController(ChinookContext context)
        {
            _context = context;
        }

        // GET: Albums
        public async Task<IActionResult> Index()
        {
            var albums = _context.Album.Include(a => a.Artist)
                .TagWith("Description: Query for Albums")
                .TagWith("Query located: Chinook.Controllers.AlbumsController.Index() method")
                .TagWith(@"Parameters:
                    None");
            return View(await albums.ToListAsync());
        }
    }
}
