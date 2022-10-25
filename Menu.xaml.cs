using System.Windows;

namespace cteds_projeto_final
{
    public partial class Menu : Window
    {
        public Menu()
        {
            InitializeComponent();
        }

        private void AddCategory(object sender, RoutedEventArgs e)
        {
            CategoryOperations categoryOperations = new CategoryOperations("add");
            categoryOperations.Show();
        }

        private void UpdateCategory(object sender, RoutedEventArgs e)
        {
            CategoryOperations categoryOperations = new CategoryOperations("update");
            categoryOperations.Show();
        }

        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void AddExpense(object sender, RoutedEventArgs e)
        {
            ExpenseOperations expenseOperations = new ExpenseOperations();
            expenseOperations.Show();
        }

        private void UpdateExpense(object sender, RoutedEventArgs e)
        {

        }

        private void DeleteExpense(object sender, RoutedEventArgs e)
        {

        }
    }
}
