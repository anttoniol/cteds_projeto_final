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
using System.Windows.Shapes;

namespace cteds_projeto_final
{
    /// <summary>
    /// Interaction logic for CategoryActions.xaml
    /// </summary>
    public partial class CategoryActions : Window
    {
        public CategoryActions()
        {
            InitializeComponent();
        }

        private void CategorySave_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
