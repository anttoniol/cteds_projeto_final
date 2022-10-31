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
            ExpenseOperations expenseOperations = new ExpenseOperations("add");
            expenseOperations.Show();
        }

        private void UpdateExpense(object sender, RoutedEventArgs e)
        {
            ExpenseOperations expenseOperations = new ExpenseOperations("update");
            expenseOperations.Show();
        }

        private void VisualizeExpenseTotal(object sender, RoutedEventArgs e)
        {
            ExpenseTotal expenseTotal = new ExpenseTotal();
            expenseTotal.Show();
        }
    }
}
