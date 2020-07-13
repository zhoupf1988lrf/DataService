using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NetCore.Model.Models;
using NetCore.Model.SqlLog;
using NetCore.Utlity;

namespace NetCore.Model
{


    public partial class CustomeContext : DbContext
    {
        private IConfiguration _IConfiguration = null;
        public CustomeContext(IConfiguration configuration)
        {
            this._IConfiguration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // 1  new ConfigurationBuilder().SetBasePath(System.IO.Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build().GetConnectionString("CustomersConnectionString");
            //2 
            optionsBuilder.UseSqlServer(this._IConfiguration.GetConnectionString("CustomersConnectionString"));
            optionsBuilder.UseLoggerFactory(new CustomLoggerFactory());

            //  3 optionsBuilder.UseSqlServer("Server=.;Database=Customers;User id=sa;password=123456;");

            // 4 如果是静态类，无法使用IOC的注入IConfiguration
           //optionsBuilder.UseSqlServer(StaticConstraint.CustomConnection);
        }

        public virtual DbSet<Company> Companies { get; set; }
        public virtual DbSet<School> Schools { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<CityEntity> CityEntitys { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DBDriverEntity>().ToTable("DBDrivers");
            modelBuilder.Entity<School>()
                .Property(e => e.SchoolName)
                .IsFixedLength();

            modelBuilder.Entity<User>()
                .Property(e => e.Name)
                .IsFixedLength();

            modelBuilder.Entity<User>()
                .Property(e => e.Account)
                .IsFixedLength();

            modelBuilder.Entity<User>()
                .Property(e => e.Password)
                .IsFixedLength();

            modelBuilder.Entity<User>()
                .Property(e => e.Email)
                .IsFixedLength();

            modelBuilder.Entity<User>()
                .Property(e => e.Mobile)
                .IsFixedLength();

            modelBuilder.Entity<User>()
                .Property(e => e.CompanyName)
                .IsFixedLength();
        }
    }
}
