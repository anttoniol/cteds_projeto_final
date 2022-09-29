using System;

namespace cteds_projeto_final.Models
{
    public class Expense
    {
        public long? expenseId;
        public float value { get; set;}
        public string desc { get; set; }
        public long category_id { get; set; }
        public System.DateTime added_dttm { get; set; }


        public Expense(float value, string desc, long category_id, DateTime added_dttm, long? expenseId = null)
        {
            this.expenseId = expenseId;
            this.value = value;
            this.desc = desc;
            this.category_id = category_id;
            this.added_dttm = added_dttm;

        }
    }
}