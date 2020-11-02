using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Server.HttpSys;
using Microsoft.EntityFrameworkCore;
using Mono.Data;
using Mono.Models;

namespace Mono.Controllers
{
    public class VehicleMakesController : Controller
    {
        private readonly VehicleContext _context;

        public VehicleMakesController(VehicleContext context)
        {
            _context = context;
        }

        // GET: VehicleMakes
        public async Task<IActionResult> Index(string sortOrder, string searchString)
        {
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name" : "";
            ViewData["CurrentFilter"] = searchString;

            var vehicleMakes = from v in _context.vehicleMakes
                               select v;
            if (!String.IsNullOrEmpty(searchString))
            {
                vehicleMakes = vehicleMakes.Where(v => v.name.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name":
                    vehicleMakes = vehicleMakes.OrderByDescending(v => v.name);
                    break;
                case "Date":
                    vehicleMakes = vehicleMakes.OrderByDescending(v => v.abrv);
                    break;
                default:
                    vehicleMakes = vehicleMakes.OrderBy(v => v.name);
                    break;
            }

            return View(await vehicleMakes.AsNoTracking().ToListAsync());
        }

        // GET: VehicleMakes/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicleMake = await _context.vehicleMakes
                .FirstOrDefaultAsync(m => m.id == id);
            if (vehicleMake == null)
            {
                return NotFound();
            }

            return View(vehicleMake);
        }

        // GET: VehicleMakes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: VehicleMakes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,name,abrv")] VehicleMake vehicleMake)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(vehicleMake);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Unable to save changes. " +
           "Try again, and if the problem persists " +
           "see your system administrator.");
            }
           
            return View(vehicleMake);
        }

        // GET: VehicleMakes/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicleMake = await _context.vehicleMakes.FindAsync(id);
            if (vehicleMake == null)
            {
                return NotFound();
            }
            return View(vehicleMake);
        }

        // POST: VehicleMakes/Edit/5
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var makeToUpdate = await _context.vehicleMakes.FirstOrDefaultAsync(m => m.id == id);

            if (await TryUpdateModelAsync<VehicleMake>(
               makeToUpdate,
               "",
               m => m.name, m => m.abrv ))
            {
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException )
                {
 
                    ModelState.AddModelError("", "Unable to save changes. " +
                        "Try again, and if the problem persists, " +
                        "see your system administrator.");
                }
            }


            return View(makeToUpdate);
        }

        // GET: VehicleMakes/Delete/5
        public async Task<IActionResult> Delete(long? id, bool? saveChangesError=false)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicleMake = await _context.vehicleMakes
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.id == id);
            if (vehicleMake == null)
            {
                return NotFound();
            }
            if (saveChangesError.GetValueOrDefault())
            {
                ViewData["ErrorMessage"] =
                    "Delete failed. Try again, and if the problem persists " +
                    "see your system administrator.";
            }


            return View(vehicleMake);
        }

        // POST: VehicleMakes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var vehicleMake = await _context.vehicleMakes.FindAsync(id);
            if (vehicleMake == null)
            {
                return RedirectToAction(nameof(Index));
            }
            try
            {
                _context.vehicleMakes.Remove(vehicleMake);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                return RedirectToAction(nameof(Delete), new { id = id, saveChangesError = true });
            }
           
        }

        private bool VehicleMakeExists(long id)
        {
            return _context.vehicleMakes.Any(e => e.id == id);
        }
    }
}
