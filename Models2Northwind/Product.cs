using System;
using System.Collections.Generic;

namespace WebApplication2022_Core6_3_6_Dapper.Models2Northwind
{
    public partial class Product
    {
        public Product()
        {
            Order_Details = new HashSet<Order_Detail>();
        }

        public int ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public int? SupplierId { get; set; }
        public int? CategoryId { get; set; }
        public string? QuantityPerUnit { get; set; }
        public decimal? UnitPrice { get; set; }
        public short? UnitsInStock { get; set; }
        public short? UnitsOnOrder { get; set; }
        public short? ReorderLevel { get; set; }
        public bool Discontinued { get; set; }



        //==== 導覽屬性（Navigation Property）==========  與其他資料表之間的關連性。
        // 一對多。
        public virtual ICollection<Order_Detail> Order_Details { get; set; }

        public virtual Supplier? Suppliers { get; set; }
    }
}
