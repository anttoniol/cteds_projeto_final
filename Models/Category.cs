using System;

namespace FinalProject.Models
{
    internal class Category
    {
        public long categoryId;
        public float name { get; set;}
        public DateTime deleted_dttm { get; set; }

        public Category(long categoryId, string name, DateTime deleted_dttm)
        {
            this.categoryId = categoryId;
            this.name = name;
            this.deleted_dttm = deleted_dttm;
        }

    }
}