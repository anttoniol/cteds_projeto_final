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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ExpenseVisualizer expenseVisualizer = new ExpenseVisualizer();
            expenseVisualizer.Show();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            CategoryVisualizer categoryVisualizer = new CategoryVisualizer();
            categoryVisualizer.Show();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
           this.Close();
        }
    }
}
