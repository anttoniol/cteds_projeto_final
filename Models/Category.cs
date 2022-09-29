using System;

namespace cteds_projeto_final.Models
{
    public class Category
    {
        public long? categoryId;
        public string name { get; set;}
        public DateTime? deleted_dttm { get; set; }

        public Category(string name, long? categoryId = null, DateTime? deleted_dttm = null)
        {
            this.categoryId = categoryId;
            this.name = name;
            this.deleted_dttm = deleted_dttm; 
        }

    }
}