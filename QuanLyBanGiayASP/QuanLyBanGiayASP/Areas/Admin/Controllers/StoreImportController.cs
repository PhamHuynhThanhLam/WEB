using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize(Roles = SD.SuperAdminEndUser)]
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
                Brands = _db.Brands.ToList()
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
        public async Task<IActionResult> CreatePost(int id)
        {
            List<int> lstProduct = HttpContext.Session.Get<List<int>>("ssCart");
            if (lstProduct == null)
            {
                lstProduct = new List<int>();
            }
            _db.Products.Add(StoreImportVM.Products);
            await _db.SaveChangesAsync();
           
            string webRootPath = _webHostEnvironment.WebRootPath;
            var files = HttpContext.Request.Form.Files;
           
            var productsFromDb = _db.Products.Find(StoreImportVM.Products.ID);

            if (files.Count != 0)
            {
                //Image has been uploaded
                var uploads = Path.Combine(webRootPath, SD.ImageFolder);
                var extension = Path.GetExtension(files[0].FileName);

                using (var filestream = new FileStream(Path.Combine(uploads, StoreImportVM.Products.ID + extension), FileMode.Create))
                {
                    files[0].CopyTo(filestream);
                }
                productsFromDb.Image = @"\" + SD.ImageFolder + @"\" + StoreImportVM.Products.ID + extension;
            }
            else
            {
                //when user does not upload image
                var uploads = Path.Combine(webRootPath, SD.ImageFolder + @"\" + SD.DefaultProductImage);

                System.IO.File.Copy(uploads, webRootPath + @"\" + SD.ImageFolder + @"\" + StoreImportVM.Products.ID + ".png");
                productsFromDb.Image = @"\" + SD.ImageFolder + @"\" + StoreImportVM.Products.ID + ".png";
                
            }
            await _db.SaveChangesAsync();

            int idproduct = StoreImportVM.Products.ID;
            lstProduct.Add(idproduct);
            HttpContext.Session.Set("ssCart", lstProduct);
            return RedirectToAction(nameof(Index));
        }

        //Add Product
        public async Task<IActionResult> Add()
        {
            List<int> lstProduct = HttpContext.Session.Get<List<int>>("ssCart");
            if (lstProduct?.Count > 0)
            {

                foreach (int cartItem in lstProduct)
                {
                    Products prod = _db.Products.Include(p => p.Merchants).Include(p => p.Brands).Where(p => p.ID == cartItem).FirstOrDefault();
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
            List<int> lstProduct = HttpContext.Session.Get<List<int>>("ssCart");

            StoreImportVM.ImportDetails.DateImport = StoreImportVM.ImportDetails.DateImport;

            //upload chi tiết nhập với tổng giá tiền
            ImportDetails import = StoreImportVM.ImportDetails;
            double total = 0;
            var productlist = _db.Products.ToList();
            foreach (int item in lstProduct)
            {
                for(int i=0; i<productlist.Count ; i++)
                {
                    if(item == productlist[i].ID)
                    {
                        total = total + productlist[i].Price * productlist[i].InStock;
                    }
                }            
            }
            import.Total = Convert.ToInt32(total);
            _db.ImportDetails.Add(import);
            _db.SaveChanges();

            int idimport = import.ID;

            //update store
            foreach (int item in lstProduct)
            {
                Stores stores = new Stores()
                {
                    ImportDetailID = idimport,
                    ProductID = item
                };
                _db.Stores.Add(stores);
            }
            _db.SaveChanges();

            lstProduct = new List<int>();
            HttpContext.Session.Set("ssCart", lstProduct);

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Remove(int id)
        {
            List<int> lstProduct = HttpContext.Session.Get<List<int>>("ssCart");
            string webRootPath = _webHostEnvironment.WebRootPath;


            if (lstProduct.Count > 0)
            {
                if (lstProduct.Contains(id))
                {
                    Products productdelete = _db.Products.Find(id);

                    var uploads = Path.Combine(webRootPath, SD.ImageFolder);
                    var extension = Path.GetExtension(productdelete.Image);
                    if (System.IO.File.Exists(Path.Combine(uploads, productdelete.ID + extension)))
                    {
                        System.IO.File.Delete(Path.Combine(uploads, productdelete.ID + extension));
                    }

                    _db.Products.Remove(productdelete);
                    lstProduct.Remove(id);
                    _db.SaveChanges();

                }
            }

            HttpContext.Session.Set("ssCart", lstProduct);

            return RedirectToAction(nameof(Index));
        }

    }
}