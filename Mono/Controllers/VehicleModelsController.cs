using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Mono.Data;
using Mono.Models;

namespace Mono.Controllers
{
    public class VehicleModelsController : Controller
    {
        private readonly VehicleContext _context;

        public VehicleModelsController(VehicleContext context)
        {
            _context = context;
        }

        // GET: VehicleModels
        public async Task<IActionResult> Index(string sortOrder, string searchString)
        {
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name" : "";
            ViewData["CurrentFilter"] = searchString;

            var vehicleModels = from v in _context.vehicleModels
                               select v;
            if (!String.IsNullOrEmpty(searchString))
            {
                vehicleModels = vehicleModels.Where(v => v.name.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name":
                    vehicleModels = vehicleModels.OrderByDescending(v => v.name);
                    break;
                case "abrv":
                    vehicleModels = vehicleModels.OrderByDescending(v => v.abrv);
                    break;
                default:
                    vehicleModels = vehicleModels.OrderBy(v => v.name);
                    break;
            }

            return View(await vehicleModels.AsNoTracking().ToListAsync());
            
        }

        // GET: VehicleModels/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicleModel = await _context.vehicleModels
                .FirstOrDefaultAsync(m => m.id == id);
            if (vehicleModel == null)
            {
                return NotFound();
            }

            return View(vehicleModel);
        }

        // GET: VehicleModels/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: VehicleModels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,name,abrv")] VehicleModel vehicleModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(vehicleModel);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Unable to save changes. ");
            }
            
            return View(vehicleModel);
        }

        // GET: VehicleModels/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicleModel = await _context.vehicleModels.FindAsync(id);
            if (vehicleModel == null)
            {
                return NotFound();
            }
            return View(vehicleModel);
        }

        // POST: VehicleModels/Edit/5
       
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var modelToUpdate= await _context.vehicleModels.FirstOrDefaultAsync(m => m.id == id);
            if (await TryUpdateModelAsync<VehicleModel>(
              modelToUpdate,
              "",
              m => m.name, m => m.abrv))
            {
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException)
                {

                    ModelState.AddModelError("", "Unable to save changes. " +
                        "Try again, and if the problem persists, " +
                        "see your system administrator.");
                }
            }

            return View(modelToUpdate);
        }

        // GET: VehicleModels/Delete/5
        public async Task<IActionResult> Delete(long? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicleModel = await _context.vehicleModels
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.id == id);
            if (vehicleModel == null)
            {
                return NotFound();
            }
            if (saveChangesError.GetValueOrDefault())
            {
                ViewData["ErrorMessage"] =
                    "Delete failed. Try again, and if the problem persists " +
                    "see your system administrator.";
            }

            return View(vehicleModel);
        }

        // POST: VehicleModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var vehicleModel = await _context.vehicleModels.FindAsync(id);

            if (vehicleModel == null)
            {
                return RedirectToAction(nameof(Index));
            }
            try
            {
                _context.vehicleModels.Remove(vehicleModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                return RedirectToAction(nameof(Delete), new { id = id, saveChangesError = true });

            }

        }

        private bool VehicleModelExists(long id)
        {
            return _context.vehicleModels.Any(e => e.id == id);
        }
    }
}
