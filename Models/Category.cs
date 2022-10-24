using System;

namespace cteds_projeto_final.Models
{
    public class Category
    {
        public long? categoryId;
        public string name { get; set;}
        public string color { get; set; }
        public byte[] icon { get; set; }
        public DateTime? added_dttm { get; set; }

        public Category(string name, string color = null, byte[] icon = null, long? categoryId = null, DateTime? added_dttm = null)
        {
            this.name = name;
            this.color = color;
            this.icon = icon;
            this.categoryId = categoryId;
            this.added_dttm = added_dttm;
        }
    }
}