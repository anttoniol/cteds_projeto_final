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
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string[] cmbMonthOptions = { "Mês atual", "Meses anteriores" };
        public MainWindow()
        {
            InitializeComponent();
            initializeComboBox(cmbMonth, cmbMonthOptions);
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
    }
}
