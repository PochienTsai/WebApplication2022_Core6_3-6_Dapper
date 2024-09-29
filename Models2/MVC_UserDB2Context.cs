using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace WebApplication2022_Core6_3_6_Dapper.Models2
{
    public partial class MVC_UserDB2Context : DbContext
    {
        public MVC_UserDB2Context()
        {
        }

        public MVC_UserDB2Context(DbContextOptions<MVC_UserDB2Context> options)
            : base(options)
        {
        }

        //*************************************************************************************************************
        public virtual DbSet<DepartmentTable2> DepartmentTable2s { get; set; } = null!;     // 這裡的名稱是複數（s）
        public virtual DbSet<UserTable2> UserTable2s { get; set; } = null!;   // 這裡的名稱是複數（s）
        // (1) UserTable2 表示 資料表裡面的「一筆記錄」。
        // (2) DbSet<UserTable2> 表示 「UserTable資料表」。
        // (3) virtual的用意？？
        // 答： Navigation properties are typically defined as "virtual" 
        //          so that they can take advantage of certain Entity Framework functionality such as "lazy loading". 
        //*************************************************************************************************************

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
//                optionsBuilder.UseSqlServer("Server=.\\sqlexpress;Database=MVC_UserDB2;Trusted_Connection=True;MultipleActiveResultSets=true");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DepartmentTable2>(entity =>
            {
                entity.HasKey(e => e.DepartmentId);

                //entity.HasMany(e => e.UserTable2s);  //******  自己手動添加的（比對 .NET 4.x版MVC的成果）  

                entity.ToTable("DepartmentTable2");

                entity.Property(e => e.DepartmentName).HasMaxLength(50);

                entity.Property(e => e.DepartmentYear).HasMaxLength(50);
            });

            modelBuilder.Entity<UserTable2>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .HasName("PK_UserTable");

                entity.ToTable("UserTable2");

                entity.Property(e => e.UserBirthDay).HasColumnType("datetime");

                entity.Property(e => e.UserMobilePhone).HasMaxLength(15);

                entity.Property(e => e.UserName).HasMaxLength(50);

                entity.Property(e => e.UserSex)
                    .HasMaxLength(1)
                    .HasDefaultValueSql("(N'M')")
                    .IsFixedLength();

                //*****************************************************************
                entity.HasOne(d => d.DepartmentTable2s)    // *** 自行修改 ***     一個科系  有多名學生（UserTable2）
                    .WithMany(p => p.UserTable2s)
                    .HasForeignKey(d => d.DepartmentId)
                    .HasConstraintName("FK_UserTable2_DepartmentTable2");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
