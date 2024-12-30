using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RentSomeWheels.Models;

namespace RentSomeWheels.Controllers
{
    public class RentalContractsController : Controller
    {
        private readonly RentSomeWheelsContext _context;

        public RentalContractsController(RentSomeWheelsContext context)
        {
            _context = context;
        }

        // GET: RentalContracts
        public async Task<IActionResult> Index()
        {
            var rentSomeWheelsContext = _context.RentalContracts.Include(r => r.Client).Include(r => r.Vehicle);
            return View(await rentSomeWheelsContext.ToListAsync());
        }

        // GET: RentalContracts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rentalContract = await _context.RentalContracts
                .Include(r => r.Client)
                .Include(r => r.Vehicle)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (rentalContract == null)
            {
                return NotFound();
            }

            return View(rentalContract);
        }

        // GET: RentalContracts/Create
        public IActionResult Create()
        {
            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "Id");
            ViewData["VehicleId"] = new SelectList(_context.Vehicles, "Id", "Id");
            return View();
        }

        // POST: RentalContracts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ClientId,VehicleId,StartDate,EndDate,InitialMileage")] RentalContract rentalContract)
        {
            if (ModelState.IsValid)
            {
                var vehicle = await _context.Vehicles.FindAsync(rentalContract.VehicleId);
                if (vehicle != null)
                {
                    vehicle.IsRented = true;
                    _context.Update(vehicle);
                }

                _context.Add(rentalContract);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Log validation errors
            foreach (var modelState in ModelState.Values)
            {
                foreach (var error in modelState.Errors)
                {
                    Console.WriteLine(error.ErrorMessage);
                }
            }

            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "Id", rentalContract.ClientId);
            ViewData["VehicleId"] = new SelectList(_context.Vehicles, "Id", "Id", rentalContract.VehicleId);
            return View(rentalContract);
        }

        // GET: RentalContracts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rentalContract = await _context.RentalContracts.FindAsync(id);
            if (rentalContract == null)
            {
                return NotFound();
            }
            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "Id", rentalContract.ClientId);
            ViewData["VehicleId"] = new SelectList(_context.Vehicles, "Id", "Id", rentalContract.VehicleId);
            return View(rentalContract);
        }

        // POST: RentalContracts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ClientId,VehicleId,StartDate,EndDate")] RentalContract rentalContract)
        {
            if (id != rentalContract.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingContract = await _context.RentalContracts.AsNoTracking().FirstOrDefaultAsync(rc => rc.Id == id);
                    if (existingContract == null)
                    {
                        return NotFound();
                    }

                    var vehicle = await _context.Vehicles.FindAsync(rentalContract.VehicleId);
                    if (vehicle != null)
                    {
                        // Check if the rental period has changed
                        if (existingContract.StartDate != rentalContract.StartDate || existingContract.EndDate != rentalContract.EndDate)
                        {
                            // Update the IsRented status based on the new rental period
                            if (rentalContract.StartDate <= DateTime.Now && rentalContract.EndDate >= DateTime.Now)
                            {
                                vehicle.IsRented = true;
                            }
                            else
                            {
                                vehicle.IsRented = false;
                            }
                            _context.Update(vehicle);
                        }
                    }

                    _context.Update(rentalContract);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RentalContractExists(rentalContract.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "Id", rentalContract.ClientId);
            ViewData["VehicleId"] = new SelectList(_context.Vehicles, "Id", "Id", rentalContract.VehicleId);
            return View(rentalContract);
        }

        // GET: RentalContracts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rentalContract = await _context.RentalContracts
                .Include(r => r.Client)
                .Include(r => r.Vehicle)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (rentalContract == null)
            {
                return NotFound();
            }

            return View(rentalContract);
        }

        // POST: RentalContracts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var rentalContract = await _context.RentalContracts.FindAsync(id);
            if (rentalContract != null)
            {
                var vehicle = await _context.Vehicles.FindAsync(rentalContract.VehicleId);
                if (vehicle != null)
                {
                    vehicle.IsRented = false;
                    _context.Update(vehicle);
                }

                _context.RentalContracts.Remove(rentalContract);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool RentalContractExists(int id)
        {
            return _context.RentalContracts.Any(e => e.Id == id);
        }
    }
}
