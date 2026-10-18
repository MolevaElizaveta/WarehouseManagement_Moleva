using NUnit.Framework;
using WarehouseManagement_Moleva.Data.Models;
using WarehouseManagement_Moleva.Data.Services;
using System.Linq;

namespace WarehouseManagement_Moleva.Tests.ServicesTests
{
    [TestFixture]
    public class ProductServiceTests
    {
        private ProductService _service;
        private WarehouseService _warehouseService;
        private OrganizationService _orgService;

        [SetUp]
        public void Setup()
        {
            _orgService = new OrganizationService();
            _warehouseService = new WarehouseService(_orgService);
            _service = new ProductService(_warehouseService);
        }

        [Test]
        public void GetAllProducts_ShouldReturnProducts()
        {
            var result = _service.GetAllProducts();
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.GreaterThan(0));
        }

        [Test]
        public void Add_ShouldIncreaseProductCount()
        {
            var initialCount = _service.GetAllProducts().Count;
            var newProduct = new Product("Тестовый товар", 1000);
            _service.Add(newProduct);
            var newCount = _service.GetAllProducts().Count;
            Assert.That(newCount, Is.EqualTo(initialCount + 1));
        }

        [Test]
        public void GetByArticle_ShouldReturnCorrectProduct()
        {
            var product = _service.GetAllProducts().First();
            var result = _service.GetByArticle(product.Article);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Article, Is.EqualTo(product.Article));
        }

        [Test]
        public void Update_ShouldChangeProductName()
        {
            var product = _service.GetAllProducts().First();
            var newName = "Обновленный товар";
            product.Name = newName;
            _service.Update(product);
            var updatedProduct = _service.GetByArticle(product.Article);
            Assert.That(updatedProduct.Name, Is.EqualTo(newName));
        }

        [Test]
        public void Delete_ShouldRemoveProduct()
        {
            var newProduct = new Product("Товар для удаления", 500);
            _service.Add(newProduct);
            var article = newProduct.Article;
            var initialCount = _service.GetAllProducts().Count;
            _service.Delete(article);
            var newCount = _service.GetAllProducts().Count;
            Assert.That(newCount, Is.EqualTo(initialCount - 1));
            Assert.That(_service.GetByArticle(article), Is.Null);
        }

        [Test]
        public void Add_ShouldAssignUniqueArticle_WhenArticleIsEmpty()
        {
            var first = new Product("Товар А", 100);
            var second = new Product("Товар Б", 200);
            _service.Add(first);
            _service.Add(second);
            Assert.That(first.Article, Is.Not.Empty);
            Assert.That(second.Article, Is.Not.Empty);
            Assert.That(second.Article, Is.Not.EqualTo(first.Article));
        }

        [Test]
        public void InvoiceItem_Total_ShouldBeQuantityTimesPrice()
        {
            var item = new InvoiceItem(new Product("Товар", 250), 4);
            Assert.That(item.Total, Is.EqualTo(1000m));
        }

        [Test]
        public void UpdateStock_ShouldChangeQuantity()
        {
            var product = _service.GetAllProducts().First();
            var initialStock = product.StockQuantity;
            var changeAmount = 10;
            _service.UpdateStock(product.Article, changeAmount);
            var updatedProduct = _service.GetByArticle(product.Article);
            Assert.That(updatedProduct.StockQuantity, Is.EqualTo(initialStock + changeAmount));
        }

        [Test]
        public void GetAllCategories_ShouldReturnCategories()
        {
            var result = _service.GetAllCategories();
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.GreaterThan(0));
        }

        [Test]
        public void GetAllManufacturers_ShouldReturnManufacturers()
        {
            var result = _service.GetAllManufacturers();
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.GreaterThan(0));
        }

        [Test]
        public void GetAllSuppliers_ShouldReturnSuppliers()
        {
            var result = _service.GetAllSuppliers();
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.GreaterThan(0));
        }

        [Test]
        public void GetProductsByWarehouse_ShouldReturnOnlyProductsInStock()
        {
            var warehouse = _warehouseService.GetAll().First();
            var result = _service.GetProductsByWarehouse(warehouse.Id);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.GreaterThan(0));
            foreach (var product in result)
            {
                Assert.That(product.StockQuantity, Is.GreaterThan(0));
            }
        }
    }
}
