using Microsoft.AspNetCore.Mvc;
using wt_lab4_4mvc_ostashevskaya.Data;
using wt_lab4_4mvc_ostashevskaya.Models;

namespace wt_lab4_4mvc_ostashevskaya.Controllers;

/// <summary>
/// Контроллер каталога складских позиций.
/// Маршрутизация: /Product/Index, /Product/Details/1, /Product/Create, /Product/Edit/1.
/// </summary>
public class ProductController : Controller
{
    /// <summary>GET /Product/Index — каталог с поиском и фильтрами.</summary>
    public IActionResult Index(string? search, string? category, string? status)
    {
        var products = ProductRepository.GetAll().AsEnumerable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var query = search.Trim();
            products = products.Where(product =>
                product.Name.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                product.Sku.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                product.Supplier.Contains(query, StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrWhiteSpace(category))
        {
            products = products.Where(product => product.Category == category);
        }

        products = status switch
        {
            "in-stock" => products.Where(product => product.Quantity > 0 && product.IsAvailable),
            "low-stock" => products.Where(product => product.Quantity is > 0 and <= 10),
            "out-of-stock" => products.Where(product => product.Quantity == 0),
            _ => products
        };

        ViewBag.Search = search;
        ViewBag.SelectedCategory = category;
        ViewBag.SelectedStatus = status;
        ViewBag.Categories = ProductRepository.GetCategories();
        ViewBag.TotalCount = ProductRepository.GetAll().Count;

        return View(products.ToList());
    }

    /// <summary>GET /Product/Details/1 — подробная карточка. Если позиции нет — 404.</summary>
    public IActionResult Details(int id)
    {
        var product = ProductRepository.GetById(id);
        if (product is null)
        {
            return NotFound();
        }

        return View(product);
    }

    /// <summary>GET /Product/Create — форма добавления.</summary>
    public IActionResult Create()
    {
        FillCategories();
        return View(new Product());
    }

    /// <summary>POST /Product/Create — сохранение новой позиции.</summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Product product)
    {
        if (!ModelState.IsValid)
        {
            FillCategories();
            return View(product);
        }

        ProductRepository.Add(product);
        TempData["Success"] = $"Позиция «{product.Name}» добавлена на склад.";
        return RedirectToAction(nameof(Index));
    }

    /// <summary>GET /Product/Edit/1 — форма редактирования.</summary>
    public IActionResult Edit(int id)
    {
        var product = ProductRepository.GetById(id);
        if (product is null)
        {
            return NotFound();
        }

        FillCategories();
        return View(product);
    }

    /// <summary>POST /Product/Edit/1 — сохранение изменений.</summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, Product product)
    {
        if (id != product.Id)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            FillCategories();
            return View(product);
        }

        if (!ProductRepository.Update(product))
        {
            return NotFound();
        }

        TempData["Success"] = $"Изменения по позиции «{product.Name}» сохранены.";
        return RedirectToAction(nameof(Index));
    }

    /// <summary>POST /Product/Delete/1 — удаление позиции.</summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Delete(int id)
    {
        var product = ProductRepository.GetById(id);
        if (product is null)
        {
            return NotFound();
        }

        ProductRepository.Delete(id);
        TempData["Success"] = $"Позиция «{product.Name}» удалена со склада.";
        return RedirectToAction(nameof(Index));
    }

    private void FillCategories()
    {
        ViewBag.Categories = ProductRepository.GetCategories();
    }
}
