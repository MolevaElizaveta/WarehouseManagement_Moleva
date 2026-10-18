using System.Collections.Generic;
using System.Linq;
using WarehouseManagement_Moleva.Data.Models;

namespace WarehouseManagement_Moleva.Data.Services
{
    /// Сервис для работы с товарами
    public class ProductService
    {
        private List<Product> _products;
        private List<Category> _categories;
        private List<Manufacturer> _manufacturers;
        private List<Supplier> _suppliers;
        private WarehouseService _warehouseService;

        public ProductService(WarehouseService warehouseService)
        {
            _warehouseService = warehouseService;
            _categories = DataGenerator.GenerateCategories();
            _manufacturers = DataGenerator.GenerateManufacturers();
            _suppliers = DataGenerator.GenerateSuppliers();
            _products = DataGenerator.GenerateProducts(_categories, _manufacturers, _suppliers);
        }

        /// Получить все товары
        public List<Product> GetAllProducts()
        {
            return _products.ToList();
        }

        /// Получить товары на конкретном складе
        public List<Product> GetProductsByWarehouse(long warehouseId)
        {
            return _products.Where(p => p.StockQuantity > 0).ToList();
        }

        /// Получить товар по артикулу
        public Product? GetByArticle(string article)
        {
            return _products.FirstOrDefault(p => p.Article == article);
        }

        /// Добавить новый товар
        public void Add(Product product)
        {
            if (string.IsNullOrWhiteSpace(product.Article))
            {
                // Новый товар получает следующий свободный артикул
                int max = _products
                    .Select(p => int.TryParse(p.Article, out var n) ? n : 0)
                    .DefaultIfEmpty(0)
                    .Max();
                product.Article = (max + 1).ToString("D6");
            }
            _products.Add(product);
        }

        /// Обновить товар
        public void Update(Product product)
        {
            var index = _products.FindIndex(p => p.Article == product.Article);
            if (index != -1)
                _products[index] = product;
        }

        /// Удалить товар
        public void Delete(string article)
        {
            var product = GetByArticle(article);
            if (product != null)
                _products.Remove(product);
        }

        /// Получить все категории
        public List<Category> GetAllCategories()
        {
            return _categories.ToList();
        }

        /// Получить всех производителей
        public List<Manufacturer> GetAllManufacturers()
        {
            return _manufacturers.ToList();
        }

        /// Получить всех поставщиков
        public List<Supplier> GetAllSuppliers()
        {
            return _suppliers.ToList();
        }

        /// Обновить остаток товара на складе
        public void UpdateStock(string article, int quantityChange)
        {
            var product = GetByArticle(article);
            if (product != null)
            {
                product.StockQuantity += quantityChange;
            }
        }
    }
}