using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using QuanLyBanGiayASP.Data;
using QuanLyBanGiayASP.Extensions;
using QuanLyBanGiayASP.Models;
using QuanLyBanGiayASP.Models.ViewModels;

namespace QuanLyBanGiayASP.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {

        private readonly ApplicationDbContext _db;

        public HomeController(ApplicationDbContext db)
        {
            _db = db;
        }


        public async Task<IActionResult> Index()
        {
            var productList = await _db.Products.Include(m => m.Merchants).Include(m => m.Brands).ToListAsync();

            return View(productList);
        }
        public async Task<IActionResult> Details(int id)
        {
            var product = await _db.Products.Include(m => m.Merchants).Include(m => m.Brands).Where(m => m.ID == id).FirstOrDefaultAsync();

            return View(product);
        }

        [HttpPost, ActionName("Details")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DetailsPost(int id,int Amount)
        {
            List<Amount_Product> lstShoppingCart = HttpContext.Session.Get<List<Amount_Product>>("ssShoppingCart");
            if (lstShoppingCart == null)
            {
               
                lstShoppingCart = new List<Amount_Product>();
            }
            lstShoppingCart.Add(new Amount_Product()
            {
                IDProduct = id,
                Amount = Amount
            }) ;
            HttpContext.Session.Set("ssShoppingCart", lstShoppingCart);
            return RedirectToAction("Index", "Home", new { area = "Customer" });

        }

        public IActionResult Remove(int id)
        {
            List<Amount_Product> lstShoppingCart = HttpContext.Session.Get<List<Amount_Product>>("ssShoppingCart");
            if (lstShoppingCart.Count > 0)
            {
                foreach (Amount_Product item in lstShoppingCart)
                {
                    if(item.IDProduct == id)
                    {
                        lstShoppingCart.Remove(item);

                        break;
                    }
                }
              
            }

            HttpContext.Session.Set("ssShoppingCart", lstShoppingCart);

            return RedirectToAction(nameof(Index));
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
