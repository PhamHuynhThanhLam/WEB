using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyBanGiayASP.Data;
using QuanLyBanGiayASP.Extensions;
using QuanLyBanGiayASP.Models;
using QuanLyBanGiayASP.Models.ViewModels;

namespace QuanLyBanGiayASP.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class ShoppingCartController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment;

        [BindProperty]
        public ShoppingCartViewModels ShoppingCartVM { get; set; }
       

        public ShoppingCartController(ApplicationDbContext db, IWebHostEnvironment webHostEnvironment)
        {
            this._db = db;
            _webHostEnvironment = webHostEnvironment;
            ShoppingCartVM = new ShoppingCartViewModels()
            {
                Products = new List<Models.Products>()
            };
        }
        public async Task<IActionResult> Index()
        {
            List<Amount_Product> lstShoppingCart = HttpContext.Session.Get<List<Amount_Product>>("ssShoppingCart");
            if (lstShoppingCart?.Count > 0)
            {
                
                foreach (Amount_Product cartItem in lstShoppingCart)
                {
                    Products prod = _db.Products.Include(p => p.Merchants).Where(p => p.ID == cartItem.IDProduct).FirstOrDefault();
                   
                    ShoppingCartVM.Products.Add(prod);
                }
               
            }

            return View(ShoppingCartVM);


        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Index")]
        public IActionResult IndexPost()
        {
            List<Amount_Product> lstCartItems = HttpContext.Session.Get<List<Amount_Product>>("ssShoppingCart");

            ShoppingCartVM.Orders.Date = ShoppingCartVM.Orders.Date
                                                            .AddHours(ShoppingCartVM.Orders.Time.Hour)
                                                            .AddMinutes(ShoppingCartVM.Orders.Time.Minute);

            Customers customer = ShoppingCartVM.Orders.Customers;
            _db.Customers.Add(customer);
            _db.SaveChanges();

            int idcustomer = customer.ID;

            Orders orders = ShoppingCartVM.Orders;
            orders.CustomerID = idcustomer;
            _db.Orders.Add(orders);
            _db.SaveChanges();

            int orderId = orders.ID;

            var ProductDB = _db.Products.ToList();

            foreach (Amount_Product item in lstCartItems)
            {
                OrderItems orderItems = new OrderItems()
                {
                    OrderID = orderId,
                    ProductID = item.IDProduct,
                    Amount = item.Amount
                };
                for (int i = 0; i < ProductDB.Count(); i++)
                {
                    if (item.IDProduct == ProductDB[i].ID)
                    {
                        ProductDB[i].InStock -= item.Amount; break;
                    }
                }
                _db.SaveChanges();
                _db.OrderItems.Add(orderItems);

            }
            _db.SaveChanges();
            lstCartItems = new List<Amount_Product>();
            HttpContext.Session.Set("ssShoppingCart", lstCartItems);

            return RedirectToAction("AppointmentConfirmation", "ShoppingCart", new { Id = orderId });

        }
      

        public IActionResult Remove(int id)
        {
            List<Amount_Product> lstCartItems = HttpContext.Session.Get<List<Amount_Product>>("ssShoppingCart");

            if (lstCartItems.Count > 0)
            {
                foreach(Amount_Product item in lstCartItems )
                {
                    if(item.IDProduct==id)
                    {
                        lstCartItems.Remove(item);
                        break;
                    }
                }
               
            }

            HttpContext.Session.Set("ssShoppingCart", lstCartItems);

            return RedirectToAction(nameof(Index));
        }



        //Get
        public IActionResult AppointmentConfirmation(int id)
        {
            Orders orders = _db.Orders.Where(a => a.ID == id).FirstOrDefault();
            orders.Customers = _db.Customers.Where(a => a.ID == orders.CustomerID).FirstOrDefault();
            
            List<OrderItems> orderItems = _db.OrderItems.Where(p => p.OrderID == id).ToList();
            List<Products> products = new List<Products>();        
            foreach (OrderItems prodAptObj in orderItems)
            {
                products.Add(_db.Products.Include(p => p.Merchants).Where(p => p.ID == prodAptObj.ProductID).FirstOrDefault());
            }

            OrderDetailViewModels orderItemsVM = new OrderDetailViewModels()
            {
                Order = orders,
                Products = products
            };

            //ShoppingCartVM.Orders = _db.Orders.Include(m => m.Customers).Where(a => a.ID == id).FirstOrDefault();
            //ShoppingCartVM.Orders.Customers = _db.Customers.Where(m => m.ID == ShoppingCartVM.Orders.CustomerID).FirstOrDefault();
            //List<OrderItems> objProdList = _db.OrderItems.Where(p => p.OrderID == id).ToList();

            //foreach (OrderItems prodAptObj in objProdList)
            //{
            //    ShoppingCartVM.Products.Add(_db.Products.Include(p => p.Merchants).Where(p => p.ID == prodAptObj.ProductID).FirstOrDefault());
            //}

            return View(orderItemsVM);
        }
    }
}