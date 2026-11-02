namespace wt_lab4_4mvc_ostashevskaya.Models;

/// <summary>
/// Данные для главной страницы: сводка по складу и последние поступления.
/// </summary>
public class HomeViewModel
{
    public int TotalProducts { get; set; }
    public int TotalUnits { get; set; }
    public decimal TotalValue { get; set; }
    public int CategoryCount { get; set; }
    public int LowStockCount { get; set; }
    public IReadOnlyList<Product> RecentProducts { get; set; } = Array.Empty<Product>();
}
