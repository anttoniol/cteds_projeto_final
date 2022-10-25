using System;
using System.Windows;
using System.Windows.Controls;
using System.Data.SQLite;


namespace cteds_projeto_final
{
    using cteds_projeto_final.Models;
    using Repositories;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    public partial class ExpenseOperations : Window
    {
        private CategoryRepository categoryRepository;
        private ExpenseRepository expenseRepository;

        private void FillCmbCategory()
        {
            List<Category> categories = categoryRepository.GetAll();
            if (categories.Count != 0)
            {
                foreach (Category category in categories)
                    cmbCategory.Items.Add(category.name);
            }
        }        

        private string[] cmbMonthOptions = { "Mês atual", "Meses anteriores" };
        public ExpenseOperations()
        {
            InitializeComponent();

            SQLiteConnection? conn = Connection.connectWithDatabase();
            if (Connection.buildDatabaseContent(conn))
            {
                categoryRepository = new CategoryRepository(conn);
                expenseRepository = new ExpenseRepository(conn, categoryRepository);
                FillCmbCategory();
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
            Close();
        }

        private bool CheckFieldsGeneral()
        {
            if (txtDesc.Text.Trim().Length == 0)
            {
                MessageBox.Show("Descrição inválida!");
                return false;
            }

            double value;
            if (double.TryParse(txtValue.Text.Trim(), out value) == false || value <= 0)
            {
                MessageBox.Show("Valor inválido para o gasto!");
                return false;
            }

            if (cmbCategory.SelectedIndex == -1)
            {
                MessageBox.Show("Categoria inválida!");
                return false;
            }

            if (dpExpense.SelectedDate == null)
            {
                MessageBox.Show("Data inválida!");
                return false;
            }
            return true;
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

        private void AddExpense(object sender, RoutedEventArgs e)
        {
            if (CheckFieldsGeneral())
            {
                MessageBox.Show("Todos os dados estão ok!");
            }
        }
    }
}
