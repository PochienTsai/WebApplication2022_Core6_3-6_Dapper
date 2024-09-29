using System;
using System.Collections.Generic;
//**************************************************
using System.ComponentModel.DataAnnotations;


#nullable disable

namespace WebApplication2022_Core6_3_6_Dapper.Models
{
    public partial class UserTable
    {
        //[Key]    // 搭配 System.ComponentModel.DataAnnotations; 命名空間
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string UserSex { get; set; }
        public DateTime? UserBirthDay { get; set; }
        public string UserMobilePhone { get; set; }
    }
}
