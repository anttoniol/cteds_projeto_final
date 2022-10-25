using System;
using System.Windows;
using System.Windows.Controls;
using System.Data.SQLite;


namespace cteds_projeto_final
{
    using Repositories;

    public partial class ExpenseOperations : Window
    {
        private CategoryRepository categoryRepository;
        private ExpenseRepository expenseRepository;

        public CategoryRepository GetCategoryRepository()
        {
            return categoryRepository;
        }

        

        private string[] cmbMonthOptions = { "Mês atual", "Meses anteriores" };
        public ExpenseOperations()
        {
            InitializeComponent();
            // initializeComboBox(cmbMonth, cmbMonthOptions);

            SQLiteConnection? conn = Connection.connectWithDatabase();
            if (Connection.buildDatabaseContent(conn))
            {
                categoryRepository = new CategoryRepository(conn);
                expenseRepository = new ExpenseRepository(conn, categoryRepository);
            }
        }
        private void initializeComboBox(ComboBox cmbName, string[] options)
        {
            foreach (string option in options) 
            {
                int index = cmbName.Items.Add(option);
            }
        }

        private void checkCmbBox(object sender, SelectionChangedEventArgs e)
        {
            object selectedObject = ((ComboBox) sender).SelectedItem;
            if (selectedObject != null)
                MessageBox.Show($"Você selecionou '{selectedObject.ToString()}'");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void InsertNewCategory(object sender, RoutedEventArgs e)
        {
            CategoryOperations categoryOperations = new CategoryOperations("add");
            categoryOperations.Show();

        }

        private void ClearFields(object sender, RoutedEventArgs e)
        {
            txtDesc.Clear();
            txtValue.Clear();
            dpExpense.Text = "";
            cmbCategory.SelectedIndex = -1;
        }
    }
}
