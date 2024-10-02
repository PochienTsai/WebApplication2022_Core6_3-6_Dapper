using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration; // ConfigurationBuilder會用到這個命名空間
using Microsoft.Extensions.DependencyInjection;
using WebApplication2022_Core6_3_6_Dapper;
using WebApplication2022_Core6_3_6_Dapper.Models; // ADOnet範例放在 /Models目錄下
using WebApplication2022_Core6_3_6_Dapper.Models2; // 本範例放在 /Models2目錄下
using WebApplication2022_Core6_3_6_Dapper.Models2Northwind; // 本範例放在 /Models2Northwind目錄下

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//**** 讀取 appsettings.json 設定檔裡面的資料（資料庫連結字串）****
////作法一：
//builder.Services.AddDbContext<MVC_UserDB2Context>(options => options.UseSqlServer("Server=.\\sqlexpress;Database=MVC_UserDB;Trusted_Connection=True;Encrypt=False;TrustServerCertificate=False;;MultipleActiveResultSets=true"));
//builder.Services.AddDbContext<MVC_UserDB2Context>(options => options.UseSqlServer("Server=.\\sqlexpress;Database=MVC_UserDB2;Trusted_Connection=True;Encrypt=False;TrustServerCertificate=False;;MultipleActiveResultSets=true"));
//builder.Services.AddDbContext<NorthwindContext>(options => options.UseSqlServer("Server=.\\sqlexpress;Database=Northwind;Trusted_Connection=True;Encrypt=False;TrustServerCertificate=False;;MultipleActiveResultSets=true"));
//// 這裡需要新增兩個命名空間，請使用「顯示可能的修正」讓系統自己加上。
//// System.ComponentModel.Win32Exception (0x80090325): 此憑證鏈結是由不受信任的授權單位發出的。https://devmanna.blogspot.com/2019/02/aspnet-mvc-sqlexception.html

////作法二： 讀取設定檔的內容
//// 資料來源  程式碼  https://github.com/CuriousDrive/EFCore.AllDatabasesConsidered/blob/main/MS%20SQL%20Server/Northwind.MSSQL/Program.cs
builder.Services.AddDbContext<MVC_UserDBContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection1"));
    options.EnableDetailedErrors(true);
});
builder.Services.AddDbContext<MVC_UserDB2Context>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
    options.EnableDetailedErrors(true);
});
builder.Services.AddDbContext<NorthwindContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnectionNorthwind"));
    options.EnableDetailedErrors(true);
});

////作法三： 讀取設定檔的內容
//var configurationBuilder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
//IConfiguration config = configurationBuilder.Build();   // ConfigurationBuilder會用到 Microsoft.Extensions.Configuration命名空間
//string DBconnectionString = config["ConnectionStrings:DefaultConnection"];  // appsettings.josn檔裡面的「資料庫連結字串」
//builder.Services.AddDbContext<MVC_UserDB2Context>(options => options.UseSqlServer(DBconnectionString));

//======================
//使用完成，會"自動"關閉資料庫連線。 https://docs.microsoft.com/zh-tw/aspnet/core/data/ef-mvc/crud?view=aspnetcore-5.0
//======================
// 您會呼叫 .AddDbContext() 擴充方法 來在 ASP.NET Core DI 容器中佈建 DbContext 類別。
// 根據預設，該方法會將服務存留期設定為 Scoped。
// Scoped 表示內容物件的存留期會與 Web 要求的存留期保持一致，並且在 Web 要求結束時會自動呼叫 Dispose 方法。
//********************************************************************



//=== 分 隔 線 ===============================================================

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
