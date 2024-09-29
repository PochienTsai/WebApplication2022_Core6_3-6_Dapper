namespace WebApplication2022_Core6_3_6_Dapper.Models2
{

    // 請輸入  /UserDB2/IndexJOINMasterDetails?_ID=4  觀看結果
    // UserDB2控制器 底下的  IndexMasterDetails5 / IndexJOINMasterDetails動作
    public class UserDepartmentViewModel
    {    // 一個 UserTable 與 一個 DepartmentTable
        public UserTable2 UserVM { get; set; }

        public DepartmentTable2 DepartmentVM { get; set; }


        // 比較一下，跟另一個 ViewModel有何不同？（另一個為 UDViewModel.cs，一對多）
        //                                                                       （另一個為 UDmultiViewModel.cs，多對多）

    }

}
