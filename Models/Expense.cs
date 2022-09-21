namespace FinalProject.Models
{
    internal class Expense
    {
        public long expenseId;
        public float value { get; set;}
        public string desc { get; set; }
        public long category_id { get; set; }

        public Expense(long expenseId, string value, string desc, long category_id)
        {
            this.expenseId = expenseId;
            this.value = value;
            this.desc = desc;
            this.category_id = category_id;
        }
    }
}