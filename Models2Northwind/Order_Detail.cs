namespace WebApplication2022_Core6_3_6_Dapper.Models2Northwind
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;


    [Table("Order Details")]
    public partial class Order_Detail
    {
        [Key]   // 重點！！兩個欄位一起當成主索引鍵（PK）
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int OrderID { get; set; }

        [Key]   // 重點！！兩個欄位一起當成主索引鍵（PK）
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ProductID { get; set; }

        [Column(TypeName = "money")]
        public decimal UnitPrice { get; set; }

        public short Quantity { get; set; }

        public float Discount { get; set; }


        //==== 導覽屬性（Navigation Property）==========   與其他資料表之間的關連性。
        public virtual Order? Orders { get; set; }

        public virtual Product? Products { get; set; }
    }
}
