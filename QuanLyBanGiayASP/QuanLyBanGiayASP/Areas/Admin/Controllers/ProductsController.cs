using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyBanGiayASP.Data;
using QuanLyBanGiayASP.Models;
using QuanLyBanGiayASP.Models.ViewModels;
using QuanLyBanGiayASP.Utility;

namespace QuanLyBanGiayASP.Areas.Admin.Controllers
{
    [Authorize(Roles = SD.SuperAdminEndUser)]
    [Area("Admin")]
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment;

        [BindProperty]
        public ProductViewModels ProductsVM { get; set; }


        public ProductsController(ApplicationDbContext db, IWebHostEnvironment webHostEnvironment)
        {
            _db = db;
            _webHostEnvironment = webHostEnvironment;
            ProductsVM = new ProductViewModels()
            {
                Merchants = _db.Merchants.ToList(),
                Products = new Models.Products(),
                Brands = _db.Brands.ToList()
            };

        }
        public async Task<IActionResult> Index()
        {
            var products = _db.Products.Include(m => m.Merchants).Include(m => m.Brands);
            return View(await products.ToListAsync());
        }

        ////Get : Products Create
        //public IActionResult Create()
        //{
        //    return View(ProductsVM);
        //}

        ////Post : Products Create
        //[HttpPost, ActionName("Create")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> CreatePOST()
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View(ProductsVM);
        //    }

        //    _db.Products.Add(ProductsVM.Products);
        //    await _db.SaveChangesAsync();

        //    //Image being saved

        //    string webRootPath = _webHostEnvironment.WebRootPath;
        //    var files = HttpContext.Request.Form.Files;

        //    var productsFromDb = _db.Products.Find(ProductsVM.Products.ID);

        //    if (files.Count != 0)
        //    {
        //        //Image has been uploaded
        //        var uploads = Path.Combine(webRootPath, SD.ImageFolder);
        //        var extension = Path.GetExtension(files[0].FileName);

        //        using (var filestream = new FileStream(Path.Combine(uploads, ProductsVM.Products.ID + extension), FileMode.Create))
        //        {
        //            files[0].CopyTo(filestream);
        //        }
        //        productsFromDb.Image = @"\" + SD.ImageFolder + @"\" + ProductsVM.Products.ID + extension;
        //    }
        //    else
        //    {
        //        //when user does not upload image
        //        var uploads = Path.Combine(webRootPath, SD.ImageFolder + @"\" + SD.DefaultProductImage);
        //        System.IO.File.Copy(uploads, webRootPath + @"\" + SD.ImageFolder + @"\" + ProductsVM.Products.ID + ".png");
        //        productsFromDb.Image = @"\" + SD.ImageFolder + @"\" + ProductsVM.Products.ID + ".png";
        //    }
        //    await _db.SaveChangesAsync();

        //    return RedirectToAction(nameof(Index));
        //}

        //GET : Edit
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ProductsVM.Products = await _db.Products.Include(m => m.Merchants).Include(m=>m.Brands).SingleOrDefaultAsync(m => m.ID == id);

            if (ProductsVM.Products == null)
            {
                return NotFound();
            }

            return View(ProductsVM);
        }


        //Post : Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id)
        {
            if (ModelState.IsValid)
            {
                string webRootPath = _webHostEnvironment.WebRootPath;
                var files = HttpContext.Request.Form.Files;

                var productFromDb = _db.Products.Where(m => m.ID == ProductsVM.Products.ID).FirstOrDefault();

                if (files.Count > 0 && files[0] != null)
                {
                    //if user uploads a new image
                    var uploads = Path.Combine(webRootPath, SD.ImageFolder);
                    var extension_new = Path.GetExtension(files[0].FileName);
                    var extension_old = Path.GetExtension(productFromDb.Image);

                    if (System.IO.File.Exists(Path.Combine(uploads, ProductsVM.Products.ID + extension_old)))
                    {
                        System.IO.File.Delete(Path.Combine(uploads, ProductsVM.Products.ID + extension_old));
                    }
                    using (var filestream = new FileStream(Path.Combine(uploads, ProductsVM.Products.ID + extension_new), FileMode.Create))
                    {
                        files[0].CopyTo(filestream);
                    }
                    ProductsVM.Products.Image = @"\" + SD.ImageFolder + @"\" + ProductsVM.Products.ID + extension_new;
                }

                if (ProductsVM.Products.Image != null)
                {
                    productFromDb.Image = ProductsVM.Products.Image;
                }

                productFromDb.Name = ProductsVM.Products.Name;
                productFromDb.Price = ProductsVM.Products.Price;
                productFromDb.Available = ProductsVM.Products.Available;
                productFromDb.MerchantId = ProductsVM.Products.MerchantId;
                productFromDb.BrandId = ProductsVM.Products.BrandId;
                productFromDb.Description = ProductsVM.Products.Description;
                await _db.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(ProductsVM);
        }


        //GET : Details
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ProductsVM.Products = await _db.Products.Include(m => m.Merchants).Include(m => m.Brands).SingleOrDefaultAsync(m => m.ID == id);

            if (ProductsVM.Products == null)
            {
                return NotFound();
            }

            return View(ProductsVM);
        }

        //GET : Delete
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ProductsVM.Products = await _db.Products.Include(m => m.Merchants).Include(m => m.Brands).SingleOrDefaultAsync(m => m.ID == id);

            if (ProductsVM.Products == null)
            {
                return NotFound();
            }

            return View(ProductsVM);
        }

        //POST : Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            string webRootPath = _webHostEnvironment.WebRootPath;
            Products products = await _db.Products.FindAsync(id);

            if (products == null)
            {
                return NotFound();
            }
            else
            {
                var uploads = Path.Combine(webRootPath, SD.ImageFolder);
                var extension = Path.GetExtension(products.Image);

                if (System.IO.File.Exists(Path.Combine(uploads, products.ID + extension)))
                {
                    System.IO.File.Delete(Path.Combine(uploads, products.ID + extension));
                }
                _db.Products.Remove(products);
                await _db.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
        }
    }
}