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
    public class MerchantsController : Controller
    {
        private readonly ApplicationDbContext _db;
        public MerchantsController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            return View(_db.Merchants.ToList());
        }

        //GET Create Action Method
        public IActionResult Create()
        {
            return View();
        }

        //POST Create action Method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Merchants merchants)
        {
            if (ModelState.IsValid)
            {
                _db.Add(merchants);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(merchants);
        }


        //GET Edit Action Method
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var merchant = await _db.Merchants.FindAsync(id);
            if (merchant == null)
            {
                return NotFound();
            }

            return View(merchant);
        }

        //POST Edit action Method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Merchants merchants)
        {
            if (id != merchants.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _db.Update(merchants);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(merchants);
        }

        //GET Details Action Method
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var merchant = await _db.Merchants.FindAsync(id);
            if (merchant == null)
            {
                return NotFound();
            }

            return View(merchant);
        }


        //GET Delete Action Method
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var merchant = await _db.Merchants.FindAsync(id);
            if (merchant == null)
            {
                return NotFound();
            }

            return View(merchant);
        }

        //POST Delete action Method
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var merchant = await _db.Merchants.FindAsync(id);
            _db.Merchants.Remove(merchant);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}