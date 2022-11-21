using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PagedList.Core;
using ThoiTrangLaAuthentic.Extension;
using ThoiTrangLaAuthentic.Models;

namespace ThoiTrangLaAuthentic.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class AdminOrdersController : Controller
    {
        private readonly ThoitrangLaAuthenticDbContext _context;
        public INotyfService _notyfService { get; }

        public AdminOrdersController(ThoitrangLaAuthenticDbContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }

        // GET: Admin/AdminOrders
        public IActionResult Index(int? page)
        {
            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var pageSize = 20;
            var Orders = _context.Orders.Include(o => o.Customers).Include(o => o.TransactStatus)
                .AsNoTracking()
                .OrderBy(x => x.OrderDate);
            PagedList<Orders> models = new PagedList<Orders>(Orders, pageNumber, pageSize);
            ViewBag.CurrentPage = pageNumber;
            return View(models);
        }

        // GET: Admin/AdminOrders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.Customers)
                .Include(o => o.TransactStatus)
                .FirstOrDefaultAsync(m => m.OrderId == id);
            if (order == null)
            {
                return NotFound();
            }

            var Chitietdonhang = _context.OrderDetails
                .Include(x => x.Products)
                .AsNoTracking()
                .Where(x => x.OrderId == order.OrderId)
                .OrderBy(x => x.OrderDetailId)
                .ToList();
            ViewBag.ChiTiet = Chitietdonhang;
            return View(order);
        }


        public async Task<IActionResult> ChangeStatus(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .AsNoTracking()
                .Include(x => x.Customers)
                .FirstOrDefaultAsync(x => x.OrderId == id);
            if (order == null)
            {
                return NotFound();
            }
            ViewData["Trangthai"] = new SelectList(_context.TransactStatuses, "TransactStatusId", "Status", order.TransactStatusId);
            return PartialView("ChangeStatus", order);
        }
        [HttpPost]
        public async Task<IActionResult> ChangeStatus(int id, [Bind("OrderId,CustomerId,OrderDate,ShipDate,TransactStatusId,Deleted,Paid,PaymentDate,TotalMoney,PaymentId,Note,Address,LocationId,District,Ward")] Orders orders)
        {
            if (id != orders.OrderId)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var donhang = await _context.Orders.AsNoTracking().Include(x => x.Customers).FirstOrDefaultAsync(x => x.OrderId == id);
                    if (donhang != null)
                    {
                        donhang.Paid = orders.Paid;
                        donhang.Deleted = orders.Deleted;
                        donhang.TransactStatusId = orders.TransactStatusId;
                        if (donhang.Paid == true)
                        {
                            donhang.PaymentDate = DateTime.Now;
                        }
                        if (donhang.TransactStatusId == 5) donhang.Deleted = true;
                        if (donhang.TransactStatusId == 3) donhang.ShipDate = DateTime.Now;
                    }
                    _context.Update(donhang);
                    await _context.SaveChangesAsync();
                    _notyfService.Success("Cập nhật trạng thái đơn hàng thành công");

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(orders.OrderId))
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
            ViewData["Trangthai"] = new SelectList(_context.TransactStatuses, "TransactStatusId", "Status", orders.TransactStatusId);
            return PartialView("ChangeStatus", orders);
        }

        // GET: Admin/AdminOrders/Create
        public IActionResult Create()
        {
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "CustomerId");
            ViewData["TransactStatusId"] = new SelectList(_context.TransactStatuses, "TransactStatusId", "TransactStatusId");
            return View();
        }

        // POST: Admin/AdminOrders/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrderId,CustomerId,OrderDate,ShipDate,TransactStatusId,Deleted,Paid,PaymentDate,TotalMoney,PaymentId,Note,Address,LocationId,District,Ward")] Orders order)
        {
            if (ModelState.IsValid)
            {
                _context.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "CustomerId", order.CustomerId);
            ViewData["TransactStatusId"] = new SelectList(_context.TransactStatuses, "TransactStatusId", "TransactStatusId", order.TransactStatusId);
            return View(order);
        }

        // GET: Admin/AdminOrders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orders = await _context.Orders.FindAsync(id);
            if (orders == null)
            {
                return NotFound();
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "CustomerId", orders.CustomerId);
            ViewData["TransactStatusId"] = new SelectList(_context.TransactStatuses, "TransactStatusId", "TransactStatusId", orders.TransactStatusId);
            return View(orders);
        }

        // POST: Admin/AdminOrders/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OrderId,CustomerId,OrderDate,ShipDate,TransactStatusId,Deleted,Paid,PaymentDate,TotalMoney,PaymentId,Note,Address,LocationId,District,Ward")] Orders orders)
        {
            if (id != orders.OrderId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(orders);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(orders.OrderId))
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
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "CustomerId", orders.CustomerId);
            ViewData["TransactStatusId"] = new SelectList(_context.TransactStatuses, "TransactStatusId", "TransactStatusId", orders.TransactStatusId);
            return View(orders);
        }

        // GET: Admin/AdminOrders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var order = await _context.Orders
                .Include(o => o.Customers)
                .Include(o => o.TransactStatus)
                .FirstOrDefaultAsync(m => m.OrderId == id);
            if (order == null)
            {
                return NotFound();
            }

            var Chitietdonhang = _context.OrderDetails
                .Include(x => x.Products)
                .AsNoTracking()
                .Where(x => x.OrderId == order.OrderId)
                .OrderBy(x => x.OrderDetailId)
                .ToList();
            ViewBag.ChiTiet = Chitietdonhang;

            return View(order);
        }

        // POST: Admin/AdminOrders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            order.Deleted = true;
            _context.Update(order);
            await _context.SaveChangesAsync();
            _notyfService.Success("Xóa đơn hàng thành công");
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.OrderId == id);
        }
    }
}
