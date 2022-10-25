using System;
using System.Windows;
using System.Windows.Controls;
using System.Data.SQLite;


namespace cteds_projeto_final
{
    using cteds_projeto_final.Models;
    using Repositories;
    using System.Collections.Generic;
    using System.Globalization;

    public partial class ExpenseOperations : Window
    {
        private CategoryRepository categoryRepository;
        private ExpenseRepository expenseRepository;

        private void FillCmbCategory()
        {
            cmbCategory.Items.Clear();
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
                InsertContentIntoExpenseGrid();
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

            float value;
            if (float.TryParse(txtValue.Text.Trim(), out value) == false || value <= 0)
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

            Expense? existingExpense = expenseRepository.GetByDesc(txtDesc.Text.Trim());
            if (existingExpense != null)
            {
                MessageBox.Show("Já existe um gasto com a mesma descrição!");
                return false;
            }

            return true;
        }
        private void InsertNewCategory(object sender, RoutedEventArgs e)
        {
            CategoryOperations categoryOperations = new CategoryOperations("add");
            categoryOperations.ShowDialog();
            FillCmbCategory();
        }

        private void ClearFields(object sender, RoutedEventArgs e)
        {
            txtDesc.Clear();
            txtValue.Clear();
            dpExpense.Text = "";
            cmbCategory.SelectedIndex = -1;
        }

        private Expense FormatFields()
        {
            Category? category = categoryRepository.GetByName(cmbCategory.SelectedItem.ToString());
            Expense expense = new Expense(
                value: float.Parse(txtValue.Text.Trim()),
                desc: txtDesc.Text.Trim(),
                category_id: (long) category.categoryId,
                expense_dttm: (DateTime) dpExpense.SelectedDate,
                added_dttm: DateTime.Now
            );
            return expense;
        }

        private string ConvertFloatToString(string value)
        {
            return double.Parse(value, NumberStyles.Any, CultureInfo.InvariantCulture).ToString();
        }

        private void FillFieldsToEdit(object sender, EventArgs e)
        {

            Button clickedButton = (Button)sender;
            Tuple<Expense?, int> tag = (Tuple<Expense?, int>) clickedButton.Tag;
            Expense? expense = tag.Item1;
            txtDesc.Text = expense.desc;
            txtValue.Text = ConvertFloatToString(expense.value.ToString());
            dpExpense.SelectedDate = expense.expense_dttm;

            Label categoryLabel = (Label) searchGridChild(tag.Item2, 0);
            string categoryName = categoryLabel.Content.ToString();
            cmbCategory.SelectedItem = categoryName;
            
            btnUpdateExpense.Click -= UpdateExpense;
            btnUpdateExpense.Click += UpdateExpense;
            btnUpdateExpense.Tag = tag;

            EnableFieldsUpdate();
        }

        private void EnableFieldsUpdate()
        {
            txtDesc.IsEnabled = true;
            txtValue.IsEnabled = true;
            cmbCategory.IsEnabled = true;
            dpExpense.IsEnabled = true;
        }

        private void InsertChildOnExpenseGrid(UIElement child, int row, int col)
        {
            Grid.SetRow(child, row);
            Grid.SetColumn(child, col);
            grdExpense.Children.Add(child);
        }

        private void InsertEditionButton(Expense? expense, int row, int col)
        {
            Button button = new Button();
            button.Content = "Editar";
            button.Height = 29;
            button.Width = 64;
            button.Tag = new Tuple<Expense?, int>(expense, row);
            button.Click += FillFieldsToEdit;
            InsertChildOnExpenseGrid(button, row, col);
        }

        private void InsertExpenseAttribute(string attribute, int row, int col)
        {
            Label lblDesc = new Label();
            lblDesc.Content = attribute;
            lblDesc.Width = 10 * attribute.Length;
            lblDesc.Height = 30;
            InsertChildOnExpenseGrid(lblDesc, row, col);
        }

        private void InsertExpenseOnGrid(Expense expense, int row, double? heightDelta = 20)
        {
            string categoryName = categoryRepository.GetById(expense.category_id).name;

            if (heightDelta != null)
            {
                grdExpense.RowDefinitions.Add(new RowDefinition());
                grdExpense.Height += (double) heightDelta;
            }
            InsertExpenseAttribute(categoryName, row, 0);
            InsertExpenseAttribute(expense.desc, row, 1);
            InsertExpenseAttribute(expense.value.ToString(), row, 2);
            InsertExpenseAttribute(expense.expense_dttm.ToShortDateString(), row, 3);
            InsertEditionButton(expense, row, 4);
        }

        private void updateGridRow(Expense expense, int row)
        {
            UIElementCollection childrenEnumerator = grdExpense.Children;
            for (int i = 0; i < childrenEnumerator.Count; i++)
            {
                if (Grid.GetRow(childrenEnumerator[i]) == row)
                    grdExpense.Children.Remove(childrenEnumerator[i]);
            }
            InsertExpenseOnGrid(expense, row, null);
        }

        private UIElement? searchGridChild(int row, int col)
        {
            UIElementCollection childrenEnumerator = grdExpense.Children;
            foreach (UIElement child in childrenEnumerator)
            {
                if (Grid.GetRow(child) == row && Grid.GetColumn(child) == col)
                    return child; 
            }
            return null;
        }

        private void updateExpenseGrid(Expense expense)
        {
            if (expense != null)
            {
                int rows = grdExpense.RowDefinitions.Count;
                InsertExpenseOnGrid(expense, rows);
            }
        }

        private void CompleteExpenseGrid()
        {
            List<Expense> expenses = expenseRepository.GetAll();
            int i = 1;
            foreach (Expense? expense in expenses)
            {
                InsertExpenseOnGrid(expense, i);
                i += 1;
            }
        }

        private void InsertColumnNamesOnExpenseGrid()
        {
            string[] columns = { "CATEGORIA", "DESCRIÇÃO", "VALOR", "DATA DO GASTO", "EDITAR" };
            grdExpense.RowDefinitions.Add(new RowDefinition());
            for (int i = 0; i < columns.Length; i++)
            {
                Label columnName = new Label();
                columnName.Content = columns[i];
                columnName.Width = 100;
                columnName.Height = 30;

                grdExpense.ColumnDefinitions.Add(new ColumnDefinition());
                Grid.SetRow(columnName, 0);
                Grid.SetColumn(columnName, i);
                grdExpense.Children.Add(columnName);
            }
        }

        private void InsertContentIntoExpenseGrid()
        {
            InsertColumnNamesOnExpenseGrid();
            CompleteExpenseGrid();
        }

        private void ClearFields()
        {
            txtDesc.Clear();
            txtValue.Clear();
            cmbCategory.SelectedIndex = -1;
            dpExpense.SelectedDate = null;
        }

        private void AddExpense(object sender, RoutedEventArgs e)
        {
            if (CheckFieldsGeneral())
            {
                MessageBox.Show("Todos os dados estão ok!");

                Expense? savedExpense = expenseRepository.AddExpense(FormatFields());
                if (savedExpense != null)
                {
                    MessageBox.Show("Gasto adicionado com sucesso!");
                    updateExpenseGrid(savedExpense);
                    ClearFields();
                    btnUpdateExpense.IsEnabled = false;
                }
                else
                    MessageBox.Show("Ocorreu um erro no cadastro do gasto!");
            }
        }

        private void UpdateExpense(object sender, RoutedEventArgs e)
        {

        }
    }
}
