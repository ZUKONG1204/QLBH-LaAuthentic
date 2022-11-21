using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PagedList.Core;
using ThoiTrangLaAuthentic.Helpper;
using ThoiTrangLaAuthentic.Models;

namespace ThoiTrangLaAuthentic.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class AdminPagesController : Controller
    {
        private readonly ThoitrangLaAuthenticDbContext _context;
        public INotyfService _notyfService { get; }
        public AdminPagesController(ThoitrangLaAuthenticDbContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }

        // GET: Admin/AdminPages
        public IActionResult Index(int? page)
        {
            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var pageSize = 20;
            var lsPages = _context.Pages
                .AsNoTracking()
                .OrderBy(x => x.PageId);
            PagedList<Pages> models = new PagedList<Pages>(lsPages, pageNumber, pageSize);

            ViewBag.CurrentPage = pageNumber;
            return View(models);
        }

        // GET: Admin/AdminPages/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pages = await _context.Pages
                .FirstOrDefaultAsync(m => m.PageId == id);
            if (pages == null)
            {
                return NotFound();
            }

            return View(pages);
        }

        // GET: Admin/AdminPages/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/AdminPages/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PageId,PageName,Contents,Thumb,Published,Title,MetaDesc,MetaKey,Alias,CreatedDate,Ordering")] Pages pages, Microsoft.AspNetCore.Http.IFormFile fThumb)
        {
            if (ModelState.IsValid)
            {

                if (fThumb != null)
                {
                    string extension = Path.GetExtension(fThumb.FileName);
                    string imageName = Utilities.SEOUrl(pages.PageName) + extension;
                    pages.Thumb = await Utilities.UploadFile(fThumb, @"pages", imageName.ToLower());
                }
                if (string.IsNullOrEmpty(pages.Thumb)) pages.Thumb = "default.jpg";
                pages.Alias = Utilities.SEOUrl(pages.PageName);
                _context.Add(pages);
                await _context.SaveChangesAsync();
                _notyfService.Success("Thêm mới thành công");
                return RedirectToAction(nameof(Index));
            }
            return View(pages);
        }

        // GET: Admin/AdminPages/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pages = await _context.Pages.FindAsync(id);
            if (pages == null)
            {
                return NotFound();
            }
            return View(pages);
        }

        // POST: Admin/AdminPages/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PageId,PageName,Contents,Thumb,Published,Title,MetaDesc,MetaKey,Alias,CreatedDate,Ordering")] Pages pages, Microsoft.AspNetCore.Http.IFormFile fThumb)
        {
            if (id != pages.PageId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (fThumb != null)
                    {
                        string extension = Path.GetExtension(fThumb.FileName);
                        string imageName = Utilities.SEOUrl(pages.PageName) + extension;
                        pages.Thumb = await Utilities.UploadFile(fThumb, @"pages", imageName.ToLower());
                    }
                    if (string.IsNullOrEmpty(pages.Thumb)) pages.Thumb = "default.jpg";
                    pages.Alias = Utilities.SEOUrl(pages.PageName);
                    _context.Update(pages);
                    await _context.SaveChangesAsync();
                    _notyfService.Success("Cập nhật thành công");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PageExists(pages.PageId))
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
            return View(pages);
        }

        // GET: Admin/AdminPages/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pages = await _context.Pages
                .FirstOrDefaultAsync(m => m.PageId == id);
            if (pages == null)
            {
                return NotFound();
            }

            return View(pages);
        }

        // POST: Admin/AdminPages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pages = await _context.Pages.FindAsync(id);
            _context.Pages.Remove(pages);
            await _context.SaveChangesAsync();
            _notyfService.Success("Xóa thành công");
            return RedirectToAction(nameof(Index));
        }

        private bool PageExists(int id)
        {
            return _context.Pages.Any(e => e.PageId == id);
        }
    }
}
