using FluentMigrator;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace OnlineClinic.Data.Migrations
{
    [Migration(2)]
    public class CreateTabels : Migration
    {
        public override void Down()
        {
            
        }

        private void createCustomer()
        {
            Create.Table("customers")
           .WithColumn("Id").AsInt32().PrimaryKey().Identity()
           .WithColumn("UserName").AsString(256).Nullable()
           .WithColumn("NormalizedUserName").AsString(256).Nullable()
           .WithColumn("Email").AsString(256).Nullable()
           .WithColumn("NormalizedEmail").AsString(256).Nullable()
           .WithColumn("EmailConfirmed").AsBoolean().NotNullable()
           .WithColumn("PasswordHash").AsString().Nullable()
           .WithColumn("SecurityStamp").AsString().Nullable()
           .WithColumn("ConcurrencyStamp").AsString().Nullable()
           .WithColumn("PhoneNumber").AsString().Nullable()
           .WithColumn("PhoneNumberConfirmed").AsBoolean().NotNullable()
           .WithColumn("TwoFactorEnabled").AsBoolean().NotNullable()
           .WithColumn("LockoutEnd").AsDateTime().Nullable()
           .WithColumn("LockoutEnabled").AsBoolean().NotNullable()
           .WithColumn("AccessFailedCount").AsInt32().NotNullable()
           .WithColumn("Name").AsString().NotNullable()
           .WithColumn("Discriminator").AsString().NotNullable();

            Create.Table("AspNetRoles")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("Name").AsString(256).Nullable()
                .WithColumn("NormalizedName").AsString().Nullable()
                .WithColumn("ConcurrencyStamp").AsInt32().Nullable();

            Create.Table("AspNetUserRoles")
                .WithColumn("UserId").AsInt32().NotNullable()
                .WithColumn("RoleId").AsInt32().NotNullable()
                .ForeignKey("FK_AspNetUserRoles_Customers", "Customers", "Id")
                .ForeignKey("FK_AspNetUserRoles_AspNetRoles", "AspNetRoles", "Id");

            Create.Table("AspNetRolesClaims")
             .WithColumn("Id").AsInt32().PrimaryKey().Identity()
             .WithColumn("RoleId").AsInt32().NotNullable()
             .WithColumn("ClaimType").AsString(256).Nullable()
             .WithColumn("ClaimValue").AsInt32().Nullable()
             .ForeignKey("FK_AspNetRolesClaims_AspNetRoles", "AspNetRoles", "Id");

            Create.Table("AspNetUserClaims")
             .WithColumn("Id").AsInt32().PrimaryKey().Identity()
             .WithColumn("UserId").AsInt32().NotNullable()
             .WithColumn("ClaimType").AsString().Nullable()
             .WithColumn("ClaimValue").AsInt32().Nullable()
             .ForeignKey("FK_AspNetRolesClaims_Customers", "Customers", "Id");

            Create.Table("AspNetUserLogins")
           .WithColumn("LoginProvider").AsString(256).PrimaryKey()
           .WithColumn("ProviderKey").AsString(256).PrimaryKey()
           .WithColumn("ProviderDisplayName").AsString().Nullable()
           .WithColumn("UserId").AsInt32().NotNullable()
           .ForeignKey("FK_AspNetUserLogins_Customers", "Customers", "Id");


            Create.Table("AspNetUserTokens")
           .WithColumn("UserId").AsInt32().PrimaryKey()
           .WithColumn("LoginProvider").AsString(256).PrimaryKey()
           .WithColumn("Name").AsString(256).PrimaryKey()
           .WithColumn("Value").AsInt32().Nullable()
           .ForeignKey("FK_AspNetUserTokens_Customers", "Customers", "Id");
        }

        private void createTabels()
        {
               Create.Table("Doctors")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("Name").AsString().NotNullable()
                .WithColumn("EmailAddress").AsString().NotNullable()
                .WithColumn("PhoneNumber").AsString().NotNullable();

               Create.Table("Services")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("Name").AsString().NotNullable()
                .WithColumn("Description").AsString().NotNullable()
                .WithColumn("Time").AsInt32().NotNullable()
                .WithColumn("Price").AsDouble().NotNullable();

               Create.Table("Schedules")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("DoctorId").AsInt32().ForeignKey("Doctors", "Id").NotNullable()
                .WithColumn("Day").AsString().NotNullable()
                .WithColumn("Start_Time").AsTime().NotNullable()
                .WithColumn("End_Time").AsTime().NotNullable();

            Create.Table("Appointments")
             .WithColumn("Id").AsInt32().PrimaryKey().Identity()
             .WithColumn("CustomerId").AsInt32().ForeignKey("Customers", "Id").NotNullable()
             .WithColumn("ServiceId").AsInt32().ForeignKey("Services", "Id").NotNullable()
             .WithColumn("DoctorId").AsInt32().ForeignKey("Doctors", "Id").NotNullable()
             .WithColumn("TotalAmount").AsDouble().NotNullable();

               Create.Table("DoctorServices")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("ServiceId").AsInt32().ForeignKey("Services", "Id").NotNullable()
                .WithColumn("DoctorId").AsInt32().ForeignKey("Doctors", "Id").NotNullable();

        }

        private void insertTabels()
        {
            Insert.IntoTable("Doctors").Row(new { Name = "Dr. Mihai Georgescu", EmailAddress = "mihai.georgescu@example.com", PhoneNumber = "0734567890" });
            Insert.IntoTable("Doctors").Row(new { Name = "Dr. Ana Marinescu", EmailAddress = "ana.marinescu@example.com", PhoneNumber = "0745678901" });

            Insert.IntoTable("Services").Row(new { Name = "General Check-up", Description = "Routine general health check-up", Time = 30, Price = 100.00 });
            Insert.IntoTable("Services").Row(new { Name = "Blood Test", Description = "Basic blood test", Time = 45, Price = 80.00 });
            Insert.IntoTable("Services").Row(new { Name = "X-ray Examination", Description = "Diagnostic X-ray examination", Time = 60, Price = 150.00 });
            Insert.IntoTable("Services").Row(new { Name = "MRI Scan", Description = "MRI scan for detailed imaging", Time = 60, Price = 300.00 });
            Insert.IntoTable("Services").Row(new { Name = "Physical Therapy Session", Description = "Physical therapy session for rehabilitation", Time = 45, Price = 120.00 });

           // Insert.IntoTable("Appointments").Row(new { CustomerId = 1, ServiceId = 1, DoctorId = 1, TotalAmount = 100.00 });
           // Insert.IntoTable("Appointments").Row(new { CustomerId = 2, ServiceId = 2, DoctorId = 2, TotalAmount = 200.00 });

            Insert.IntoTable("DoctorServices").Row(new { ServiceId = 1, DoctorId = 1 });
            Insert.IntoTable("DoctorServices").Row(new { ServiceId = 2, DoctorId = 2 });
            Insert.IntoTable("DoctorServices").Row(new { ServiceId = 3, DoctorId = 1 });
            Insert.IntoTable("DoctorServices").Row(new { ServiceId = 4, DoctorId = 2 });

        }

        public override void Up()
        {

            createCustomer();
            createTabels();

            insertTabels();

        }
    }
}
