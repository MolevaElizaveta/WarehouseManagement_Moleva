using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WarehouseManagement_Moleva.WPF.ViewModels
{
    /// Клиентская валидация: подсветка поля с ошибкой + всплывающее сообщение
    public static class FieldValidation
    {
        public static void Reset(params Control[] controls)
        {
            var normal = (Brush)Application.Current.Resources["BorderBrush"];
            foreach (var c in controls)
            {
                c.BorderBrush = normal;
                c.BorderThickness = new Thickness(1);
            }
        }

        /// Подсвечивает поле, показывает сообщение и возвращает false
        public static bool Fail(Control control, string message)
        {
            control.BorderBrush = (Brush)Application.Current.Resources["DeleteBrush"];
            control.BorderThickness = new Thickness(2);
            MessageBox.Show(message, "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Warning);
            control.Focus();
            return false;
        }
    }
}
