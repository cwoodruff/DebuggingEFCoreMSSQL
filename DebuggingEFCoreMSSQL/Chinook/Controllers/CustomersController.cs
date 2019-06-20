using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chinook.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Chinook.Controllers
{
    public class CustomersController : Controller
    {
        private readonly ChinookContext _context;

        public CustomersController(ChinookContext context)
        {
            _context = context;
        }

        // GET: Customers
        public async Task<IActionResult> Index()
        {
            var chinookContext = _context.Customer.Include(c => c.SupportRep)
                .TagWith("Description: Customers Query from Query Tag demo")
                .TagWith("Query located: Chinook.Controllers.Customer.Index Action method")
                .TagWith("Parameters: None");
            return View(await chinookContext.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            var customerTracks = new List<CustomerTrack>();

            if (id == null) return NotFound();

            var customerOrders = await (from c in _context.Customer
                        .TagWith("Description: Customer Orders Query from Query Tag demo")
                        .TagWith("Query located: Chinook.Controllers.Customer.Details Action method")
                        .TagWith(@"Parameters:
                            CustomerId")
                    join i in _context.Invoice on c.CustomerId equals i.CustomerId
                    join il in _context.InvoiceLine on i.InvoiceId equals il.InvoiceId
                    join t in _context.Track on il.TrackId equals t.TrackId
                    join a in _context.Album on t.AlbumId equals a.AlbumId
                    join ar in _context.Artist on a.ArtistId equals ar.ArtistId
                    where c.CustomerId == 2
                    orderby a.Title
                    select new
                    {
                        c.CustomerId, TrackName = t.Name, AlbumName = a.Title, ArtistName = ar.Name,
                        i.InvoiceDate
                    })
                .ToListAsync();

            if (customerOrders == null) return NotFound();

            foreach (var i in customerOrders)
            {
                var info = new CustomerTrack
                {
                    CustomerId = i.CustomerId,
                    TrackName = i.TrackName,
                    AlbumName = i.AlbumName,
                    ArtistName = i.ArtistName,
                    OrderDate = i.InvoiceDate
                };
                customerTracks.Add(info);
            }

            return View(customerTracks);
        }
    }
}