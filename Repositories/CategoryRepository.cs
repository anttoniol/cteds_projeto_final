using System.Data.SQLite;

using cteds_projeto_final.Models;

using System;
using System.Data;
using System.Collections.Generic;
using System.Windows;

namespace cteds_projeto_final.Repositories
{
    public class CategoryRepository 
    {
        private SQLiteConnection conn;

        public CategoryRepository(SQLiteConnection connection)
        {
            conn = connection;
        }

        private Category? ReadCategoryData(SQLiteDataReader rdr)
        {
            byte[]? icon;
            try
            {
                icon = (byte[]?) rdr["icon"];
            } catch
            {
                icon = null;
            }
            
            DateTime? added_dttm;
            try
            {
                added_dttm = Convert.ToDateTime(rdr["added_dttm"]);
            }
            catch
            {
                added_dttm = null;
            }

            Category category = new Category(
                name: rdr["name"].ToString()!,
                color: rdr["color"].ToString(),
                icon: icon,
                categoryId: (long?) rdr["id"],
                added_dttm: added_dttm
            );
            return category;
        }

        public List<Category?> GetAll()
        {
            List<Category?> categoryList = new List<Category?>();
            string queryString = "SELECT * FROM categories";
            using (SQLiteCommand cmd = new SQLiteCommand(queryString, conn))
            {
                SQLiteDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    Category? category = ReadCategoryData(rdr);
                    categoryList.Add(category);
                }
                return categoryList;
            }
        }

        public Category? GetById(long id)
        {
            string queryString = "SELECT * FROM categories WHERE id = @id";
            using (SQLiteCommand cmd = new SQLiteCommand(queryString, conn))
            {
                cmd.Parameters.AddWithValue("@id", id);
                SQLiteDataReader rdr = cmd.ExecuteReader();
                return ReadCategoryData(rdr);
            }
        }

        public Category? GetByNameAndExcludeId(string name, long? id)
        {
            string queryString = "SELECT * FROM categories WHERE name LIKE @name and id != @id";
            using (SQLiteCommand cmd = new SQLiteCommand(queryString, conn))
            {
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@id", id);
                SQLiteDataReader rdr = cmd.ExecuteReader();
                if (rdr.Read())
                    return ReadCategoryData(rdr);
                return null;
            }
        }

        public Category? GetByName(string name)
        {
            string queryString = "SELECT * FROM categories WHERE name LIKE @name";
            using (SQLiteCommand cmd = new SQLiteCommand(queryString, conn))
            {
                cmd.Parameters.AddWithValue("@name", name);
                SQLiteDataReader rdr = cmd.ExecuteReader();
                if (rdr.Read())
                    return ReadCategoryData(rdr);
                return null;
            }
        }

        public Category? AddCategory(Category category)
        {   
            string queryString = "INSERT into categories (name, color, icon, added_dttm) values (@name, @color, @icon, @added_dttm)";
            try
            {
                using (SQLiteCommand cmd = new SQLiteCommand(queryString, conn))
                {
                    cmd.Parameters.AddWithValue("@name", category.name);
                    cmd.Parameters.AddWithValue("@color", category.color);
                    cmd.Parameters.AddWithValue("@icon", category.icon);
                    cmd.Parameters.AddWithValue("@added_dttm", DateTime.Now);

                    int numberOfRowsInserted = cmd.ExecuteNonQuery();
                    if (numberOfRowsInserted > 0)
                        return GetByName(category.name);
                    return null;
                }
            } catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }

        public Category? UpdateCategory(Category category)
        {
            string queryString = "UPDATE categories set name = @name, color = @color, icon = @icon where id = @id";
            try
            {
                using (SQLiteCommand cmd = new SQLiteCommand(queryString, conn))
                {
                    cmd.Parameters.AddWithValue("@id", category.categoryId);
                    cmd.Parameters.AddWithValue("@name", category.name);
                    cmd.Parameters.AddWithValue("@color", category.color);
                    cmd.Parameters.AddWithValue("@icon", category.icon);

                    int numberOfRowsUpdated = cmd.ExecuteNonQuery();
                    if (numberOfRowsUpdated > 0)
                        return category;
                    return null;
                }
            } catch
            {
                return null;
            }
        }
    }
}