using wt_lab4_4mvc_ostashevskaya.Models;

namespace wt_lab4_4mvc_ostashevskaya.Data;

/// <summary>
/// Простое хранилище данных в памяти приложения.
/// По условию лабораторной работы база данных не используется:
/// после перезапуска приложения изменения пользователя сбрасываются.
/// </summary>
public static class ProductRepository
{
    private static readonly object SyncRoot = new();
    private static int _nextId = 7;

    private static readonly List<Product> Products = new()
    {
        new Product
        {
            Id = 1,
            Name = "Ноутбук Lenovo ThinkPad E14",
            Sku = "SKU-1001",
            Category = "Компьютеры",
            Supplier = "ООО «ТехноСнаб»",
            Quantity = 12,
            Price = 89_990m,
            Description = "Корпоративный ноутбук для офисных задач: 16 ГБ ОЗУ, SSD 512 ГБ, экран 14″.",
            IsAvailable = true,
            ReceivedAt = DateTime.Today.AddDays(-3)
        },
        new Product
        {
            Id = 2,
            Name = "Беспроводная мышь Logitech M185",
            Sku = "SKU-1002",
            Category = "Аксессуары",
            Supplier = "ООО «ТехноСнаб»",
            Quantity = 48,
            Price = 2_490m,
            Description = "Компактная беспроводная мышь с USB-приёмником и временем работы до 12 месяцев.",
            IsAvailable = true,
            ReceivedAt = DateTime.Today.AddDays(-5)
        },
        new Product
        {
            Id = 3,
            Name = "Офисное кресло ErgoLine Comfort",
            Sku = "SKU-2001",
            Category = "Мебель",
            Supplier = "Мебельная фабрика «Комфорт»",
            Quantity = 7,
            Price = 15_900m,
            Description = "Эргономичное кресло с регулировкой высоты, поясничной поддержкой и сетчатой спинкой.",
            IsAvailable = true,
            ReceivedAt = DateTime.Today.AddDays(-10)
        },
        new Product
        {
            Id = 4,
            Name = "Монитор Dell UltraSharp 27″",
            Sku = "SKU-1003",
            Category = "Компьютеры",
            Supplier = "ООО «ТехноСнаб»",
            Quantity = 0,
            Price = 32_990m,
            Description = "27-дюймовый IPS-монитор с разрешением 2560×1440 и поддержкой USB-C.",
            IsAvailable = false,
            ReceivedAt = DateTime.Today.AddDays(-14)
        },
        new Product
        {
            Id = 5,
            Name = "Бумага офисная А4, 500 листов",
            Sku = "SKU-3001",
            Category = "Расходники",
            Supplier = "ООО «Офис-Маркет»",
            Quantity = 120,
            Price = 390m,
            Description = "Белая офисная бумага плотностью 80 г/м². Упаковка 500 листов.",
            IsAvailable = true,
            ReceivedAt = DateTime.Today.AddDays(-1)
        },
        new Product
        {
            Id = 6,
            Name = "Стеллаж металлический 4 полки",
            Sku = "SKU-2002",
            Category = "Мебель",
            Supplier = "Мебельная фабрика «Комфорт»",
            Quantity = 15,
            Price = 7_490m,
            Description = "Разборный металлический стеллаж для складских помещений, нагрузка до 150 кг на полку.",
            IsAvailable = true,
            ReceivedAt = DateTime.Today.AddDays(-20)
        }
    };

    public static IReadOnlyList<Product> GetAll()
    {
        lock (SyncRoot)
        {
            return Products.OrderBy(product => product.Id).ToList();
        }
    }

    public static Product? GetById(int id)
    {
        lock (SyncRoot)
        {
            return Products.FirstOrDefault(product => product.Id == id);
        }
    }

    public static IReadOnlyList<string> GetCategories()
    {
        lock (SyncRoot)
        {
            return Products
                .Select(product => product.Category)
                .Distinct()
                .OrderBy(category => category)
                .ToList();
        }
    }

    public static void Add(Product product)
    {
        lock (SyncRoot)
        {
            product.Id = _nextId++;
            Products.Add(product);
        }
    }

    public static bool Update(Product product)
    {
        lock (SyncRoot)
        {
            var existing = Products.FirstOrDefault(item => item.Id == product.Id);
            if (existing is null)
            {
                return false;
            }

            existing.Name = product.Name;
            existing.Sku = product.Sku;
            existing.Category = product.Category;
            existing.Supplier = product.Supplier;
            existing.Quantity = product.Quantity;
            existing.Price = product.Price;
            existing.Description = product.Description;
            existing.IsAvailable = product.IsAvailable;
            existing.ReceivedAt = product.ReceivedAt;
            return true;
        }
    }

    public static bool Delete(int id)
    {
        lock (SyncRoot)
        {
            var product = Products.FirstOrDefault(item => item.Id == id);
            return product is not null && Products.Remove(product);
        }
    }
}
