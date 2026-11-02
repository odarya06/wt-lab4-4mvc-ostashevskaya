using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using wt_lab4_4mvc_ostashevskaya.Data;
using wt_lab4_4mvc_ostashevskaya.Models;

namespace wt_lab4_4mvc_ostashevskaya.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        var products = ProductRepository.GetAll();

        var model = new HomeViewModel
        {
            TotalProducts = products.Count,
            TotalUnits = products.Sum(product => product.Quantity),
            TotalValue = products.Sum(product => product.TotalValue),
            CategoryCount = products.Select(product => product.Category).Distinct().Count(),
            LowStockCount = products.Count(product => product.Quantity is > 0 and <= 10),
            RecentProducts = products
                .OrderByDescending(product => product.ReceivedAt)
                .Take(3)
                .ToList()
        };

        return View(model);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult Status(int code)
    {
        ViewData["StatusCode"] = code;
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
