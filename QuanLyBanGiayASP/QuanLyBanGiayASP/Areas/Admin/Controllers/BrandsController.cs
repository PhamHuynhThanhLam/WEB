using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuanLyBanGiayASP.Data;
using QuanLyBanGiayASP.Models;
using QuanLyBanGiayASP.Utility;

namespace QuanLyBanGiayASP.Areas.Admin.Controllers
{
    [Authorize(Roles = SD.SuperAdminEndUser)]
    [Area("Admin")]
    public class BrandsController : Controller
    {
        private readonly ApplicationDbContext _db;
        public BrandsController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            return View(_db.Brands.ToList());
        }

        //GET Create Action Method
        public IActionResult Create()
        {
            return View();
        }

        //POST Create action Method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Brands brands)
        {
            if (ModelState.IsValid)
            {
                _db.Add(brands);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(brands);
        }


        //GET Edit Action Method
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var brand = await _db.Brands.FindAsync(id);
            if (brand == null)
            {
                return NotFound();
            }

            return View(brand);
        }

        //POST Edit action Method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Brands brands)
        {
            if (id != brands.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _db.Update(brands);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(brands);
        }

        //GET Details Action Method
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var brand = await _db.Brands.FindAsync(id);
            if (brand == null)
            {
                return NotFound();
            }

            return View(brand);
        }


        //GET Delete Action Method
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var brand = await _db.Brands.FindAsync(id);
            if (brand == null)
            {
                return NotFound();
            }

            return View(brand);
        }

        //POST Delete action Method
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var brand = await _db.Brands.FindAsync(id);
            _db.Brands.Remove(brand);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}