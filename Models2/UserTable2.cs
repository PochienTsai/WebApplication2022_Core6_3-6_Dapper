using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;     // 每個欄位上方的 [ ]符號，就得搭配這個命名空間（NuGet自行安裝）
using System.ComponentModel.DataAnnotations.Schema;


namespace WebApplication2022_Core6_3_6_Dapper.Models2
{
    public partial class UserTable2
    {
        [Key]    // 主索引鍵（P.K.）。   一個科系  有多名學生（UserTable2）
        public int UserId { get; set; }
        public string? UserName { get; set; }
        public string? UserSex { get; set; }
        public DateTime? UserBirthDay { get; set; }
        public string? UserMobilePhone { get; set; }

        //***** 新增的資料表欄位（類別的屬性） ******************************
        [Display(Name = "帳號已啟用？（UserApproved）")]
        public bool UserApproved { get; set; }

        public int? DepartmentId { get; set; }


        //*************************************************************************
        //*** 導覽屬性（navigation property.）***
        // https://docs.microsoft.com/zh-tw/dotnet/framework/data/adonet/ef/language-reference/query-expression-syntax-examples-navigating-relationships

        //************************************************************************ 一個科系  有多名學生（UserTable2）
        public virtual DepartmentTable2? DepartmentTable2s { get; set; }  // 注意！後面是複數（s） ***自行修改***
        // (1) virtual的用意？？
        // 答： Navigation properties are typically defined as "virtual" 
        //         so that they can take advantage of certain Entity Framework functionality such as "lazy loading". 
    }
}
