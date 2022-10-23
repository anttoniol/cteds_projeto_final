using cteds_projeto_final.Models;
using cteds_projeto_final.Repositories;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
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
    /// Interaction logic for CategoryActions.xaml
    /// </summary>
    public partial class CategoryAdd : Window
    {
        private CategoryRepository categoryRepository;
        public CategoryAdd()
        {
            InitializeComponent();
            SQLiteConnection? conn = Connection.connectWithDatabase();
            if (Connection.buildDatabaseContent(conn))
            {
                categoryRepository = new CategoryRepository(conn);
                insertContentIntoCategoryGrid();
            }
        }
        private void insertContentIntoCategoryGrid()
        {
            DataGridTextColumn categoryId = new DataGridTextColumn();
            DataGridTextColumn categoryName = new DataGridTextColumn();

            grdCategory.Columns.Add(categoryId);
            grdCategory.Columns.Add(categoryName);

            categoryId.Binding = new Binding("id");
            categoryName.Binding = new Binding("name");

            categoryId.Header = "ID";
            categoryName.Header = "NOME";

            List<Tuple<long, string>> data = categoryRepository.GetAll();
            foreach (Tuple<long, string> item in data)
                grdCategory.Items.Add(new { id = item.Item1, name = item.Item2 });

        }
        private void updateCategoryGrid(Category? category)
        {
            if (category != null)
            {
                grdCategory.Items.Add(new { id = category.categoryId, name = category.name });
                grdCategory.Items.Refresh();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void AddCategory(object sender, RoutedEventArgs e)
        {
            if (txtCategoryName.Text.Trim().Length == 0)
                MessageBox.Show("Nome de categoria inválido!");
            else
            {
                Category newCategory = new Category(txtCategoryName.Text);
                Category? savedCategory = categoryRepository.AddCategory(newCategory);
                Console.WriteLine(savedCategory.ToString());

                MessageBox.Show("Categoria adicionada com sucesso!");
                updateCategoryGrid(savedCategory);
            }
        }
    }
}
