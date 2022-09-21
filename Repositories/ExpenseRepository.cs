using System.Data.SQLite;

using FinalProject.Models;
using FinalProject.Repositories.Cat;

namespace FinalProject.Repositories
{
    public class ExpenseRepository 
    {
        private SQLiteConnection conn;

        public ExpenseRepository(SQLiteConnection connection, CategoryRepository category_repository)
        {
            conn = connection;
        }

        public Expense? GetById(long id)
        {
            string queryString = "SELECT * FROM expenses WHERE id = @id";
            using (SQLiteCommand cmd = new SQLiteCommand(queryString, conn))
            {
                cmd.Parameters.AddWithValue("@id", id);

                SQLiteDataReader rdr = cmd.ExecuteReader();

                if(rdr.Read())
                {
                    Expense expense = new Expense(
                        expenseId: (long)rdr["id"],
                        value: rdr["value"].ToFloat()!,
                        desc: rdr["desc"].ToString()!,
                        category: (long)rdr["category_id"],
                        added_dttm: rdr["added_dttm"].ToDateTime()!,
                    );
                    return expense;
                }
            }
            return null;
        }

        public Expense? GetByDate(DateTime start, DateTime end)
        {
            string queryString = "SELECT * FROM expenses WHERE added_dttm >= @start and added_dttm <= @end";
            using (SQLiteCommand cmd = new SQLiteCommand(queryString, conn))
            {
                cmd.Parameters.AddWithValue("@start", start);
                cmd.Parameters.AddWithValue("@end", end);

                SQLiteDataReader rdr = cmd.ExecuteReader();

                if(rdr.Read())
                {
                    Expense expense = new Expense(
                        expenseId: (long)rdr["id"],
                        value: rdr["value"].ToFloat()!,
                        desc: rdr["desc"].ToString()!,
                        category: (long)rdr["category_id"],
                        added_dttm: rdr["added_dttm"].ToDateTime()!,
                    );
                    return expense;
                }
            }
            return null;
        }

        public Expense? AddExpense(Expense expense)
        {
            string queryString = "INSERT into expenses (value, desc, category_id) values (@value, @desc, @category_id)";
            using (SQLiteCommand cmd = new SQLiteCommand(queryString, conn))
            {
                cmd.Parameters.AddWithValue("@value", expense.value);
                cmd.Parameters.AddWithValue("@desc", expense.desc);
                cmd.Parameters.AddWithValue("@category_id", expense.category_id);

                SQLiteDataReader rdr = cmd.ExecuteReader();

                if(rdr.Read())
                {
                    expense.expenseId = (long)rdr["id"];
                    expense.added_dttm = rdr["added_dttm"].ToDateTime()!;
                    return expense;
                }
            }
            return null;
        }

        public Expense? UpdateExpense(Expense expense)
        {
            string queryString = "UPDATE expenses set value = @value, desc = @desc, category_id = @category_id, added_dttm = @added_dttm where id = @id";
            using (SQLiteCommand cmd = new SQLiteCommand(queryString, conn))
            {
                cmd.Parameters.AddWithValue("@id", expense.id);
                cmd.Parameters.AddWithValue("@value", expense.value);
                cmd.Parameters.AddWithValue("@desc", expense.desc);
                cmd.Parameters.AddWithValue("@category_id", expense.category_id);
                cmd.Parameters.AddWithValue("@added_dttm", expense.added_dttm);

                SQLiteDataReader rdr = cmd.ExecuteReader();

                if(rdr.Read())
                {
                    expense.expenseId = (long)rdr["id"];
                    return expense;
                }
            }
            return null;
        }

        public Expense? DeleteExpense(Expense expense)
        {
            string queryString = "delete from categories WHERE id = @id";
            using (SQLiteCommand cmd = new SQLiteCommand(queryString, conn))
            {
                cmd.Parameters.AddWithValue("@id", expense.id);
                SQLiteDataReader rdr = cmd.ExecuteReader();

                if(rdr.Read())
                {
                    return null;
                }
            }
            return null;
        }
    }
}