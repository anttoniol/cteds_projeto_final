using System.Data.SQLite;

using cteds_projeto_final.Models;

using System;

namespace cteds_projeto_final.Repositories
{
    public class CategoryRepository 
    {
        private SQLiteConnection conn;

        public CategoryRepository(SQLiteConnection connection)
        {
            conn = connection;
        }

        public Category? GetById(long id)
        {
            string queryString = "SELECT * FROM categories WHERE id = @id AND deleted_dttm = DATETIME('0')";
            using (SQLiteCommand cmd = new SQLiteCommand(queryString, conn))
            {
                cmd.Parameters.AddWithValue("@id", id);

                SQLiteDataReader rdr = cmd.ExecuteReader();

                if(rdr.Read())
                {
                    Category category = new Category(
                        categoryId: (long)rdr["id"],
                        name: rdr["name"].ToString()!,
                        deleted_dttm: Convert.ToDateTime(rdr["deleted_dttm"])!
                    );
                    return category;
                }
            }
            return null;
        }

        public Category? GetByName(string name)
        {
            string queryString = "SELECT * FROM categories WHERE name LIKE @name AND deleted_dttm = DATETIME('0')";
            using (SQLiteCommand cmd = new SQLiteCommand(queryString, conn))
            {
                cmd.Parameters.AddWithValue("@name", name);

                SQLiteDataReader rdr = cmd.ExecuteReader();

                if(rdr.Read())
                {
                    Category category = new Category(
                        categoryId: (long)rdr["id"],
                        name: rdr["name"].ToString()!,
                        deleted_dttm: Convert.ToDateTime(rdr["deleted_dttm"])
                    );
                    return category;
                }
            }
            return null;
        }

        public Category? AddCategory(Category category)
        {
            string queryString = "INSERT into categories (name) values (@name)";
            using (SQLiteCommand cmd = new SQLiteCommand(queryString, conn))
            {
                cmd.Parameters.AddWithValue("@name", category.name);

                SQLiteDataReader rdr = cmd.ExecuteReader();

                if(rdr.Read())
                {
                    category.categoryId = (long)rdr["id"];
                    category.deleted_dttm = Convert.ToDateTime(rdr["deleted_dttm"]);

                    return category;
                }
            }
            return null;
        }

        public Category? UpdateCategory(Category category)
        {
            string queryString = "UPDATE categories set name = @name where id = @id and deleted_dttm = DATETIME('0')";
            using (SQLiteCommand cmd = new SQLiteCommand(queryString, conn))
            {

                cmd.Parameters.AddWithValue("@id", category.categoryId);
                cmd.Parameters.AddWithValue("@name", category.name);

                SQLiteDataReader rdr = cmd.ExecuteReader();

                if(rdr.Read())
                {
                    category.categoryId = (long)rdr["id"];
                    category.deleted_dttm = Convert.ToDateTime(rdr["deleted_dttm"]);

                    return category;
                }
            }
            return null;
        }

        public Category? DeleteCategory(Category category)
        {
            string queryString = "UPDATE categories set deleted_dttm = DATETIME('now') where id = @id and deleted_dttm = DATETIME('0')";
            using (SQLiteCommand cmd = new SQLiteCommand(queryString, conn))
            {
                cmd.Parameters.AddWithValue("@id", category.name);

                SQLiteDataReader rdr = cmd.ExecuteReader();

                if(rdr.Read())
                {
                    category.categoryId = (long)rdr["id"];
                    category.deleted_dttm = Convert.ToDateTime(rdr["deleted_dttm"]);

                    return category;
                }
            }
            return null;
        }
    }
}