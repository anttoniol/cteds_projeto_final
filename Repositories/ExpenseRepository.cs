using System.Data.SQLite;

using cteds_projeto_final.Models;
using System;
using System.Collections.Generic;

namespace cteds_projeto_final.Repositories
{
    public class ExpenseRepository 
    {
        private SQLiteConnection conn;

        public ExpenseRepository(SQLiteConnection connection, CategoryRepository category_repository)
        {
            conn = connection;
        }

        public List<Expense> GetAll()
        {
            List<Expense> expenseList = new List<Expense>();
            string queryString = "SELECT * FROM expenses";
            using (SQLiteCommand cmd = new SQLiteCommand(queryString, conn))
            {
                SQLiteDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    Expense expense = new Expense(
                       expenseId: (long) rdr["id"],
                       value: float.Parse(rdr["value"].ToString()),
                       desc: rdr["desc"].ToString()!,
                       category_id: (long) rdr["category_id"],
                       added_dttm: Convert.ToDateTime(rdr["added_dttm"]),
                       expense_dttm: Convert.ToDateTime(rdr["expense_dttm"])
                    );
                    expenseList.Add(expense);
                }
                return expenseList;
            }
        }

        private Expense? ReadExpenseData(SQLiteDataReader rdr)
        {
            if (rdr.Read())
            {
                Expense expense = new Expense(
                    expenseId: (long) rdr["id"],
                    value: float.Parse(rdr["value"].ToString()),
                    desc: rdr["desc"].ToString()!,
                    category_id: (long) rdr["category_id"],
                    added_dttm: Convert.ToDateTime(rdr["added_dttm"]),
                    expense_dttm: Convert.ToDateTime(rdr["expense_dttm"])
                );
                return expense;
            }
            return null;
        }

        public Expense? GetById(long id)
        {
            string queryString = "SELECT * FROM expenses WHERE id = @id";
            using (SQLiteCommand cmd = new SQLiteCommand(queryString, conn))
            {
                cmd.Parameters.AddWithValue("@id", id);

                SQLiteDataReader rdr = cmd.ExecuteReader();
                return ReadExpenseData(rdr);
            }
        }

        public Expense? GetByDesc(string desc)
        {
            string queryString = "SELECT * FROM expenses WHERE desc = @desc";
            using (SQLiteCommand cmd = new SQLiteCommand(queryString, conn))
            {
                cmd.Parameters.AddWithValue("@desc", desc);

                SQLiteDataReader rdr = cmd.ExecuteReader();
                return ReadExpenseData(rdr);
            }
        }

        public Expense? GetByDate(DateTime start, DateTime end)
        {
            string queryString = "SELECT * FROM expenses WHERE added_dttm >= @start and added_dttm <= @end";
            using (SQLiteCommand cmd = new SQLiteCommand(queryString, conn))
            {
                cmd.Parameters.AddWithValue("@start", start);
                cmd.Parameters.AddWithValue("@end", end);

                SQLiteDataReader rdr = cmd.ExecuteReader();
                return ReadExpenseData(rdr);
            }
        }

        public Expense? AddExpense(Expense expense)
        {
            string queryString = "INSERT into expenses (value, desc, category_id, expense_dttm, added_dttm) values (@value, @desc, @category_id, @expense_dttm, @added_dttm)";
            using (SQLiteCommand cmd = new SQLiteCommand(queryString, conn))
            {
                cmd.Parameters.AddWithValue("@value", expense.value);
                cmd.Parameters.AddWithValue("@desc", expense.desc);
                cmd.Parameters.AddWithValue("@category_id", expense.category_id);
                cmd.Parameters.AddWithValue("@expense_dttm", expense.expense_dttm);
                cmd.Parameters.AddWithValue("@added_dttm", expense.added_dttm);

                int numberOfRowsInserted = cmd.ExecuteNonQuery();
                if (numberOfRowsInserted > 0)
                    return GetByDesc(expense.desc);
                return null;
            }
        }

        public Expense? UpdateExpense(Expense expense)
        {
            string queryString = "UPDATE expenses set value = @value, desc = @desc, category_id = @category_id, expense_dttm = @expense_dttm where id = @id";
            using (SQLiteCommand cmd = new SQLiteCommand(queryString, conn))
            {
                cmd.Parameters.AddWithValue("@id", expense.expenseId);
                cmd.Parameters.AddWithValue("@value", expense.value);
                cmd.Parameters.AddWithValue("@desc", expense.desc);
                cmd.Parameters.AddWithValue("@category_id", expense.category_id);
                cmd.Parameters.AddWithValue("@expense_dttm", expense.expense_dttm);

                int numberOfRowsInserted = cmd.ExecuteNonQuery();
                if (numberOfRowsInserted > 0)
                    return expense;
                return null;
            }
        }

        public Expense? DeleteExpense(Expense expense)
        {
            string queryString = "delete from categories WHERE id = @id";
            using (SQLiteCommand cmd = new SQLiteCommand(queryString, conn))
            {
                cmd.Parameters.AddWithValue("@id", expense.expenseId);
                SQLiteDataReader rdr = cmd.ExecuteReader();

                if(rdr.Read())
                    return expense;
                return null;
            }
        }
    }
}