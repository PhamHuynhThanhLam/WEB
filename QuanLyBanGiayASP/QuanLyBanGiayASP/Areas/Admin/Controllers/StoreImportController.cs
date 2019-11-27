using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyBanGiayASP.Data;
using QuanLyBanGiayASP.Extensions;
using QuanLyBanGiayASP.Models;
using QuanLyBanGiayASP.Models.ViewModels;
using QuanLyBanGiayASP.Utility;

namespace QuanLyBanGiayASP.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class StoreImportController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment;
        [BindProperty]
        public StoreImportDetailViewModels StoreImportVM { get; set; }


        public StoreImportController(ApplicationDbContext db, IWebHostEnvironment webHostEnvironment)
        {
            _db = db;
            _webHostEnvironment = webHostEnvironment;
            StoreImportVM = new StoreImportDetailViewModels()
            {
                Products = new Models.Products(),
                Merchants = _db.Merchants.ToList(),
                ListProducts = new List<Models.Products>(),
                
            };
        }
        public async Task<IActionResult> Index()
        {
            var import = _db.ImportDetails;
            return View(await import.ToListAsync());
        }

        //Get : Import Create
        public async Task<IActionResult> Create()
        {
            return View(StoreImportVM);
        }

        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePost(Products products)
        {

            List<AddProducts> lstProduct = HttpContext.Session.Get<List<AddProducts>>("ssCart");
            if (lstProduct == null)
            {
                lstProduct = new List<AddProducts>();
            }

            string webRootPath = _webHostEnvironment.WebRootPath;
            var files = HttpContext.Request.Form.Files;



            if (files.Count != 0)
            {
                //Image has been uploaded
                var uploads = Path.Combine(webRootPath, SD.ImageFolder);
                var extension = Path.GetExtension(files[0].FileName);

                using (var filestream = new FileStream(Path.Combine(uploads, StoreImportVM.Products.ID + extension), FileMode.Create))
                {
                    files[0].CopyTo(filestream);
                }
                products.Image = @"\" + SD.ImageFolder + @"\" + StoreImportVM.Products.ID + extension;
            }
            else
            {
                //when user does not upload image
                var uploads = Path.Combine(webRootPath, SD.ImageFolder + @"\" + SD.DefaultProductImage);
                System.IO.File.Copy(uploads, webRootPath + @"\" + SD.ImageFolder + @"\" + StoreImportVM.Products.ID + ".png");
                products.Image = @"\" + SD.ImageFolder + @"\" + StoreImportVM.Products.ID + ".png";
            }

            lstProduct.Add(new AddProducts()
            {
                Products = products
            });

            HttpContext.Session.Set("ssCart", lstProduct);
            return RedirectToAction("Create", "StoreImport", new { area = "Admin" });


        }

        //Add Product
        public async Task<IActionResult> Add()
        {
            List<AddProducts> lstProduct = HttpContext.Session.Get<List<AddProducts>>("ssCart");
            if (lstProduct?.Count > 0)
            {

                foreach (AddProducts cartItem in lstProduct)
                {
                    Products prod = cartItem.Products;
                    prod.MerchantId = cartItem.Products.MerchantId;
                    var merchant = _db.Merchants.Where(a=>a.ID == prod.MerchantId).FirstOrDefault();
                    prod.Merchants = merchant;
                    StoreImportVM.ListProducts.Add(prod);
                }

            }

            return View(StoreImportVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Add")]
        public IActionResult AddPost()
        {
            List<AddProducts> lstProduct = HttpContext.Session.Get<List<AddProducts>>("ssCart");

            StoreImportVM.ImportDetails.DateImport = StoreImportVM.ImportDetails.DateImport;

            ImportDetails import = StoreImportVM.ImportDetails;

            double total = 0;
            foreach (AddProducts item in lstProduct)
            {
                total = total + item.Products.Price * item.Products.InStock;
            }
            import.Total = Convert.ToInt32(total);
            _db.ImportDetails.Add(import);
            _db.SaveChanges();

            int idimport = import.ID;

            foreach (AddProducts item in lstProduct)
            {
                _db.Products.Add(item.Products);
                _db.SaveChanges();   
            }

            var product = _db.Products.ToList();
            foreach (AddProducts item in lstProduct)
            {
                Stores stores = new Stores()
                {
                    ImportDetailID = idimport,
                    ProductID = item.Products.ID
                };
                _db.Stores.Add(stores);
            }
            _db.SaveChanges();

            lstProduct = new List<AddProducts>();
            HttpContext.Session.Set("ssCart", lstProduct);

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Remove(Products products)
        {
            List<AddProducts> lstProduct = HttpContext.Session.Get<List<AddProducts>>("ssCart");

            if (lstProduct.Count > 0)
            {
                foreach (AddProducts item in lstProduct)
                {
                    if (item.Products.ID == products.ID)
                    {
                        lstProduct.Remove(item);
                        break;
                    }
                }

            }

            HttpContext.Session.Set("ssCart", lstProduct);

            return RedirectToAction(nameof(Index));
        }

    }
}