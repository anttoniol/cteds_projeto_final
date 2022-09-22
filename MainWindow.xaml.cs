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
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Boolean connectWithDatabase()
        {
            Config config = ConfigManager.Loader();
            
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(config.ConnectionString))
                {
                    conn.Open();
                    MessageBox.Show("Conexão com o banco de dados feita com sucesso");
                }
                return true;
            }
            catch(Exception ex)
            {
                MessageBox.Show("Houve problema ao tentar conectar com o banco de dados");
                return false;
            }
            
        }

        private string[] cmbMonthOptions = { "Mês atual", "Meses anteriores" };
        public MainWindow()
        {
            InitializeComponent();
            initializeComboBox(cmbMonth, cmbMonthOptions);

            connectWithDatabase();
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
                //if (option == "Mês atual")

            } 

        }
    }
}
