using cteds_projeto_final.Models;
using cteds_projeto_final.Repositories;
using Dsafa.WpfColorPicker;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;


namespace cteds_projeto_final
{
    public partial class CategoryOperations : Window
    {
        private CategoryRepository categoryRepository;
        private ColorPickerDialog? colorPicker;

        public CategoryOperations(string operation)
        {
            InitializeComponent();
            SQLiteConnection? conn = Connection.connectWithDatabase();
            if (Connection.buildDatabaseContent(conn))
            {
                categoryRepository = new CategoryRepository(conn);
                insertContentIntoCategoryGrid();
                CreateColorPicker();
                CheckOperation(operation);
            }
        }

        private void CheckOperation(string operation)
        {
            switch (operation)
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

        private void SetFieldsStatusUpdate()
        {
            txtCategoryName.IsEnabled = false;
            btnAddCategory.IsEnabled = false;
            btnColor.IsEnabled = false;
            btnIcon.IsEnabled = false;
            btnUpdateCategory.IsEnabled = false;
        }

        private void SetFieldsStatusAdd()
        {
            txtCategoryName.IsEnabled = true;
            btnAddCategory.IsEnabled = true;
            btnColor.IsEnabled = true;
            btnIcon.IsEnabled = true;
            btnAddCategory.IsEnabled = true;
            btnUpdateCategory.IsEnabled = false;
        }
        private void CreateColorPicker()
        {
            colorPicker = new ColorPickerDialog();
        }

        private void InsertChildOnCategoryGrid(UIElement child, int row, int col)
        {
            Grid.SetRow(child, row);
            Grid.SetColumn(child, col);
            grdCategory.Children.Add(child);
        }
        private void InsertCategoryName(string categoryName, int row)
        {
            Label lblCategoryName = new Label();
            lblCategoryName.Content = categoryName;
            lblCategoryName.Width = 10 * categoryName.Length;
            lblCategoryName.Height = 30;
            //lblCategoryName.HorizontalAlignment = HorizontalAlignment.Center;
            InsertChildOnCategoryGrid(lblCategoryName, row, 0);
        }
        private void InsertCategoryColor(string colorCode, int row)
        {
            SolidColorBrush brush;
            try
            {
                Color categoryColor = (Color) ColorConverter.ConvertFromString(colorCode);
                brush = new SolidColorBrush(categoryColor);
            } catch
            {
                brush = null;
            }

            Rectangle recColor = new Rectangle()
            {
                Width = 20,
                Height = 20,
                Fill = brush,
            };
            InsertChildOnCategoryGrid(recColor, row, 1);
        }

        private void InsertCategoryIcon(Category? category, int row)
        {
            try
            {
                BitmapImage bitmap;
                if (category.icon != null)
                {
                    bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.StreamSource = new MemoryStream(category.icon);
                    bitmap.EndInit();
                }
                else
                    bitmap = null;

                if (bitmap != null)
                {
                    Image icon = new Image();
                    icon.Source = bitmap;
                    icon.Stretch = Stretch.Uniform;
                    icon.Width = 20;
                    icon.Height = 20;
                    InsertChildOnCategoryGrid(icon, row, 2);
                }
            } catch
            {
                return;
            }
        }

        private void InsertEditionButton(Category? category, int row)
        {
            Button button = new Button();
            button.Content = "Editar";
            button.Height = 29;
            button.Width = 64;
            button.Tag = new Tuple<Category?, int>(category, row);
            button.Click += FillFieldsToEdit;
            InsertChildOnCategoryGrid(button, row, 3);
        }

        private void FillFieldsToEdit(object sender, EventArgs e)
        {
            Button clickedButton = (Button) sender;
            Tuple<Category?, int> tag = (Tuple<Category?, int>) clickedButton.Tag;
            Category? category = (Category?) tag.Item1;
            txtCategoryName.Text = category.name;
            btnUpdateCategory.IsEnabled = true;
            btnUpdateCategory.Click -= UpdateCategory;
            btnUpdateCategory.Click += UpdateCategory;
            btnUpdateCategory.Tag = tag;

            Color categoryColor = (Color) ColorConverter.ConvertFromString(category.color);
            recColor.Fill = new SolidColorBrush(categoryColor);
        }

        private void updateGridRow(Category? category, int row)
        {
            UIElementCollection childrenEnumerator = grdCategory.Children;
            for (int i = 0; i < childrenEnumerator.Count; i++)
            {
                if (Grid.GetRow(childrenEnumerator[i]) == row)
                {
                    grdCategory.Children.Remove(childrenEnumerator[i]);
                }
            }
            InsertCategoryOnGrid(category, row, null);
        }

        private void UpdateCategory(object sender, RoutedEventArgs e)
        {
            Button clickedButton = (Button) sender;
            Tuple<Category?, int> tag = (Tuple<Category?, int>) clickedButton.Tag;
            Category? category = (Category?) tag.Item1;
            long? categoryId = category.categoryId;

            bool ok = CheckFieldsUpdate(categoryId);
            if (ok)
            {
                Category? updatedCategory = categoryRepository.UpdateCategory(FormatFields(categoryId));
                if (updatedCategory != null)
                {
                    MessageBox.Show("Categoria atualizada com sucesso!");
                    updateGridRow(updatedCategory, tag.Item2);
                    ClearFields();
                    btnUpdateCategory.IsEnabled = false;
                }
                else
                    MessageBox.Show("Ocorreu um erro na atualização da categoria!");
            }
        }

        private void InsertCategoryOnGrid(Category? category, int row, double? heightDelta = 20)
        {
            grdCategory.RowDefinitions.Add(new RowDefinition());
            if (heightDelta != null)
                grdCategory.Height += (double) heightDelta; 
            InsertCategoryName(category.name, row);
            InsertCategoryColor(category.color, row);
            InsertCategoryIcon(category, row);
            InsertEditionButton(category, row);
        }

        private void CompleteCategoryGrid()
        {
            List<Category?> categories = categoryRepository.GetAll();
            int i = 1;
            foreach (Category? category in categories)
            { 
                InsertCategoryOnGrid(category, i);
                i += 1;
            }
        }

        private void InsertColumnNamesOnCategoryGrid()
        {
            string[] columns = { "NOME", "COR", "ÍCONE", "EDITAR" };
            grdCategory.RowDefinitions.Add(new RowDefinition());
            for (int i = 0; i < columns.Length; i++)
            {
                Label columnName = new Label();
                columnName.Content = columns[i];
                columnName.Width = 50;
                columnName.Height = 30;

                grdCategory.ColumnDefinitions.Add(new ColumnDefinition());
                Grid.SetRow(columnName, 0);
                Grid.SetColumn(columnName, i);
                grdCategory.Children.Add(columnName);
            }
        }
        private void insertContentIntoCategoryGrid()
        {
            InsertColumnNamesOnCategoryGrid();
            CompleteCategoryGrid();
        }
        private void updateCategoryGrid(Category? category)
        {
            if (category != null)
            {
                int rows = grdCategory.RowDefinitions.Count;
                InsertCategoryOnGrid(category, rows);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private byte[] GetByteListFromIcon(string iconPath)
        {
            FileStream fs = new FileStream(iconPath, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);

            byte[] iconBytes = br.ReadBytes((int) fs.Length);

            br.Close();
            fs.Close();
            br.Dispose();
            fs.Dispose();

            return iconBytes;
        }

        private void ClearFields()
        {
            txtCategoryName.Text = "";
            txtIcon.Text = "";
            recColor.Fill = null;
        }
        private bool CheckFieldsGeneral()
        {
            if (txtCategoryName.Text.Trim().Length == 0)
            {
                MessageBox.Show("Nome de categoria inválido!");
                return false;
            }
            return true;
        } 

        private bool CheckFieldsUpdate(long? categoryId)
        {
            if (CheckFieldsGeneral() == false)
                return false;

            Category? existingCategory = categoryRepository.GetByNameAndExcludeId(txtCategoryName.Text.Trim(), categoryId);
            if (existingCategory != null)
            {
                MessageBox.Show("Já existe uma categoria com o mesmo nome");
                return false;
            }

            return true;
        }

        private bool CheckFieldsAdd()
        {
            if (CheckFieldsGeneral() == false)
                return false;

            Category? existingCategory = categoryRepository.GetByName(txtCategoryName.Text.Trim());
            if (existingCategory != null)
            {
                MessageBox.Show("Já existe uma categoria com o mesmo nome");
                return false;
            }

            return true;
        }

        private string? CheckColor()
        {
            SolidColorBrush brush = (SolidColorBrush)recColor.Fill;
            if (brush != null)
                return brush.Color.ToString();
            return null;
        }

        private Category? FormatFields(long? categoryId = null)
        {
            byte[] icon;
            if (txtIcon.Text == "")
                icon = null;
            else
                icon = GetByteListFromIcon(txtIcon.Text);

            string? color = CheckColor();
            
            Category newCategory = new Category(
                categoryId: categoryId,
                name: txtCategoryName.Text,
                color: color,
                icon: icon
            );

            return newCategory;
        }

        private void AddCategory(object sender, RoutedEventArgs e)
        {
            bool ok = CheckFieldsAdd();
            if (ok)
            {
                Category? savedCategory = categoryRepository.AddCategory(FormatFields());
                if (savedCategory != null)
                {
                    MessageBox.Show("Categoria adicionada com sucesso!");
                    updateCategoryGrid(savedCategory);
                    ClearFields();
                    btnUpdateCategory.IsEnabled = false;
                }
                else
                    MessageBox.Show("Ocorreu um erro no cadastro da categoria!");
            }
        }

        private void ShowColorPicker(object sender, RoutedEventArgs e)
        {
            CreateColorPicker();
            colorPicker.ShowDialog();
            recColor.Fill = new SolidColorBrush(colorPicker.Color);
            colorPicker.Close();
        }

        private void SearchIcon(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.DefaultExt = ".png"; // Required file extension
            fileDialog.Filter = "Image files (*.png, *.bmp, *.jpg)|*.png;*.bmp;*.jpg";

            if (fileDialog.ShowDialog() == true)
                txtIcon.Text = fileDialog.FileName;
        }
        private void ClearFields(object sender, RoutedEventArgs e)
        {
            ClearFields();
        }
    }
}
