using System.ComponentModel.DataAnnotations;

namespace wt_lab4_4mvc_ostashevskaya.Models;

/// <summary>
/// Товар на складе — модель предметной области «Складской учёт».
/// </summary>
public class Product
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Укажите наименование товара")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Наименование должно содержать от 2 до 100 символов")]
    [Display(Name = "Наименование")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Укажите артикул")]
    [StringLength(20, MinimumLength = 2, ErrorMessage = "Артикул должен содержать от 2 до 20 символов")]
    [Display(Name = "Артикул")]
    public string Sku { get; set; } = string.Empty;

    [Required(ErrorMessage = "Выберите категорию")]
    [StringLength(50, ErrorMessage = "Категория не должна превышать 50 символов")]
    [Display(Name = "Категория")]
    public string Category { get; set; } = string.Empty;

    [StringLength(100, ErrorMessage = "Название поставщика не должно превышать 100 символов")]
    [Display(Name = "Поставщик")]
    public string Supplier { get; set; } = string.Empty;

    [Range(0, 1_000_000, ErrorMessage = "Количество должно быть от 0 до 1 000 000")]
    [Display(Name = "Количество на складе")]
    public int Quantity { get; set; }

    [Range(0.01, 10_000_000, ErrorMessage = "Цена должна быть больше нуля")]
    [Display(Name = "Цена, ₽")]
    public decimal Price { get; set; }

    [StringLength(500, ErrorMessage = "Описание не должно превышать 500 символов")]
    [Display(Name = "Описание")]
    public string Description { get; set; } = string.Empty;

    [Display(Name = "Доступен к отгрузке")]
    public bool IsAvailable { get; set; } = true;

    [DataType(DataType.Date)]
    [Display(Name = "Дата поступления")]
    public DateTime ReceivedAt { get; set; } = DateTime.Today;

    /// <summary>Стоимость остатка: цена × количество.</summary>
    [Display(Name = "Стоимость остатка")]
    public decimal TotalValue => Price * Quantity;

    /// <summary>Текстовый статус позиции для badge в представлениях.</summary>
    [Display(Name = "Статус")]
    public string StockStatus => Quantity == 0
        ? "Нет в наличии"
        : IsAvailable ? "В наличии" : "Недоступен";
}
