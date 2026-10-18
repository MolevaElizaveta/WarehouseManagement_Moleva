using System.Collections.Generic;
using System.Linq;
using System.Windows;
using WarehouseManagement_Moleva.Data.Models;
using WarehouseManagement_Moleva.WPF.ViewModels;

namespace WarehouseManagement_Moleva.WPF
{
    public partial class ProductDialog : Window
    {
        public Product? Product { get; private set; }

        public ProductDialog(List<Category> categories,
                              List<Manufacturer> manufacturers,
                              List<Supplier> suppliers,
                              Product? existingProduct = null)
        {
            InitializeComponent();

            CmbCategory.ItemsSource = categories;
            CmbManufacturer.ItemsSource = manufacturers;
            CmbSupplier.ItemsSource = suppliers;

            if (existingProduct != null)
            {
                Title = "Редактирование товара";
                Product = existingProduct;

                TxtName.Text = existingProduct.Name;
                TxtUnit.Text = existingProduct.Unit;
                TxtPrice.Text = existingProduct.Price.ToString();
                TxtDiscount.Text = existingProduct.DiscountPercent.ToString();
                TxtStockQuantity.Text = existingProduct.StockQuantity.ToString();
                TxtDescription.Text = existingProduct.Description;

                CmbCategory.SelectedValue = existingProduct.CategoryId;
                CmbManufacturer.SelectedValue = existingProduct.ManufacturerId;
                CmbSupplier.SelectedValue = existingProduct.SupplierId;
            }
            else
            {
                Title = "Новый товар";
                Product = new Product();

                if (categories.Any()) CmbCategory.SelectedIndex = 0;
                if (manufacturers.Any()) CmbManufacturer.SelectedIndex = 0;
                if (suppliers.Any()) CmbSupplier.SelectedIndex = 0;
            }
        }

        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            FieldValidation.Reset(TxtName, TxtUnit, TxtPrice, TxtDiscount, TxtStockQuantity,
                                  CmbCategory, CmbManufacturer, CmbSupplier);

            if (string.IsNullOrWhiteSpace(TxtName.Text))
            { FieldValidation.Fail(TxtName, "Введите наименование товара"); return; }

            if (string.IsNullOrWhiteSpace(TxtUnit.Text))
            { FieldValidation.Fail(TxtUnit, "Введите единицу измерения"); return; }

            if (CmbCategory.SelectedItem == null)
            { FieldValidation.Fail(CmbCategory, "Выберите категорию"); return; }

            if (CmbManufacturer.SelectedItem == null)
            { FieldValidation.Fail(CmbManufacturer, "Выберите производителя"); return; }

            if (CmbSupplier.SelectedItem == null)
            { FieldValidation.Fail(CmbSupplier, "Выберите поставщика"); return; }

            if (!decimal.TryParse(TxtPrice.Text, out var price))
            { FieldValidation.Fail(TxtPrice, "Цена должна быть числом"); return; }

            if (price <= 0)
            { FieldValidation.Fail(TxtPrice, "Цена должна быть больше нуля"); return; }

            if (!decimal.TryParse(TxtDiscount.Text, out var discount) || discount < 0 || discount > 100)
            { FieldValidation.Fail(TxtDiscount, "Скидка должна быть числом от 0 до 100"); return; }

            if (!int.TryParse(TxtStockQuantity.Text, out var stock) || stock < 0)
            { FieldValidation.Fail(TxtStockQuantity, "Остаток должен быть целым числом, не меньше нуля"); return; }

            Product!.Name = TxtName.Text;
            Product.Unit = TxtUnit.Text.Trim();
            Product.Price = price;
            Product.DiscountPercent = discount;
            Product.StockQuantity = stock;
            Product.Description = TxtDescription.Text;

            if (CmbCategory.SelectedItem is Category category)
            {
                Product.CategoryId = category.Id;
                Product.Category = category;
            }

            if (CmbManufacturer.SelectedItem is Manufacturer manufacturer)
            {
                Product.ManufacturerId = manufacturer.Id;
                Product.Manufacturer = manufacturer;
            }

            if (CmbSupplier.SelectedItem is Supplier supplier)
            {
                Product.SupplierId = supplier.Id;
                Product.Supplier = supplier;
                Product.SupplierName = supplier.Name;
            }

            DialogResult = true;
            Close();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}