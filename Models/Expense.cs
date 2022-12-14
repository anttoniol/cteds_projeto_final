using System;

namespace cteds_projeto_final.Models
{
    public class Expense
    {
        public long? expenseId;
        public decimal value { get; set;}
        public string desc { get; set; }
        public long category_id { get; set; }
        public DateTime? added_dttm { get; set; }
        public DateTime expense_dttm { get; set; }


        public Expense(decimal value, string desc, long category_id, DateTime expense_dttm, DateTime ? added_dttm = null, long? expenseId = null)
        {
            this.expenseId = expenseId;
            this.value = value;
            this.desc = desc;
            this.category_id = category_id;
            this.added_dttm = added_dttm;
            this.expense_dttm = expense_dttm;
        }
    }
}