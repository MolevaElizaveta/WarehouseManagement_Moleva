using System.Collections.Generic;
using WarehouseManagement_Moleva.Data.Models;

namespace WarehouseManagement_Moleva.Data.Services
{
    /// Генератор тестовых данных (хранилище — коллекции вместо БД)
    public static class DataGenerator
    {
        public static List<Organization> GenerateOrganizations()
        {
            var names = new[]
            {
                "ООО КамаПродукт",
                "АО ПермКнига",
                "ИП Соколова А.В.",
                "ООО ДомУют",
                "ЗАО УралТекстиль",
            };

            var result = new List<Organization>();
            for (int i = 0; i < names.Length; i++)
            {
                result.Add(new Organization(names[i]) { Id = i + 1 });
            }
            return result;
        }

        public static List<Category> GenerateCategories()
        {
            var names = new[]
            {
                "Канцелярия",
                "Книги",
                "Товары для дома",
                "Текстиль",
                "Хозяйственные товары",
            };

            var result = new List<Category>();
            for (int i = 0; i < names.Length; i++)
            {
                result.Add(new Category(names[i]) { Id = i + 1 });
            }
            return result;
        }

        public static List<Manufacturer> GenerateManufacturers()
        {
            var names = new[]
            {
                "Erich Krause",
                "Brauberg",
                "Parker",
                "Tefal",
                "Luminarc",
                "Rondell",
                "Vileda",
                "Ивановский текстиль",
            };

            var result = new List<Manufacturer>();
            for (int i = 0; i < names.Length; i++)
            {
                result.Add(new Manufacturer(names[i]) { Id = i + 1 });
            }
            return result;
        }

        public static List<Supplier> GenerateSuppliers()
        {
            var names = new[]
            {
                "ООО КанцОптТорг",
                "КамаСнаб",
                "ПермьБумага",
                "ДомОпт",
                "ТекстильПартнёр",
                "ХозТрейд",
            };

            var result = new List<Supplier>();
            for (int i = 0; i < names.Length; i++)
            {
                result.Add(new Supplier(names[i]) { Id = i + 1 });
            }
            return result;
        }

        public static List<Warehouse> GenerateWarehouses(List<Organization> organizations)
        {
            // Склады привязываются к организации по её порядковому номеру в справочнике
            var data = new (int OrgIndex, string Name, string Address)[]
            {
                (0, "Склад Кама-1", "г. Пермь, ул. Мира, 41"),
                (0, "Склад Кама-2", "г. Пермь, ул. Плеханова, 59"),
                (1, "Книжный склад", "г. Екатеринбург, ул. Бебеля, 17"),
                (2, "Склад на Парковом", "г. Пермь, ул. Хлебозаводская, 30"),
                (3, "Склад Уют", "г. Казань, ул. Тихорецкая, 12"),
                (3, "Склад возвратов", "г. Казань, ул. Мамадышский тракт, 8"),
                (4, "Текстиль-центр", "г. Ижевск, ул. Удмуртская, 205"),
            };

            var warehouses = new List<Warehouse>();
            int whId = 1;
            foreach (var item in data)
            {
                if (item.OrgIndex >= organizations.Count) continue;
                var org = organizations[item.OrgIndex];
                warehouses.Add(new Warehouse(item.Name, item.Address, org.Id) { Id = whId++ });
            }
            return warehouses;
        }

        public static List<Product> GenerateProducts(List<Category> categories,
                                                       List<Manufacturer> manufacturers,
                                                       List<Supplier> suppliers)
        {
            var data = new (string Name, decimal Price, decimal Discount, int Stock, string Unit, string Category, string Manufacturer, string Supplier)[]
            {
                ("Ручки гелевые Erich Krause, 12 шт", 490, 0, 150, "наб", "Канцелярия", "Erich Krause", "ООО КанцОптТорг"),
                ("Тетрадь Brauberg 96 л., клетка", 120, 0, 400, "шт", "Канцелярия", "Brauberg", "ПермьБумага"),
                ("Ручка перьевая Parker Jotter", 3990, 10, 25, "шт", "Канцелярия", "Parker", "КамаСнаб"),
                ("Бумага офисная Brauberg А4, 500 л.", 450, 5, 220, "уп", "Канцелярия", "Brauberg", "ПермьБумага"),
                ("Сковорода Tefal Easy Cook 26 см", 3290, 8, 30, "шт", "Товары для дома", "Tefal", "ДомОпт"),
                ("Набор тарелок Luminarc, 18 предметов", 2790, 0, 24, "наб", "Товары для дома", "Luminarc", "ДомОпт"),
                ("Кастрюля Rondell Flamme 4,5 л", 4190, 12, 18, "шт", "Товары для дома", "Rondell", "ДомОпт"),
                ("Комплект постельного белья 1,5-спальный", 2490, 0, 55, "компл", "Текстиль", "Ивановский текстиль", "ТекстильПартнёр"),
                ("Полотенце махровое 70×140", 790, 0, 140, "шт", "Текстиль", "Ивановский текстиль", "ТекстильПартнёр"),
                ("Плед флисовый 150×200", 1390, 15, 48, "шт", "Текстиль", "Ивановский текстиль", "ТекстильПартнёр"),
                ("Швабра Vileda Ultramax с отжимом", 2190, 5, 36, "шт", "Хозяйственные товары", "Vileda", "ХозТрейд"),
                ("Салфетки микрофибра Vileda, 5 шт", 350, 0, 260, "уп", "Хозяйственные товары", "Vileda", "ХозТрейд"),
            };

            var products = new List<Product>();
            int article = 1;
            foreach (var item in data)
            {
                var category = categories.Find(c => c.Name == item.Category) ?? categories[0];
                var manufacturer = manufacturers.Find(m => m.Name == item.Manufacturer) ?? manufacturers[0];
                var supplier = suppliers.Find(s => s.Name == item.Supplier) ?? suppliers[0];

                products.Add(new Product(item.Name, item.Price)
                {
                    Article = (article++).ToString("D6"),
                    Unit = item.Unit,
                    DiscountPercent = item.Discount,
                    StockQuantity = item.Stock,
                    CategoryId = category.Id,
                    ManufacturerId = manufacturer.Id,
                    SupplierId = supplier.Id,
                    Category = category,
                    Manufacturer = manufacturer,
                    Supplier = supplier,
                    SupplierName = supplier.Name
                });
            }
            return products;
        }
    }
}
