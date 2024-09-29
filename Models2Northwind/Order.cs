using System;
using System.Collections.Generic;

namespace WebApplication2022_Core6_3_6_Dapper.Models2Northwind
{
    public partial class Order
    {
        public Order()
        {
            Order_Details = new HashSet<Order_Detail>();
        }

        public int OrderId { get; set; }
        public string? CustomerId { get; set; }
        public int? EmployeeId { get; set; }
        public DateTime? OrderDate { get; set; }
        public DateTime? RequiredDate { get; set; }
        public DateTime? ShippedDate { get; set; }
        public int? ShipVia { get; set; }
        public decimal? Freight { get; set; }
        public string? ShipName { get; set; }
        public string? ShipAddress { get; set; }
        public string? ShipCity { get; set; }
        public string? ShipRegion { get; set; }
        public string? ShipPostalCode { get; set; }
        public string? ShipCountry { get; set; }



        //==== 導覽屬性（Navigation Property）==========   與其他資料表之間的關連性。
        // 一對多。
        public virtual Customer? Customers { get; set; }


        // 一對多。一張訂單（Order）底下有很多訂購的項目（Order_Detail）
        public virtual ICollection<Order_Detail> Order_Details { get; set; }

        public virtual Shipper? Shippers { get; set; }
    }
}
