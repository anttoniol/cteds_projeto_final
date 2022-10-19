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

namespace cteds_projeto_final
{
    using Models;
    using Repositories;
    using System.Data.SQLite;

    /// <summary>
    /// Interaction logic for UserControl2.xaml
    /// </summary>
    public partial class CategorySave : UserControl
    {
        private CategoryRepository categoryRepository;
        public bool finished = false;

        public CategorySave()
        {
            InitializeComponent();
            SQLiteConnection? conn = Connection.connectWithDatabase();
            if (Connection.buildDatabaseContent(conn))
            {
                categoryRepository = new CategoryRepository(conn);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Category newCategory = new Category(txtCategoryName.Text);
            categoryRepository.AddCategory(newCategory);
            finished = true;
        }
    }
}
