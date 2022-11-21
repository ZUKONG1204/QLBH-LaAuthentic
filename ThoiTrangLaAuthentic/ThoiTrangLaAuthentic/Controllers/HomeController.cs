using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ThoiTrangLaAuthentic.Models;
using ThoiTrangLaAuthentic.ModelViews;

namespace ThoiTrangLaAuthentic.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ThoitrangLaAuthenticDbContext _context;

        public HomeController(ILogger<HomeController> logger, ThoitrangLaAuthenticDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            HomeView model = new HomeView();
            var lsProducts = _context.Products.AsNoTracking()
                .Where(x => x.Active == true && x.HomeFlag == true)
                .OrderByDescending(x => x.DateCreated)
                .ToList();

            List<ProductViewHome> lsProductViews = new List<ProductViewHome>();
            var lsCats = _context.Categories
                .AsNoTracking()
                .Where(x => x.Published == true)
                .OrderByDescending(x => x.Ordering)
                .ToList();

            foreach (var item in lsCats)
            {
                ProductViewHome productHome = new ProductViewHome();
                productHome.category = item;
                productHome.lsProducts = lsProducts.Where(x => x.CatId == item.CatId).ToList();
                lsProductViews.Add(productHome);

                
                model.Products = lsProductViews;
                ViewBag.AllProducts = lsProducts;
            }
            return View(model);
        }



        [Route("lien-he.html", Name = "Contact")]
        public IActionResult Contact()
        {
            return View();
        }
        [Route("gioi-thieu.html", Name = "About")]
        public IActionResult About()
        {
            return View();
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
