using cteds_projeto_final.Repositories;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;

namespace cteds_projeto_final
{
    public partial class ExpenseTotal : Window
    {
        private CategoryRepository categoryRepository;
        private ExpenseRepository expenseRepository;
        public ExpenseTotal()
        {
            InitializeComponent();
            SQLiteConnection? conn = Connection.connectWithDatabase();
            if (Connection.buildDatabaseContent(conn))
            {
                categoryRepository = new CategoryRepository(conn);
                expenseRepository = new ExpenseRepository(conn, categoryRepository);
                dpBegin.DisplayDateEnd = DateTime.Now;
                dpEnd.DisplayDateEnd = DateTime.Now;
            }
        }

        private void InsertChildOnExpenseTotalGrid(UIElement child, int row, int col)
        {
            Grid.SetRow(child, row);
            Grid.SetColumn(child, col);
            grdExpenseTotal.Children.Add(child);
        }

        private void InsertExpenseTotalAttribute(string attribute, int row, int col)
        {
            Label lblAttribute = new Label();
            lblAttribute.Content = attribute.Trim();
            lblAttribute.Width = 10 * attribute.Length;
            lblAttribute.Height = 30;
            InsertChildOnExpenseTotalGrid(lblAttribute, row, col);
        }

        private void InsertExpenseOnGridByMonth(List<string> expenseRow, int row, double? heightDelta = 20)
        {
            string[] expenseRowArray = expenseRow.ToArray();
            if (heightDelta != null)
            {
                grdExpenseTotal.RowDefinitions.Add(new RowDefinition());
                grdExpenseTotal.Height += (double)heightDelta;
            }

            string formattedValue;
            decimal expenseTotal = decimal.Parse(expenseRowArray[1]);
            if (expenseTotal > 1000000000000)
                formattedValue = expenseTotal.ToString("E", CultureInfo.InvariantCulture);
            else
                formattedValue = expenseTotal.ToString();

            InsertExpenseTotalAttribute(expenseRowArray[0], row, 0);
            InsertExpenseTotalAttribute(formattedValue, row, 1);
        }

        private void InsertExpenseOnGridByMonthAndCategory(List<string> expenseRow, int row, double? heightDelta = 20)
        {
            string[] expenseRowArray = expenseRow.ToArray();
            if (heightDelta != null)
            {
                grdExpenseTotal.RowDefinitions.Add(new RowDefinition());
                grdExpenseTotal.Height += (double)heightDelta;
            }

            string formattedValue;
            decimal expenseTotal = decimal.Parse(expenseRowArray[2]);
            if (expenseTotal > 1000000000000)
                formattedValue = expenseTotal.ToString("E", CultureInfo.InvariantCulture);
            else
                formattedValue = expenseTotal.ToString();

            InsertExpenseTotalAttribute(expenseRowArray[0], row, 0);
            InsertExpenseTotalAttribute(expenseRowArray[1], row, 1);
            InsertExpenseTotalAttribute(formattedValue, row, 2);
        }

        private void CompleteExpenseGridByMonth(DateTime begin, DateTime end)
        {
            List<List<string>> expensesTotal = expenseRepository.GetExpenseTotalByMonth(begin, end);
            if (expensesTotal.Count == 0)
                return;

            int i = 1;
            foreach (List<string> expenseTotal in expensesTotal)
            {
                InsertExpenseOnGridByMonth(expenseTotal, i);
                i += 1;
            }
        }

        private void CompleteExpenseGridByMonthAndCategory(DateTime begin, DateTime end)
        { 
            List<List<string>> expensesTotal = expenseRepository.GetExpenseTotalByMonthAndCategory(begin, end);
            if (expensesTotal.Count == 0)
                return;

            int i = 1;
            foreach (List<string> expenseTotal in expensesTotal)
            {
                InsertExpenseOnGridByMonthAndCategory(expenseTotal, i);
                i += 1;
            }
        }

        private void InsertContentIntoExpenseGrid(string visualizationType, DateTime begin, DateTime end)
        {
            grdExpenseTotal.Children.Clear();
            grdExpenseTotal.RowDefinitions.Clear();
            grdExpenseTotal.ColumnDefinitions.Clear();
            grdExpenseTotal.Height = 249;

            List<string> columnNames;
            switch (visualizationType)
            {
                case "Mês":
                    columnNames = new List<string>();
                    columnNames.Add("MÊS-ANO");
                    columnNames.Add("GASTO TOTAL");
                    InsertColumnNamesOnExpenseGrid(columnNames.ToArray());
                    CompleteExpenseGridByMonth(begin, end);
                    break;

                case "Categoria e mês":
                    columnNames = new List<string>();
                    columnNames.Add("MÊS-ANO");
                    columnNames.Add("CATEGORIA");
                    columnNames.Add("GASTO TOTAL");
                    InsertColumnNamesOnExpenseGrid(columnNames.ToArray());
                    CompleteExpenseGridByMonthAndCategory(begin, end);
                    break;

                default:
                    break;
            }
            
        }

        private void InsertColumnNamesOnExpenseGrid(string[] columns)
        {
            grdExpenseTotal.RowDefinitions.Add(new RowDefinition());
            for (int i = 0; i < columns.Length; i++)
            {
                Label columnName = new Label();
                columnName.Content = columns[i];
                columnName.Width = 100;
                columnName.Height = 30;

                grdExpenseTotal.ColumnDefinitions.Add(new ColumnDefinition());
                Grid.SetRow(columnName, 0);
                Grid.SetColumn(columnName, i);
                grdExpenseTotal.Children.Add(columnName);
            }
        }

        private bool CheckFields()
        {
            if (cmbVisualizationType.SelectedIndex == -1)
            {
                MessageBox.Show("Tipo de visualização inválida!");
                return false;
            }

            if (dpBegin.SelectedDate == null || dpEnd.SelectedDate == null)
            {
                MessageBox.Show("Data inicial e/ou final inválida(s)!");
                return false;
            }

            if (dpBegin.SelectedDate > dpEnd.SelectedDate)
            {
                MessageBox.Show("A data inicial deve ser anterior ou igual à data final!");
                return false;
            }
            return true;
        }

        private void btnVisualize_Click(object sender, RoutedEventArgs e)
        {
            if (CheckFields())
            {
                string visualizationType = cmbVisualizationType.SelectionBoxItem.ToString();
                List<string> columns = new List<string>();
                DateTime begin = (DateTime) dpBegin.SelectedDate;
                DateTime end = (DateTime) dpEnd.SelectedDate;

                InsertContentIntoExpenseGrid(visualizationType, begin, end);
            }
        }
    }
}
