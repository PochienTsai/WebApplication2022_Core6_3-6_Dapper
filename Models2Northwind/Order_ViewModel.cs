namespace WebApplication2022_Core6_3_6_Dapper.Models2Northwind
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;


    [NotMapped] // �o�� Order_ViewModel���O�ɦb��Ʈw�W�S���A�ҥH�g�F[NotMapped] 
    public class Order_ViewModel
    {       
        public int OrderID { get; set; }
        public string CustomerID { get; set; }

        //*****************************************
        [NotMapped]    // �o�����b��Ʈw�W�S���A�ҥH�g�F[NotMapped] 
        public string CompanyName { get; set; }
        //*****************************************

        public DateTime? OrderDate { get; set; }
        

        //==== �����ݩʡ]Navigation Property�^==========   �P��L��ƪ��������s�ʡC
        //****************************************************************************
        [NotMapped]    // �o�� Order_Detail_ViewModel���O�ɦb��Ʈw�W�S���A�ҥH�g�F[NotMapped] 
        public virtual ICollection<Order_Detail_ViewModel> OD_VMs { get; set; }
        //****************************************************************************
    }
}
