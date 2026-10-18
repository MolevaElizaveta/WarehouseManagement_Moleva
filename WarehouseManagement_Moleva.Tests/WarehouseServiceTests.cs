using NUnit.Framework;
using WarehouseManagement_Moleva.Data.Models;
using WarehouseManagement_Moleva.Data.Services;
using System.Linq;

namespace WarehouseManagement_Moleva.Tests.ServicesTests
{
    [TestFixture]
    public class WarehouseServiceTests
    {
        private WarehouseService _service;
        private OrganizationService _orgService;

        [SetUp]
        public void Setup()
        {
            _orgService = new OrganizationService();
            _service = new WarehouseService(_orgService);
        }

        [Test]
        public void GetAll_ShouldReturnWarehouses()
        {
            var result = _service.GetAll();
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.GreaterThan(0));
        }

        [Test]
        public void GetByOrganizationId_ShouldReturnCorrectWarehouses()
        {
            var firstOrg = _orgService.GetAll().First();
            var warehouses = _service.GetByOrganizationId(firstOrg.Id);
            Assert.That(warehouses, Is.Not.Null);
            foreach (var warehouse in warehouses)
            {
                Assert.That(warehouse.OrganizationId, Is.EqualTo(firstOrg.Id));
            }
        }

        [Test]
        public void Add_ShouldIncreaseCount()
        {
            var firstOrg = _orgService.GetAll().First();
            var initialCount = _service.GetAll().Count;
            var newWarehouse = new Warehouse("Тестовый склад", "Тестовый адрес", firstOrg.Id);
            _service.Add(newWarehouse);
            var newCount = _service.GetAll().Count;
            Assert.That(newCount, Is.EqualTo(initialCount + 1));
        }

        [Test]
        public void GetById_ShouldReturnCorrectWarehouse()
        {
            var firstWarehouse = _service.GetAll().First();
            var result = _service.GetById(firstWarehouse.Id);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(firstWarehouse.Id));
        }

        [Test]
        public void Delete_ShouldRemoveWarehouse()
        {
            var firstOrg = _orgService.GetAll().First();
            var newWarehouse = new Warehouse("Для удаления", "Адрес", firstOrg.Id);
            _service.Add(newWarehouse);
            var warehouseId = newWarehouse.Id;
            var initialCount = _service.GetAll().Count;
            _service.Delete(warehouseId);
            var newCount = _service.GetAll().Count;
            Assert.That(newCount, Is.EqualTo(initialCount - 1));
            Assert.That(_service.GetById(warehouseId), Is.Null);
        }

        [Test]
        public void Update_ShouldChangeWarehouse()
        {
            var warehouse = _service.GetAll().First();
            warehouse.Name = "Переименованный склад";
            warehouse.Address = "Новый адрес";
            _service.Update(warehouse);
            var updated = _service.GetById(warehouse.Id);
            Assert.That(updated, Is.Not.Null);
            Assert.That(updated.Name, Is.EqualTo("Переименованный склад"));
            Assert.That(updated.Address, Is.EqualTo("Новый адрес"));
        }
    }
}
