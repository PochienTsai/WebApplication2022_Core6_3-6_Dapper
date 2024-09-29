using System;
using System.Collections.Generic;

namespace WebApplication2022_Core6_3_6_Dapper.Models2Northwind
{
    public partial class Shipper
    {
        public Shipper()
        {
            Orders = new HashSet<Order>();
        }

        public int ShipperId { get; set; }
        public string CompanyName { get; set; } = null!;
        public string? Phone { get; set; }


        //==== 導覽屬性（Navigation Property）==========   與其他資料表之間的關連性。

        // 一對多。
        public virtual ICollection<Order> Orders { get; set; }
    }
}
