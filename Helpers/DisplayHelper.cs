using wt_lab4_4mvc_ostashevskaya.Models;

namespace wt_lab4_4mvc_ostashevskaya.Helpers;

/// <summary>Небольшие помощники для оформления представлений.</summary>
public static class DisplayHelper
{
    public static string CategoryIcon(string category) => category switch
    {
        "Компьютеры" => "💻",
        "Аксессуары" => "🖱️",
        "Мебель" => "🪑",
        "Расходники" => "📄",
        _ => "📦"
    };

    public static string StatusBadgeClass(Product product) => product.Quantity == 0
        ? "bg-danger"
        : !product.IsAvailable
            ? "bg-secondary"
            : product.Quantity <= 10
                ? "bg-warning text-dark"
                : "bg-success";
}
