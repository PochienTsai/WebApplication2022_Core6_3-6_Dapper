using System;
using System.Collections.Generic;

namespace WebApplication2022_Core6_3_6_Dapper.Models2Northwind
{
    public partial class Supplier
    {
        public Supplier()
        {
            Products = new HashSet<Product>();
        }

        public int SupplierId { get; set; }
        public string CompanyName { get; set; } = null!;
        public string? ContactName { get; set; }
        public string? ContactTitle { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? Region { get; set; }
        public string? PostalCode { get; set; }
        public string? Country { get; set; }
        public string? Phone { get; set; }
        public string? Fax { get; set; }
        public string? HomePage { get; set; }


        //==== 導覽屬性（Navigation Property）==========   與其他資料表之間的關連性。

        // 一對多。
        public virtual ICollection<Product> Products { get; set; }
    }
}
