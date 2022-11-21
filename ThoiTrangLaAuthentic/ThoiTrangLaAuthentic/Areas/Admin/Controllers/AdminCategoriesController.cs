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
    public class AdminCategoriesController : Controller
    {
        private readonly ThoitrangLaAuthenticDbContext _context;
        public INotyfService _notyfService { get; }

        public AdminCategoriesController(ThoitrangLaAuthenticDbContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }

        // GET: Admin/AdminCategories
        public IActionResult Index(int? page)
        {
            var pageNumber = page == null || page < 0 ? 1 : page.Value;
            var pageSize = 20;
            var lsCategorys = _context.Categories
                .AsNoTracking()
                .OrderBy(x => x.CatId);
            PagedList<Categories> models = new PagedList<Categories>(lsCategorys, pageNumber, pageSize);
            ViewBag.CurrentPage = pageNumber;
            return View(models);
        }

        // GET: Admin/AdminCategories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categories = await _context.Categories
                .FirstOrDefaultAsync(m => m.CatId == id);
            if (categories == null)
            {
                return NotFound();
            }

            return View(categories);
        }

        // GET: Admin/AdminCategories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/AdminCategories/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CatId,CatName,Description,ParentId,Levels,Ordering,Published,Thumb,Title,Alias,MetaDesc,MetaKey,Cover,SchemaMarkup")] Categories categories, Microsoft.AspNetCore.Http.IFormFile fThumb)
        {
            if (ModelState.IsValid)
            {
                if (fThumb != null)
                {
                    string extension = Path.GetExtension(fThumb.FileName);
                    string imageName = Utilities.SEOUrl(categories.CatName) + extension;
                    categories.Thumb = await Utilities.UploadFile(fThumb, @"categories", imageName.ToLower());
                }
                if (string.IsNullOrEmpty(categories.Thumb)) categories.Thumb = "default.jpg";
                categories.Alias = Utilities.SEOUrl(categories.CatName);
                _context.Add(categories);
                await _context.SaveChangesAsync();
                _notyfService.Success("Thêm mới thành công");
                return RedirectToAction(nameof(Index));
            }
            return View(categories);
        }

        // GET: Admin/AdminCategories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categories = await _context.Categories.FindAsync(id);
            if (categories == null)
            {
                return NotFound();
            }
            return View(categories);
        }

        // POST: Admin/AdminCategories/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CatId,CatName,Description,ParentId,Levels,Ordering,Published,Thumb,Title,Alias,MetaDesc,MetaKey,Cover,SchemaMarkup")] Categories categories, Microsoft.AspNetCore.Http.IFormFile fThumb)
        {
            if (id != categories.CatId)
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
                        string imageName = Utilities.SEOUrl(categories.CatName) + extension;
                        categories.Thumb = await Utilities.UploadFile(fThumb, @"category", imageName.ToLower());
                    }
                    if (string.IsNullOrEmpty(categories.Thumb)) categories.Thumb = "default.jpg";
                    categories.Alias = Utilities.SEOUrl(categories.CatName);
                    _context.Update(categories);
                    await _context.SaveChangesAsync();
                    _notyfService.Success("Chỉnh sửa thành công");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoriesExists(categories.CatId))
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
            return View(categories);
        }

        // GET: Admin/AdminCategories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categories = await _context.Categories
                .FirstOrDefaultAsync(m => m.CatId == id);
            if (categories == null)
            {
                return NotFound();
            }

            return View(categories);
        }

        // POST: Admin/AdminCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var categories = await _context.Categories.FindAsync(id);
            _context.Categories.Remove(categories);
            await _context.SaveChangesAsync();
            _notyfService.Success("Xóa thành công");
            return RedirectToAction(nameof(Index));
        }

        private bool CategoriesExists(int id)
        {
            return _context.Categories.Any(e => e.CatId == id);
        }
    }
}
