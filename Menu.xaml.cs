using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace cteds_projeto_final
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Menu : Window
    {
        public Menu()
        {
            InitializeComponent();
        }

        private void AddCategory(object sender, RoutedEventArgs e)
        {
            CategoryAdd categoryAdd = new CategoryAdd();
            categoryAdd.Show();
        }

        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void AddExpense(object sender, RoutedEventArgs e)
        {
            ExpenseVisualizer expenseVisualizer = new ExpenseVisualizer();
            expenseVisualizer.Show();
        }

        private void UpdateExpense(object sender, RoutedEventArgs e)
        {

        }

        private void DeleteExpense(object sender, RoutedEventArgs e)
        {

        }

        private void UpdateCategory(object sender, RoutedEventArgs e)
        {

        }
    }
}
