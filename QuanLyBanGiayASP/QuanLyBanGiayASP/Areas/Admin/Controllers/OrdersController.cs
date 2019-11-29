using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize(Roles = SD.AdminEndUser + "," + SD.SuperAdminEndUser)]
    [Area("Admin")]

    public class OrdersController : Controller
    {
        
        private readonly ApplicationDbContext _db;
        private int PageSize = 3;

        public OrdersController(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<IActionResult> Index(int productPage = 1,string searchName = null, string searchEmail = null, string searchPhone = null, string searchDate = null)
        {
            System.Security.Claims.ClaimsPrincipal currentUser = this.User;
            var claimsIdentity = (ClaimsIdentity)this.User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            OrderViewModels orderVM = new OrderViewModels()
            {
                Orders = new List<Models.Orders>()
            };

          

            StringBuilder param = new StringBuilder();
            param.Append("/Admin/Orders?productPage=:");
            param.Append("&searchName=");

            if (searchName != null)
            {
                param.Append(searchName);
            }
            param.Append("&searchEmail=");
            if (searchEmail != null)
            {
                param.Append(searchEmail);
            }
            param.Append("&searchPhone=");
            if (searchPhone != null)
            {
                param.Append(searchPhone);
            }
            param.Append("&searchDate=");
            if (searchDate != null)
            {
                param.Append(searchDate);
            }


            orderVM.Orders = _db.Orders.Include(a => a.Customers).Include(a => a.SalerPerson).ToList();

            if (User.IsInRole(SD.AdminEndUser))
            {
                orderVM.Orders = orderVM.Orders.Where(a => a.SalesPersonId == claim.Value).ToList();
            }

            if (searchName != null)
            {

                orderVM.Orders = orderVM.Orders.Where(a => a.Customers.CustomerName.ToLower().Contains(searchName.ToLower())).ToList();

            }
            if (searchEmail != null)
            {
                orderVM.Orders = orderVM.Orders.Where(a => a.Customers.Email.ToLower().Contains(searchEmail.ToLower())).ToList();
            }
            if (searchPhone != null)
            {
                orderVM.Orders = orderVM.Orders.Where(a => a.Customers.Phone.ToLower().Contains(searchPhone.ToLower())).ToList();
            }
            if (searchDate != null)
            {
                try
                {
                    DateTime appDate = Convert.ToDateTime(searchDate);
                    orderVM.Orders = orderVM.Orders.Where(a => a.Date.ToShortDateString().Equals(appDate.ToShortDateString())).ToList();
                }
                catch (Exception ex)
                {

                }

            }


            var count = orderVM.Orders.Count;

            orderVM.Orders = orderVM.Orders.OrderBy(p => p.Date)
                .Skip((productPage - 1) * PageSize)
                .Take(PageSize).ToList();


            orderVM.PagingInfo = new PagingInfo
            {
                CurrentPage = productPage,
                ItemsPerPage = PageSize,
                TotalItems = count,
                urlParam = param.ToString()
            };


            return View(orderVM);
        }

        //GET Edit
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productList = (IEnumerable<Products>)(from p in _db.Products
                                                      join a in _db.OrderItems
                                                      on p.ID equals a.ProductID
                                                      where a.OrderID == id
                                                      select p).Include("Merchants").Include("Brands");

            OrderDetailViewModels objOrderVM = new OrderDetailViewModels()
            {
                Order = _db.Orders.Include(a=>a.Customers).Include(a=>a.OrderItems).Include(a => a.SalerPerson)
                      .Where(a => a.ID == id).FirstOrDefault(),
                SalesPerson = _db.ApplicationUser.ToList(),
                Products = productList.ToList()
            };

            return View(objOrderVM);

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, OrderDetailViewModels objOrderVM)
        {
            if (ModelState.IsValid)
            {
                objOrderVM.Order.Date = objOrderVM.Order.Date
                                    .AddHours(objOrderVM.Order.Time.Hour)
                                    .AddMinutes(objOrderVM.Order.Time.Minute);

                var orderFromDb = _db.Orders.Where(a => a.ID == objOrderVM.Order.ID).FirstOrDefault();

                orderFromDb.Customers = objOrderVM.Order.Customers;             
                orderFromDb.Date = objOrderVM.Order.Date;
                orderFromDb.isConfirmed = objOrderVM.Order.isConfirmed;
                if (User.IsInRole(SD.SuperAdminEndUser))
                {
                    orderFromDb.SalesPersonId = objOrderVM.Order.SalesPersonId;
                }
                _db.SaveChanges();

                return RedirectToAction(nameof(Index));


            }

            return View(objOrderVM);
        }

        //GET Detials
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productList = (IEnumerable<Products>)(from p in _db.Products
                                                      join a in _db.OrderItems
                                                      on p.ID equals a.ProductID
                                                      where a.OrderID == id
                                                      select p).Include("Merchants").Include("Brands"); 

            OrderDetailViewModels objOrderVM = new OrderDetailViewModels()
            {
                Order = _db.Orders.Include(a => a.Customers).Include(a => a.OrderItems).Include(a => a.SalerPerson)
                      .Where(a => a.ID == id).FirstOrDefault(),
                SalesPerson = _db.ApplicationUser.ToList(),
                Products = productList.ToList()
            };

            return View(objOrderVM);

        }

        //GET Delete
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productList = (IEnumerable<Products>)(from p in _db.Products
                                                      join a in _db.OrderItems
                                                      on p.ID equals a.ProductID
                                                      where a.OrderID == id
                                                      select p).Include("Merchants").Include("Brands");

            OrderDetailViewModels objOrderVM = new OrderDetailViewModels()
            {
                Order = _db.Orders.Include(a=>a.Customers).Include(a => a.SalerPerson).Include(a=>a.OrderItems).Where(a => a.ID == id).FirstOrDefault(),
                SalesPerson = _db.ApplicationUser.ToList(),
                Products = productList.ToList()
            };

            return View(objOrderVM);

        }


        //POST Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _db.Orders.FindAsync(id);
            var customer = await _db.Customers.FindAsync(order.CustomerID);

            var tableCTOrder = _db.OrderItems.ToList();

            var tableProduct = _db.Products.ToList();

            foreach (var item in tableCTOrder)
            {
                if (item.OrderID == order.ID)
                {
                    var ctOrder = await _db.OrderItems.FindAsync(item.ID);
                    //Cập nhật lại số product trong kho nếu giao dịch không thành công
                    foreach (var product in tableProduct)
                    {
                        if (product.ID == ctOrder.ProductID)
                        {
                            int update = product.InStock + ctOrder.Amount;
                            var productDB = await _db.Products.FindAsync(ctOrder.ProductID);
                            productDB.InStock = update;
                            _db.Products.Update(productDB);
                        }
                    }

                    _db.OrderItems.Remove(ctOrder);
                }
            }

            _db.Customers.Remove(customer);
            _db.Orders.Remove(order);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}