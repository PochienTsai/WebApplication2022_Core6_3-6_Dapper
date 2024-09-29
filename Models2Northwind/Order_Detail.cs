namespace WebApplication2022_Core6_3_6_Dapper.Models2Northwind
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;


    [Table("Order Details")]
    public partial class Order_Detail
    {
        [Key]   // ���I�I�I������@�_���D������]PK�^
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int OrderID { get; set; }

        [Key]   // ���I�I�I������@�_���D������]PK�^
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ProductID { get; set; }

        [Column(TypeName = "money")]
        public decimal UnitPrice { get; set; }

        public short Quantity { get; set; }

        public float Discount { get; set; }


        //==== �����ݩʡ]Navigation Property�^==========   �P��L��ƪ��������s�ʡC
        public virtual Order? Orders { get; set; }

        public virtual Product? Products { get; set; }
    }
}
