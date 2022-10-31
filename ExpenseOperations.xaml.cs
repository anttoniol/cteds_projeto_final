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
        private string operation;

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
        public ExpenseOperations(string operation)
        {
            InitializeComponent();
            this.operation = operation;

            SQLiteConnection? conn = Connection.connectWithDatabase();
            if (Connection.buildDatabaseContent(conn))
            {
                categoryRepository = new CategoryRepository(conn);
                expenseRepository = new ExpenseRepository(conn, categoryRepository);
                FillCmbCategory();
                InsertContentIntoExpenseGrid();
                CheckOperation();
                dpExpense.DisplayDateEnd = DateTime.Now;
            }
        }

        private void CheckOperation()
        {
            switch (this.operation)
            {
                case "add":
                    SetFieldsStatusAdd();
                    break;
                case "update":
                    SetFieldsStatusUpdate();
                    break;
                default:
                    break;
            }
        }

        private void CheckComboBoxNumberOfOptions(ComboBox comboBox)
        {
            if (comboBox.Items.Count > 0)
                comboBox.IsEnabled = true;
            else
                comboBox.IsEnabled = false;
        }

        private void SetFieldsStatusUpdate()
        {
            txtDesc.IsEnabled = false;
            txtValue.IsEnabled = false;
            cmbCategory.IsEnabled = false;
            dpExpense.IsEnabled = false;
            btnUpdateExpense.IsEnabled = false;
        }

        private void SetFieldsStatusAdd()
        {
            txtDesc.IsEnabled = true;
            txtValue.IsEnabled = true;
            dpExpense.IsEnabled = true;
            btnUpdateExpense.IsEnabled = false;
            CheckComboBoxNumberOfOptions(cmbCategory);
        }

        private bool CheckFieldsGeneral()
        {
            if (txtDesc.Text.Trim().Length == 0)
            {
                MessageBox.Show("Descrição inválida!");
                return false;
            }

            decimal value;
            if (decimal.TryParse(txtValue.Text.Trim(), out value) == false || value <= 0 || value == decimal.MaxValue)
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

        private bool CheckFieldsAdd()
        {
            if (!CheckFieldsGeneral())
                return false;

            Expense? existingExpense = expenseRepository.GetByDesc(txtDesc.Text.Trim());
            if (existingExpense != null)
            {
                MessageBox.Show("Já existe um gasto com a mesma descrição!");
                return false;
            }
            return true;
        }

        private bool CheckFieldsUpdate(long? expenseId)
        {
            if (!CheckFieldsGeneral())
                return false;
            
            Expense? existingExpense = expenseRepository.GetByDescAndExcludeId(txtDesc.Text.Trim(), expenseId);
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
            if (this.operation != "update")
                CheckComboBoxNumberOfOptions(cmbCategory);
        } 

        private void ClearFields(object sender, RoutedEventArgs e)
        {
            txtDesc.Clear();
            txtValue.Clear();
            dpExpense.Text = "";
            cmbCategory.SelectedIndex = -1;
        }

        private Expense? FormatFields(long? expenseId = null)
        {
            decimal? convertedValue = ConvertStringToDecimal(txtValue.Text.Trim());
            if (convertedValue == null)
                return null;
            Category? category = categoryRepository.GetByName(cmbCategory.SelectedItem.ToString());
            Expense expense = new Expense(
                value: (decimal) convertedValue,
                desc: txtDesc.Text.Trim(),
                category_id: (long) category.categoryId,
                expense_dttm: (DateTime) dpExpense.SelectedDate,
                expenseId: expenseId
            );
            return expense;
        }

        private decimal? ConvertStringToDecimal(string number)
        {
            decimal convertedNumber;
            bool ok = decimal.TryParse(number, NumberStyles.Any, CultureInfo.CurrentUICulture, out convertedNumber);
            if (ok)
                return convertedNumber;
            return null;
        }

        private string ConvertDecimalToString(decimal number)
        {
            return number.ToString("0." + new string('#', 339)).TrimEnd('0');
        }

        private void FillFieldsToEdit(object sender, EventArgs e)
        {

            Button clickedButton = (Button)sender;
            Tuple<Expense?, int> tag = (Tuple<Expense?, int>)clickedButton.Tag;
            Expense? expense = tag.Item1;
            txtDesc.Text = expense.desc;
            dpExpense.SelectedDate = expense.expense_dttm;

            Label valueLabel = (Label)SearchGridChild(tag.Item2, 2);
            txtValue.Text = ConvertDecimalToString(expense.value);

            Label categoryLabel = (Label)SearchGridChild(tag.Item2, 0);
            string categoryName = categoryLabel.Content.ToString();
            cmbCategory.SelectedItem = categoryName;

            btnUpdateExpense.Click -= UpdateExpense;
            btnUpdateExpense.Click += UpdateExpense;
            btnUpdateExpense.Tag = tag;

            EnableFieldsUpdate();
        }
  

        private void FillFieldsToDelete(object sender, EventArgs e)
        {
            Button clickedButton = (Button)sender;
            Tuple<Expense?, int> tag = (Tuple<Expense?, int>) clickedButton.Tag;

            string message = "Você realmente deseja remover esse gasto?";
            string title = "Remover Gasto";
            MessageBoxButton buttons = MessageBoxButton.YesNo;
            MessageBoxResult result = MessageBox.Show(message, title, buttons);
            if (result == MessageBoxResult.Yes)
            {
                if (expenseRepository.DeleteExpense(tag.Item1) != null)
                {
                    grdExpense.Children.Clear();
                    grdExpense.RowDefinitions.Clear();
                    grdExpense.ColumnDefinitions.Clear();
                    InsertContentIntoExpenseGrid();
                }
            }

            SetFieldsStatusAdd();
        }

        private void EnableFieldsUpdate()
        {
            txtDesc.IsEnabled = true;
            txtValue.IsEnabled = true;
            cmbCategory.IsEnabled = true;
            dpExpense.IsEnabled = true;
            btnUpdateExpense.IsEnabled = true;
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

        private void InsertDeletionButton(Expense? expense, int row, int col)
        {
            Button button = new Button();
            button.Content = "Remover";
            button.Height = 29;
            button.Width = 64;
            button.Tag = new Tuple<Expense?, int>(expense, row);
            button.Click += FillFieldsToDelete;
            InsertChildOnExpenseGrid(button, row, col);
        }

        private void InsertExpenseAttribute(string attribute, int row, int col)
        {
            Label lblAttribute = new Label();
            lblAttribute.Content = attribute.Trim();
            lblAttribute.Width = 10 * attribute.Length;
            lblAttribute.Height = 30;
            InsertChildOnExpenseGrid(lblAttribute, row, col);
        }

        private void InsertExpenseOnGrid(Expense expense, int row, double? heightDelta = 20)
        {
            Console.WriteLine("LEGAL");
            Category? category = categoryRepository.GetById(expense.category_id);
            string categoryName = category.name;

            if (heightDelta != null)
            {
                grdExpense.RowDefinitions.Add(new RowDefinition());
                grdExpense.Height += (double) heightDelta;
            }

            InsertExpenseAttribute(categoryName, row, 0);
            InsertExpenseAttribute(expense.desc, row, 1);
            
            string formattedValue;
            if (expense.value > 1000000000000)
                formattedValue = expense.value.ToString("E", CultureInfo.InvariantCulture);
            else
                formattedValue = expense.value.ToString();
            InsertExpenseAttribute(formattedValue, row, 2);

            InsertExpenseAttribute(expense.expense_dttm.ToShortDateString(), row, 3);
            InsertEditionButton(expense, row, 4);
            InsertDeletionButton(expense, row, 5);
        }

        private void UpdateGridRow(Expense expense, int row)
        {
            UIElementCollection childrenEnumerator = grdExpense.Children;
            List<UIElement> childrenInRow = new List<UIElement>();
            for (int i = 0; i < childrenEnumerator.Count; i++)
            {
                if (Grid.GetRow(childrenEnumerator[i]) == row)
                    childrenInRow.Add(childrenEnumerator[i]);
            }

            foreach(UIElement child in childrenInRow)
                grdExpense.Children.Remove(child);

            InsertExpenseOnGrid(expense, row, null);
        }

        private UIElement? SearchGridChild(int row, int col)
        {
            UIElementCollection childrenEnumerator = grdExpense.Children;
            foreach (UIElement child in childrenEnumerator)
            {
                if (Grid.GetRow(child) == row && Grid.GetColumn(child) == col)
                    return child; 
            }
            return null;
        }

        private void UpdateExpenseGrid(Expense expense)
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
            string[] columns = { "CATEGORIA", "DESCRIÇÃO", "VALOR", "DATA DO GASTO", "EDIÇÃO", "REMOÇÃO" };
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
            if (CheckFieldsAdd())
            {
                Expense? savedExpense = expenseRepository.AddExpense(FormatFields());
                if (savedExpense != null)
                {
                    MessageBox.Show("Gasto adicionado com sucesso!");
                    UpdateExpenseGrid(savedExpense);
                    ClearFields();
                    btnUpdateExpense.IsEnabled = false;
                }
                else
                    MessageBox.Show("Ocorreu um erro no cadastro do gasto!");
            }
        }

        private void UpdateExpense(object sender, RoutedEventArgs e)
        {
            Button clickedButton = (Button) sender;
            Tuple<Expense?, int> tag = (Tuple<Expense?, int>) clickedButton.Tag;
            Expense? expense = tag.Item1;
            long? expenseId = expense.expenseId;

            if (CheckFieldsUpdate(expenseId))
            {
                Expense? updatedExpense = expenseRepository.UpdateExpense(FormatFields(expenseId));
                if (updatedExpense != null)
                {
                    MessageBox.Show("Gasto atualizado com sucesso!");
                    UpdateGridRow(updatedExpense, tag.Item2);
                    ClearFields();
                    btnUpdateExpense.IsEnabled = false;
                }
                else
                    MessageBox.Show("Ocorreu um erro na atualização do gasto!");
            }
        }
    }
}
