using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.Extensions.Logging;
using System.Linq;
//************************************************************
using WebApplication2022_Core6_3_6_Dapper.Models;
using WebApplication2022_Core6_3_6_Dapper.Models2;
using WebApplication2022_Core6_3_6_Dapper.Models2Northwind;

using Microsoft.EntityFrameworkCore;   // Async「非同步」會用到的命名空間
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;  // 讀取appsettings.json設定檔

using Dapper;  // *******************************************
//************************************************************



namespace WebApplication2022_Core6_3_6_Dapper.Controllers
{
    public class AdoNetController : Controller
    {
        //*************************************   連結 MVC_UserDB 資料庫  ********************************* (start)
        #region
        private readonly MVC_UserDB2Context _db = new MVC_UserDB2Context();
        // 如果沒寫上方的命名空間 --「專案名稱.Models」，就得寫成下面這樣，加註「Models.」字樣。
        // private Models.MVC_UserDB2Context _db = new Models.MVC_UserDB2Context();

        //// (1) 資料庫一旦開啟連線，用完就得要關閉連線與釋放資源。https://msdn.microsoft.com/zh-tw/library/system.web.mvc.controller_methods(v=vs.118).aspx
        //// 微軟的 .Net Core範例沒有這段寫法。仍可以使用，所以僅供參考。
        ////
        /////protected override void Dispose(bool disposing)
        //{   // 有開啟DB連結，就得動手關掉、Dispose這個資源。https://docs.microsoft.com/zh-tw/dotnet/api/microsoft.aspnetcore.mvc.controller.dispose?view=aspnetcore-5.0
        //    // 或是 官方網站的教材（程式碼）https://github.com/dotnet/AspNetCore.Docs/tree/main/aspnetcore/tutorials/first-mvc-app/start-mvc/sample
        //    if (disposing)
        //    {
        //        _db.Dispose();  //***這裡需要自己修改，例如 _db字樣
        //    }
        //    base.Dispose(disposing);
        //    // 資料庫一旦開啟連線，用完就得要關閉連線與釋放資源。
        //    // The base "Controller" class already implements the "IDisposable" interface, so this code simply adds an "override" to the 
        //    // "Dispose(bool)" method to explicitly dispose the context instance. 
        //    // ( "Dispose(bool)"方法標示為 virtual，所以可以用override覆寫。

        //    // "Controller" class  https://docs.microsoft.com/zh-tw/dotnet/api/microsoft.aspnetcore.mvc.controller
        //}

        //// (2) 如果找不到動作（Action）或是輸入錯誤的動作名稱，一律跳回首頁
        //// Controller的 HandleUnknownAction方法。  ** .NET Core 已經消失！無此寫法 **
        #endregion
        //*************************************   連結 MVC_UserDB 資料庫  ********************************* (end)


            private readonly ILogger<AdoNetController> _logger;
            public AdoNetController(ILogger<AdoNetController> logger, MVC_UserDB2Context context)
            {  //                                                                                          ****************************（自己動手加上）
                _logger = logger;
                _db = context;    //*****************************（自己動手加上）
                // https://blog.givemin5.com/asp-net-mvc-core-tao-bi-hen-jiu-di-yi-lai-zhu-ru-dependency-injection/
            }


        //====================================================
        //=== 下拉式選單（DropDownLinst）  +  ADO.NET
        //====================================================
        public IActionResult Create()
        {   // 檢視畫面上，有多種 下拉式選單（DropDownList）的寫法
            return View();
            //*** 重點！！加入檢視畫面時，務必勾選「參考指令碼程式庫」才能產生表單驗證
        }

        [HttpPost]
        [ValidateAntiForgeryToken]   // 避免XSS、CSRF攻擊，請配合檢視畫面，Html.BeginForm()表單裡的「@Html.AntiForgeryToken()」這一列程式
        public IActionResult Create(UserTable2 _userTable)
        {
            //**********************************************
            #region  //** 第一種作法。EF (Entity Framework)的作法。
            //// 程式碼 折疊（區塊關閉）
            //if (ModelState.IsValid)
            //{
            //    _db.UserTable2s.Add(_userTable);
            //    _db.SaveChanges();
            //}

            ////return Content(" 新增一筆記錄，成功！");    // 新增成功後，出現訊息（字串）。
            //return RedirectToAction("List");
            #endregion

            //**********************************************
            #region  //** 第二種作法。使用 ADO.NET做「資料新增」
            // 程式碼 折疊（區塊關閉）

            int RecordsAffected = 0;
            if (ModelState.IsValid)
            {   //== (1). 開啟資料庫的連結。 appsettings.json設定檔裡面的資料庫連結字串（ConnectionString），此連線名為 MVC_UserDB2
                var configurationBuilder = new ConfigurationBuilder().AddJsonFile("appsettings.json");
                //  讀取appsettings.json設定檔的內容。需搭配 Microsoft.Extensions.Configuration命名空間。
                IConfiguration config = configurationBuilder.Build();
                string connectionString = config["ConnectionStrings:DefaultConnection"];
                SqlConnection Conn = new SqlConnection(connectionString);
                //SqlConnection Conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MVC_UserDB2"].ConnectionString);  // 以前 .NET 4.8版的寫法，透過 Web.Config設定檔
                Conn.Open();   //---- 開啟連結。這時候才連結DB

                //== (2). 執行SQL指令。或是查詢、撈取資料。  
                //*** 參數（Parameter），可避免SQL Injection攻擊 ************** (start)
                string sqlstr = "INSERT INTO [UserTable2] ([UserName],[UserSex],[UserBirthDay],[UserMobilePhone],[UserApproved], [DepartmentId]) ";
                sqlstr += " VALUES (@UserName,@UserSex, @UserBirthDay, @UserMobilePhone, @UserApproved, @DepartmentId)";
                SqlCommand cmd = new SqlCommand(sqlstr, Conn);

                //-- 方法一。精簡版。
                cmd.Parameters.AddWithValue("@UserName", _userTable.UserName);
                cmd.Parameters.AddWithValue("@UserSex", _userTable.UserSex);
                cmd.Parameters.AddWithValue("@UserBirthDay", _userTable.UserBirthDay);
                cmd.Parameters.AddWithValue("@UserMobilePhone", _userTable.UserMobilePhone);
                cmd.Parameters.AddWithValue("@UserApproved", _userTable.UserApproved);
                cmd.Parameters.AddWithValue("@DepartmentId", _userTable.DepartmentId);

                //-- 方法二，另一種寫法，效率高！只寫一個參數作為示範。
                //cmd.Parameters.Add("@UserName", SqlDbType.NVarChar, 50)
                //cmd.Parameters("@UserName").Value = _userTable.UserName   ////-- 參考資料 http://msdn.microsoft.com/zh-tw/library/system.data.sqlclient.sqlparametercollection.addwithvalue.aspx
                //*** 參數（Parameter），可避免SQL Injection攻擊 **************  (end)

                //== (3). 自由發揮。
                RecordsAffected = cmd.ExecuteNonQuery();

                //== (4). 釋放資源、關閉資料庫的連結。
                cmd.Cancel();
                if (Conn.State == System.Data.ConnectionState.Open)
                {
                    Conn.Close();
                }
            }
            else
            {   // 需搭配檢視畫面的 @Html.ValidationSummary(true)
                // 並且 return View() 回到原本的新增畫面上，顯示錯誤訊息！
                ModelState.AddModelError("", "輸入資料有誤！");
            }
            return Content(" [ADO.NET] 執行 Insert Into的SQL指令以後，影響了" + RecordsAffected + "列的紀錄。");
            //-- 或者是，您可以這樣寫，代表有新增一些紀錄。
            //if (RecordsAffected > 0)  {
            //    return Content(" [ADO.NET] 資料新增成功。共有" + RecordsAffected + "列紀錄被影響。");
            // }
            #endregion
        }



        //=============================================
        //== 列表（Master） ==  // 使用 ADO.NET做「資料列表」
        //=============================================
        public IActionResult List()
        {
            //== 第一，連結資料庫。  appsettings.json設定檔裡面的資料庫連結字串（ConnectionString），此連線名為 MVC_UserDB2
            var configurationBuilder = new ConfigurationBuilder().AddJsonFile("appsettings.json");
            //  讀取appsettings.json設定檔的內容。需搭配 Microsoft.Extensions.Configuration命名空間。
            IConfiguration config = configurationBuilder.Build();
            string connectionString = config["ConnectionStrings:DefaultConnection"];
            SqlConnection Conn = new SqlConnection(connectionString);
            //SqlConnection Conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MVC_UserDB2"].ConnectionString);  // 以前 .NET 4.8版的寫法，透過 Web.Config設定檔
            Conn.Open();   //---- 開啟連結。這時候才連結DB

            //== 第二，執行SQL指令。
            SqlDataReader dr = null;
            SqlCommand cmd = new SqlCommand("Select * From [UserTable2]", Conn);
            dr = cmd.ExecuteReader();   //---- 執行SQL指令（Select，搜尋、查詢）取出資料

            //==第三，自由發揮，把執行後的結果呈現到畫面上。
            //==寫迴圈，展示每一筆記錄與其中的欄位==
            List<UserTable2> result = new List<UserTable2>();
            // 為何不用 IQueryable<T>來做呢？？ 請參考 https://stackoverflow.com/questions/434737/how-do-i-add-a-new-record-to-an-iqueryable-variable
            // Remember that an IQueryable is not a result set, it is a query.

            while (dr.Read())
            {    // 傳統方法，一筆一筆記錄（類別）加入List 裡面
                result.Add(new UserTable2
                {
                    UserId = Convert.ToInt32(dr["UserId"]),
                    UserName = dr["UserName"].ToString(),
                    UserSex = dr["UserSex"].ToString(),
                    UserBirthDay = Convert.ToDateTime(dr["UserBirthDay"]),
                    UserMobilePhone = dr["UserMobilePhone"].ToString(),
                    UserApproved = Convert.ToBoolean(dr["UserApproved"])
                });
            }

            // == 第四，釋放資源、關閉資料庫的連結。       
            if (dr != null)
            {
                cmd.Cancel();
                dr.Close();
            }
            if (Conn.State == System.Data.ConnectionState.Open)
            {
                Conn.Close();
            }

            return View(result);   //直接把 UserTables的全部內容 列出來
        }


        //=============================================
        //== 單一文章的內容（Details） ==  // 使用 ADO.NET做「資料列表」
        //      建議使用 Dateails2 動作
        //=============================================

        //**** 把資料填入 List 裡面（List<UserTable2>） ****  檢視畫面 需做一些修正，不然會報錯！
        // 重點！新增檢視的時候，請選擇「 List」樣版！！  因為本範例把資料填入 List 裡面（List<UserTable2>）
        public IActionResult Details(int? _ID)    // 網址 http://xxxxxx/AdoNet/Details?ID=1 
        {
            if (_ID == null)
            {   // 沒有輸入文章編號（ID），就會報錯 - Bad Request
                return Content("找不到這一份資料");
            }

            //== 第一，連結資料庫。  appsettings.json設定檔裡面的資料庫連結字串（ConnectionString），此連線名為 MVC_UserDB2
            var configurationBuilder = new ConfigurationBuilder().AddJsonFile("appsettings.json");
            //  讀取appsettings.json設定檔的內容。需搭配 Microsoft.Extensions.Configuration命名空間。
            IConfiguration config = configurationBuilder.Build();
            string connectionString = config["ConnectionStrings:DefaultConnection"];
            SqlConnection Conn = new SqlConnection(connectionString);
            //SqlConnection Conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MVC_UserDB2"].ConnectionString);  // 以前 .NET 4.8版的寫法，透過 Web.Config設定檔
            Conn.Open();   //---- 開啟連結。這時候才連結DB

            //== 第二，執行SQL指令。
            SqlDataReader dr = null;
            SqlCommand cmd = new SqlCommand("Select * From [UserTable2] Where UserId = @ID", Conn);
            cmd.Parameters.AddWithValue("@ID", _ID.ToString());

            dr = cmd.ExecuteReader();   //---- 執行SQL指令（Select，搜尋、查詢）取出資料

            //==第三，自由發揮，把執行後的結果呈現到畫面上。
            List<UserTable2> result = new List<UserTable2>();

            if (dr.Read())
            {    // （不是迴圈）有資料的話，將會讀取到一筆記錄            
                result.Add(new UserTable2
                {
                    UserId = Convert.ToInt32(dr["UserId"]),
                    UserName = dr["UserName"].ToString(),
                    UserSex = dr["UserSex"].ToString(),
                    UserBirthDay = Convert.ToDateTime(dr["UserBirthDay"]),
                    UserMobilePhone = dr["UserMobilePhone"].ToString(),
                    UserApproved = Convert.ToBoolean(dr["UserApproved"])
                });
            }

            // == 第四，釋放資源、關閉資料庫的連結。       
            if (dr != null)
            {
                cmd.Cancel();
                dr.Close();
            }
            if (Conn.State == System.Data.ConnectionState.Open)   // https://docs.microsoft.com/zh-tw/dotnet/api/system.data.connectionstate?view=net-6.0
            {
                Conn.Close();
            }

            return View(result);   // 把這一筆記錄呈現出來
            // 重點！新增檢視的時候，請選擇「 List」樣版！！  因為本範例把資料填入 List 裡面（List<UserTable2>） 
            // 如果您選擇「Details」樣版，就會出現錯誤，請看 DetailsError.cshtml。想想看是哪裡有錯？
        }


        //**** 把資料填入 UserTable2「類別」 裡面 ****   // 重點！新增檢視的時候，請選擇「 Details」樣版！！
        public ActionResult Details2(int? _ID)    // 網址 http://xxxxxx/AdoNet/Details2?ID=1 
        {
            if (_ID == null)
            {   // 沒有輸入文章編號（ID），就會報錯 - Bad Request
                return Content("找不到這一份資料");
            }


            //== 第一，連結資料庫。  appsettings.json設定檔裡面的資料庫連結字串（ConnectionString），此連線名為 MVC_UserDB2
            var configurationBuilder = new ConfigurationBuilder().AddJsonFile("appsettings.json");
            //  讀取appsettings.json設定檔的內容。需搭配 Microsoft.Extensions.Configuration命名空間。
            IConfiguration config = configurationBuilder.Build();
            string connectionString = config["ConnectionStrings:DefaultConnection"];
            SqlConnection Conn = new SqlConnection(connectionString);
            //SqlConnection Conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MVC_UserDB2"].ConnectionString);  // 以前 .NET 4.8版的寫法，透過 Web.Config設定檔
            Conn.Open();            //---- 開啟連結。這時候才連結DB

            //== 第二，執行SQL指令。
            SqlDataReader dr = null;
            SqlCommand cmd = new SqlCommand("Select * From [UserTable2] Where UserId = @ID", Conn);
            cmd.Parameters.AddWithValue("@ID", _ID.ToString());

            dr = cmd.ExecuteReader();   //---- 執行SQL指令（Select，搜尋、查詢）取出資料

            //==第三，自由發揮，把執行後的結果呈現到畫面上。
            //*** 與上一個範例不同之處 ********************************************(start)
            UserTable2 resultClass = new UserTable2();

            if (dr.Read())   // 不是迴圈，沒用到 while迴圈
            {    // 有資料的話，就讀取一筆記錄。並放入 UserTable2類別裡面            
                resultClass.UserId = Convert.ToInt32(dr["UserId"]);
                resultClass.UserName = dr["UserName"].ToString();
                resultClass.UserSex = dr["UserSex"].ToString();
                resultClass.UserBirthDay = Convert.ToDateTime(dr["UserBirthDay"]);
                resultClass.UserMobilePhone = dr["UserMobilePhone"].ToString();
                resultClass.UserApproved = Convert.ToBoolean(dr["UserApproved"]);
            }
            else
            {
                return Content("抱歉！沒有找到任何一筆記錄");
            }
            //*** 與上一個範例不同之處 ********************************************(end)

            // == 第四，釋放資源、關閉資料庫的連結。       
            if (dr != null)
            {
                cmd.Cancel();
                dr.Close();
            }
            if (Conn.State == System.Data.ConnectionState.Open)  // https://docs.microsoft.com/zh-tw/dotnet/api/system.data.connectionstate?view=net-6.0
            {
                Conn.Close();
            }

            return View(resultClass);   // 把這一筆記錄呈現出來
            // 重點！新增檢視的時候，請選擇「 Details」樣版！！

        }




        //=============================================
        //== 修改（編輯）畫面 ==  請沿用上方 Details2動作即可
        //     （資料填入 UserTable2類別 裡面）
        //=============================================
        public ActionResult Edit(int? _ID)    // 網址 http://xxxxxx/AdoNet/Edit?ID=1
        {
            if (_ID == null)
            {   // 沒有輸入文章編號（ID），就會報錯 - Bad Request
                return Content("找不到這一份資料");
            }

            #region  // 使用上方 Details2動作的程式(ADO.NET)，先列出這一筆的內容，給使用者確認
            //== 第一，連結資料庫。   appsettings.json設定檔裡面的資料庫連結字串（ConnectionString），此連線名為 MVC_UserDB2
            var configurationBuilder = new ConfigurationBuilder().AddJsonFile("appsettings.json");
            //  讀取appsettings.json設定檔的內容。需搭配 Microsoft.Extensions.Configuration命名空間。
            IConfiguration config = configurationBuilder.Build();
            string connectionString = config["ConnectionStrings:DefaultConnection"];
            SqlConnection Conn = new SqlConnection(connectionString);
            //SqlConnection Conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MVC_UserDB2"].ConnectionString);  // 以前 .NET 4.8版的寫法，透過 Web.Config設定檔        
            Conn.Open();   //---- 開啟連結。這時候才連結DB

            //== 第二，執行SQL指令。
            SqlDataReader dr = null;
            SqlCommand cmd = new SqlCommand("Select * From [UserTable2] Where UserId = @ID", Conn);
            cmd.Parameters.AddWithValue("@ID", _ID.ToString());

            dr = cmd.ExecuteReader();   //---- 執行SQL指令（Select，搜尋、查詢）取出資料

            //==第三，自由發揮，把執行後的結果呈現到畫面上。
            //*** 與上一個範例（Details）不同之處 ********************************************(start)
            UserTable2 resultClass = new UserTable2();

            if (dr.Read())   // 不是迴圈，沒用到 while迴圈
            {    // 有資料的話，就讀取 "一筆"記錄。並放入 UserTable2類別裡面            
                resultClass.UserId = Convert.ToInt32(dr["UserId"]);
                resultClass.UserName = dr["UserName"].ToString();
                resultClass.UserSex = dr["UserSex"].ToString();
                resultClass.UserBirthDay = Convert.ToDateTime(dr["UserBirthDay"]);
                resultClass.UserMobilePhone = dr["UserMobilePhone"].ToString();
                resultClass.UserApproved = Convert.ToBoolean(dr["UserApproved"]);
                resultClass.DepartmentId = Convert.ToInt32(dr["DepartmentId"]);
            }
            else
            {
                return Content("抱歉！沒有找到任何一筆記錄");
            }
            //*** 與上一個範例（Details）不同之處 ********************************************(end)

            // == 第四，釋放資源、關閉資料庫的連結。       
            if (dr != null)
            {
                cmd.Cancel();
                dr.Close();
            }
            if (Conn.State == System.Data.ConnectionState.Open)
            {
                Conn.Close();
            }
            #endregion


            // 檢視畫面上的「下拉式選單（DropDownList）」。
            // 直接連結一個資料表，當作 DropDownList的資料來源。
            #region   // 請參閱 DropDownList控制器，底下的 Create1動作。  ADO.NET版 請看 Create1A動作
            //var dt = _db.DepartmentTable2s.ToList();   // 這一列改成 ADO.NET程式，就能到資料庫撈取您要的數據

            ////*******************************************************************************
            ////*** 重點！！這裡需要用 第四個參數。當作「預設值（SelectedValue）」
            ////*******************************************************************************
            //SelectList listItems = new SelectList(dt, "DepartmentId", "DepartmentName", resultClass.DepartmentId);
            //// 先寫 <option>子選項的 value，再寫 text

            //ViewBag.DtListItem = listItems;
            ////*******************
            #endregion


            return View(resultClass);   // 把這一筆記錄呈現出來。   
            // 新增檢視的時候，請選擇「 Edit」樣版！！    

            // *** 重點！***        
            // 產生檢視畫面時，請參閱圖片檔 Edit.jpg的說明。
            // (1). 挑選下面（第四個）的「資料內容類別（UserDB2Context）」，DepartmentId 會變成「下拉式選單（Html.DropDownList）」
            // (2).  "不" 挑選下面的「資料內容類別」，保持空白的話。DepartmentId 就會變成 Html.EditorFor（詳見   Edit_比較版.cshtml）

            // *** 重點！***        
            // 產生檢視畫面之後，兩個重點 -- (1). CheckBox   (2). DropDownList
        }

        //== 修改（更新），回寫資料庫 ===============
        [HttpPost]
        [ValidateAntiForgeryToken]   // 避免XSS、CSRF攻擊
        // 參考資料 http://blog.kkbruce.net/2011/10/aspnet-mvc-model-binding6.html
        //public ActionResult Edit([Bind(Include = "UserId, UserName, UserSex, UserBirthDay, UserMobilePhone, UserApproved, DepartmentId")]UserTable2 _userTable)
        public ActionResult Edit(UserTable2 _userTable)
        {
            if (_userTable == null)
            {   // 沒有輸入內容，就會報錯 - Bad Request
                return Content("找不到這一份資料");
            }

            string sqlstr = null;

            if (ModelState.IsValid)
            {   //** 原本 EF的寫法 **********************************************************(start)
                //_db.Entry(_userTable).State = System.Data.Entity.EntityState.Modified;
                //_db.SaveChanges();
                //** 原本 EF的寫法 **********************************************************(end)

                // 以下的 ADO.NET程式可以取代上面這兩列。

                #region  //** 第二種作法。使用 ADO.NET類似「資料新增」的寫法
                // 程式碼 折疊（區塊關閉）

                int RecordsAffected = 0;
                //== (1). 開啟資料庫的連結。 appsettings.json設定檔裡面的資料庫連結字串（ConnectionString），此連線名為 MVC_UserDB2
                var configurationBuilder = new ConfigurationBuilder().AddJsonFile("appsettings.json");
                //  讀取appsettings.json設定檔的內容。需搭配 Microsoft.Extensions.Configuration命名空間。
                IConfiguration config = configurationBuilder.Build();
                string connectionString = config["ConnectionStrings:DefaultConnection"];
                SqlConnection Conn = new SqlConnection(connectionString);              
                //SqlConnection Conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MVC_UserDB2"].ConnectionString);  // 以前 .NET 4.8版的寫法，透過 Web.Config設定檔
                Conn.Open();

                //== (2). 執行SQL指令。或是查詢、撈取資料。  
                //*** 參數（Parameter），可避免SQL Injection攻擊 ****** (start)
                sqlstr = "Update [UserTable2] Set [UserName] = @UserName, [UserSex] = @UserSex, [UserBirthDay] = @UserBirthDay, [UserMobilePhone] = @UserMobilePhone, [UserApproved] = @UserApproved, [DepartmentId] = @DepartmentId ";
                sqlstr += " Where UserId = @UserId";
                SqlCommand cmd = new SqlCommand(sqlstr, Conn);

                //-- 方法一。精簡版。
                cmd.Parameters.AddWithValue("@UserName", _userTable.UserName);
                cmd.Parameters.AddWithValue("@UserSex", _userTable.UserSex);
                cmd.Parameters.AddWithValue("@UserBirthDay", _userTable.UserBirthDay);
                cmd.Parameters.AddWithValue("@UserMobilePhone", _userTable.UserMobilePhone);
                cmd.Parameters.AddWithValue("@UserApproved", _userTable.UserApproved);   // 1 為 true
                cmd.Parameters.AddWithValue("@DepartmentId", _userTable.DepartmentId);
                cmd.Parameters.AddWithValue("@UserId", _userTable.UserId);

                //-- 方法二，另一種寫法，效率高！只寫一個參數作為示範。
                //cmd.Parameters.Add("@UserName", SqlDbType.NVarChar, 50)
                //cmd.Parameters("@UserName").Value = _userTable.UserName
                //-- 參考資料 http://msdn.microsoft.com/zh-tw/library/system.data.sqlclient.sqlparametercollection.addwithvalue.aspx
                //*** 參數（Parameter），可避免SQL Injection攻擊 ******  (end)

                //== (3). 自由發揮。
                RecordsAffected = cmd.ExecuteNonQuery();

                //== (4). 釋放資源、關閉資料庫的連結。
                cmd.Cancel();
                if (Conn.State == System.Data.ConnectionState.Open)
                {
                    Conn.Close();
                }
                #endregion

                return Content(" [ADO.NET] 執行 Update的SQL指令以後，影響了" + RecordsAffected + "列的紀錄。");
                //return RedirectToAction("List");
            }
            else
            {
                return Content(" 更新失敗！！更新失敗！！ ");
            }
        }




    }
}
