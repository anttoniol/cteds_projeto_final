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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SqlClient;
using System.Data.SQLite;
using authenticator.Configuration;


namespace cteds_projeto_final
{
    using Models;
    using Repositories;
    using System.IO;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class ExpenseVisualizer : Window
    {
        private CategoryRepository categoryRepository;
        private ExpenseRepository expenseRepository;

        public CategoryRepository GetCategoryRepository()
        {
            return categoryRepository;
        }

        

        private string[] cmbMonthOptions = { "Mês atual", "Meses anteriores" };
        public ExpenseVisualizer()
        {
            InitializeComponent();
            initializeComboBox(cmbMonth, cmbMonthOptions);

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

        private void checkCmbMonth(object sender, SelectionChangedEventArgs e)
        {
            object selectedItem = cmbMonth.SelectedItem;
            if (selectedItem != null)
            {
                MessageBox.Show($"Você selecionou '{selectedItem.ToString()}'");

                string option = selectedItem.ToString();
                if (option == "Mês atual")
                {
                    Category newCategory = new Category("teste");
                    categoryRepository.AddCategory(newCategory);
                } else if (option == "Meses anteriores")
                {
                    Console.WriteLine("Depois");
                    
                    /*
                    Category newCategory = new Category("teste_expense");
                    categoryRepository.AddCategory(newCategory);

                    long
                    Expense newExpense = new Expense(123, "expense test", long category_id, DateTime added_dttm, long ? expenseId = null);
                    categoryRepository.AddCategory(newCategory);
                    */
                }

            } 

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
