using System.Windows;
using WarehouseManagement_Moleva.WPF.ViewModels;

namespace WarehouseManagement_Moleva.WPF
{
    public partial class WarehouseDialog : Window
    {
        public string WarehouseName => TxtWarehouseName.Text.Trim();
        public string WarehouseAddress => TxtWarehouseAddress.Text.Trim();

        public WarehouseDialog(string initialName = "", string initialAddress = "")
        {
            InitializeComponent();
            TxtWarehouseName.Text = initialName;
            TxtWarehouseAddress.Text = initialAddress;

            if (!string.IsNullOrEmpty(initialName))
            {
                Title = "Редактирование склада";
            }
            TxtWarehouseName.Focus();
        }

        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            FieldValidation.Reset(TxtWarehouseName, TxtWarehouseAddress);
            if (string.IsNullOrWhiteSpace(TxtWarehouseName.Text))
            {
                FieldValidation.Fail(TxtWarehouseName, "Введите название склада");
                return;
            }
            if (string.IsNullOrWhiteSpace(TxtWarehouseAddress.Text))
            {
                FieldValidation.Fail(TxtWarehouseAddress, "Введите адрес склада");
                return;
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
