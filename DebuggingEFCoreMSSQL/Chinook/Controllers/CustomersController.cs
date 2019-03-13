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
                .TagWith("Description: Query for Customers")
                .TagWith("Query located: Chinook.Controllers.CustomersController.Index() method")
                .TagWith(
                    @"Parameters:
                    None");
            return View(await chinookContext.ToListAsync());
        }
    }
}
